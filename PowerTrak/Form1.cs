using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;
using Emgu.CV.Reg;
using System.Threading.Tasks;

namespace PowerTrak
{
    public partial class Form1 : Form
    {
        VideoCapture capture = null;
        double realFPS;
        double avgFrameDuration;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }

        private void getVideo_Click(object sender, System.EventArgs e) {
            try {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Video Files (*.mp4;)|*.mp4";

                if (dialog.ShowDialog()==DialogResult.OK)
                {
                    capture = new VideoCapture(dialog.FileName);
                    if (capture == null) return;

                    // initializes empty matrix frame
                    Mat frame = new Mat();
                    Stopwatch sw = new Stopwatch();
                    BarTracker barTracker = new BarTracker();

                    int prevY = 0;
                    int frameCount = (int)capture.Get(CapProp.FrameCount);
                    Image<Gray, byte>[] hueMasks = new Image<Gray, byte>[frameCount];

                    Mat[] imageArray = GetVideoFrames(hueMasks, frameCount);
                    Console.WriteLine(hueMasks.Length);
                    for (int i = 0; i < imageArray.Length; i++)
                    {
                        Mat mat = imageArray[i];
                        using (mat)
                        {
                            sw.Restart();
                            prevY = FindBar(hueMasks[i], mat.ToImage<Bgr, byte>(), barTracker, prevY);

                            double dur = sw.ElapsedMilliseconds;
                            pictureBox1.Refresh();

                            Console.WriteLine(sw.ElapsedMilliseconds);
                            Thread.Sleep(15);
                            //if (dur.ElapsedMilliseconds < 16) Thread.Sleep(16 - (int)Math.Floor((double)dur.ElapsedMilliseconds));
                        }
                    }
                    Console.WriteLine(barTracker.pauseY);
                    Console.WriteLine($"Tempo Time: {barTracker.tempoTime}, Pause Time: {barTracker.pauseTime}");
                }
            }

            catch (Exception ex) { 
                MessageBox.Show(ex.Message);
            }
        }

        internal int FindBar(Image<Gray, byte> hueMask, Image<Bgr, byte> hsv, BarTracker barTracker, int prevY)
        {
            // Convert the image to HSV
            // "using" keyword disposes object after code block is run
            using (hueMask)
            {
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                // grabs extreme outer contours by endpoints (compresses horizontal, vertical, and diagonal segments)
                CvInvoke.FindContours(hueMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                int minArea = 10000;
                int maxArea = 10000000;

                for (int i = 0; i < contours.Size; i++)
                {
                    // returns rectangle for set of points
                    var bbox = CvInvoke.BoundingRectangle(contours[i]);
                    var area = bbox.Width * bbox.Height;
                    var ar = (float)bbox.Width / bbox.Height;

                    // bounding wide rectangle instead?
                    if (area > minArea && area < maxArea && ar < 1.0)
                    {
                        // Generates rectangle to the frame
                        CvInvoke.Rectangle(hsv, bbox, new MCvScalar(0, 0, 255), 2);

                        // Check unrack status (ignoring atm)
                        // start downwards tracking
                        // method should end tempo timer when difiference between frame Y is 0?
                        Console.WriteLine($"Current Y: {bbox.Y}, Prev Y: {prevY}, Pause Y: {barTracker.pauseY}");
                        if (barTracker.UnrackStatus() && bbox.Y >= prevY) prevY = barTracker.TrackDownwards(prevY, bbox, 5);

                        // if current Y - pauseY > tolerance and pauseY != 0
                        // start upwards tracking which ends pause timer
                        int pauseY = barTracker.pauseY;
                        if (pauseY != 0 && pauseY - bbox.Y > 5) barTracker.TrackUpwards(pauseY, bbox);
                    }
                }
                pictureBox1.Image = hueMask.ToBitmap();
                //pictureBox1.Image = hsv.ToBitmap();
            }
            return prevY;
        }

        private Mat[] GetVideoFrames(Image<Gray, byte>[] hueMasks, int frameCount)
        {
            // switch to Image since preprocessing returns hsv frames
            Mat[] imageArray = new Mat[frameCount];
            Mat frame = new Mat();
            int frameNumber = 0;
            Stopwatch durSW = new Stopwatch();

            for (int i = 0; i < frameCount; i++)
            {
                durSW.Restart();
                if (frame == null)
                {
                    return imageArray;
                }

                capture.Read(frame);
                imageArray[i] = frame.Clone();

                realFPS = getFPS.calculateFPS();
                avgFrameDuration = durSW.ElapsedMilliseconds;
                Console.WriteLine($"avg time per frame {getFPS.calculateAvgDur(avgFrameDuration)} ms. fps {realFPS}. frameNo = {frameNumber++}");
                if (durSW.ElapsedMilliseconds < 16) Thread.Sleep(16 - (int)Math.Floor((double)durSW.ElapsedMilliseconds));

                FilterFrames(frame.Clone(), hueMasks, i);
            } 
            return imageArray;
        }

        private void FilterFrames(Mat frame, Image<Gray, byte>[] hueMasks, int i)
        {
            Image<Hsv, byte> hsv = frame.ToImage<Bgr, byte>().Convert<Hsv, byte>();
            // Obtain the 3 channels (hue, saturation and value) that compose the HSV image
            Image<Gray, byte>[] hsvChannels = hsv.Split();

            try
            {
                foreach (Image<Gray, byte> channel in hsvChannels) SmoothFrames(channel, channel);
                Image<Gray, byte> hueMask = hsvChannels[0];
                Image<Gray, byte> saturationMask = hsvChannels[1];
                Image<Gray, byte> valueMask = hsvChannels[2];
                Image<Gray, byte> combinedMask = new Image<Gray, byte>(hsv.Size);

                CvInvoke.InRange(hueMask, new ScalarArray(new Gray(40).MCvScalar), new ScalarArray(new Gray(80).MCvScalar), hueMask);
                CvInvoke.InRange(saturationMask, new ScalarArray(new Gray(50).MCvScalar), new ScalarArray(new Gray(255).MCvScalar), saturationMask);
                CvInvoke.InRange(valueMask, new ScalarArray(new Gray(50).MCvScalar), new ScalarArray(new Gray(255).MCvScalar), valueMask);

                // Combine hue, saturation, and value masks into a single mask
                CvInvoke.BitwiseAnd(hueMask, saturationMask, combinedMask);
                CvInvoke.BitwiseAnd(combinedMask, valueMask, combinedMask);

                hueMasks[i] = combinedMask;
            }
            finally
            {
                hsvChannels[0].Dispose();
                hsvChannels[1].Dispose();
                hsvChannels[2].Dispose();
            }
        }

        private void SmoothFrames(Image<Gray, byte> inputMask, Image<Gray, byte> outputMask)
        {
            // reduces image noise, so smaller movements not detected, size is tolerance of what gets muted
            CvInvoke.GaussianBlur(inputMask, outputMask, new Size(3, 3), 1);
            // MorphOp.Close fills in holes (dilate-->erode/shrink), Mat.Ones = array of "1" values where location/type is satisfied,
            // also reflects upside down?
            CvInvoke.MorphologyEx(inputMask, outputMask, MorphOp.Close, Mat.Ones(7, 3, DepthType.Cv8U, 1),
                new Point(-1, -1), 1, BorderType.Reflect, new MCvScalar(0));
        }

        private void cancel_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        internal void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}