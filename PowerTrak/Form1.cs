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

                    Mat[] imageArray = GetVideoFrames();
                    foreach (Mat mat in imageArray)
                    {
                        using (mat)
                        {
                            sw.Restart();
                            Console.WriteLine(prevY);
                            prevY = FindBar(mat.ToImage<Bgr, Byte>(), barTracker, prevY);

                            double dur = sw.ElapsedMilliseconds;
                            pictureBox1.Refresh();

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

        internal int FindBar(Image<Bgr, byte> image, BarTracker barTracker, int prevY)
        {
            // Convert the image to HSV
            // "using" keyword disposes object after code block is run
            using (Image<Hsv, byte> hsv = image.Convert<Hsv, byte>())
            {
                // Obtain the 3 channels (hue, saturation and value) that compose the HSV image
                Image<Gray, byte>[] channels = hsv.Split();

                try
                {
                    // MOVE TO PREPROCESSING
                    // ------------------------------------------------------------------------------------------------------------------------
                    Image<Gray, byte> hueMask = channels[0];
                    // reduces image noise, so smaller movements not detected, size is tolerance of what gets muted
                    CvInvoke.GaussianBlur(hueMask, hueMask, new Size(3, 3), 1);
                    // MorphOp.Close fills in holes (dilate-->erode/shrink), Mat.Ones = array of "1" values where location/type is satisfied,
                    // also reflects upside down?
                    CvInvoke.MorphologyEx(hueMask, hueMask, MorphOp.Close, Mat.Ones(7, 3, DepthType.Cv8U, 1),
                        new Point(-1, -1), 1, BorderType.Reflect, new MCvScalar(0));

                    // Remove all pixels from the hue channel that are not in the range (currently green)
                    CvInvoke.InRange(hueMask, new ScalarArray(new Gray(40).MCvScalar), new ScalarArray(new Gray(60).MCvScalar), hueMask);
                    // ------------------------------------------------------------------------------------------------------------------------

                    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                    // grabs extreme outer contours by endpoints (compresses horizontal, vertical, and diagonal segments)
                    CvInvoke.FindContours(hueMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                    int minArea = 100000;

                    for(int i=0; i<contours.Size; i++)
                    {
                        // returns rectangle for set of points
                        var bbox = CvInvoke.BoundingRectangle(contours[i]);
                        var area = bbox.Width * bbox.Height;
                        var ar = (float)bbox.Width / bbox.Height;

                        // bounding wide rectangle instead?
                        if (area > minArea && ar < 1.0)
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
                    pictureBox1.Image = hsv.ToBitmap();
                }
                finally
                {
                    channels[1].Dispose();
                    channels[2].Dispose();
                }
            }
            return prevY;
        }

        private Mat[] GetVideoFrames()
        {
            int frames = (int)capture.Get(CapProp.FrameCount);
            Mat[] imageArray = new Mat[frames];
            Mat frame = new Mat();
            int frameNumber = 0;
            Stopwatch durSW = new Stopwatch();

            for (int i = 0; i < frames; i++)
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
            } 
            return imageArray;
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