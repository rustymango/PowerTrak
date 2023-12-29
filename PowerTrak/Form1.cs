using System;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

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
                //pictureBox1.Image = hueMask.ToBitmap();
                pictureBox1.Image = hsv.ToBitmap();
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

                Preprocessing.FilterFrames(frame.Clone(), hueMasks, i, "green");
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