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
                    // grab file
                    capture = new VideoCapture(dialog.FileName);
                    if (capture != null )
                    {
                        // initializes empty matrix frame
                        Mat frame = new Mat();
                        //// reads grey frame from capture to matrix
                        //capture.Read(frame);
                        //// turns frame to array of pixel data (colour, etc.)
                        //pictureBox1.Image = frame.ToBitmap();

                        Stopwatch sw = new Stopwatch();
                        //int frameNumber = 0;
                        //for (; ; )
                        //{
                        //    sw.Restart();
                        //    capture.Read(frame);

                        //    BoundingBoxes(frame);
                        //    pictureBox1.Image = frame.ToBitmap();

                        //    double dur = sw.ElapsedMilliseconds;
                        //    Console.WriteLine($"avg time per frame {getFPS.calculateAvgDur(dur)} ms. fps {getFPS.calculateFPS()}. frameNo = {frameNumber++}");


                        //    if (CvInvoke.WaitKey(1) == 27)
                        //        Environment.Exit(0);
                        //    Console.WriteLine(dur);
                        //    if (dur < 14)
                        //    {
                        //        Thread.Sleep(16 - (int)Math.Floor(dur));
                        //    }
                        //}

                        Mat[] imageArray = GetVideoFrames();

                        foreach (Mat mat in imageArray)
                        {
                            using (mat)
                            {
                                sw.Restart();
                                //TrackBar.BoundingBoxes(mat);
                                FindBar(mat.ToImage<Bgr, Byte>());

                                double dur = sw.ElapsedMilliseconds;
                                //pictureBox1.Image = mat.ToBitmap();
                                pictureBox1.Refresh();

                                Thread.Sleep(15);
                                //if (dur.ElapsedMilliseconds < 16) Thread.Sleep(16 - (int)Math.Floor((double)dur.ElapsedMilliseconds));
                            }
                        }
                    }
                }
            }

            catch (Exception ex) { 
                MessageBox.Show(ex.Message);
            }
        }

        internal void FindBar(Image<Bgr, byte> image)
        {
            // Convert the image to HSV
            // "using" keyword disposes object after code block is run
            using (Image<Hsv, byte> hsv = image.Convert<Hsv, byte>())
            {
                // Obtain the 3 channels (hue, saturation and value) that compose the HSV image
                Image<Gray, byte>[] channels = hsv.Split();

                try
                {
                    Image<Gray, byte> hueMask = channels[0];
                    // Remove all pixels from the hue channel that are not in the range (currently yellow or red)
                    CvInvoke.InRange(hueMask, new ScalarArray(new Gray(150).MCvScalar), new ScalarArray(new Gray(200).MCvScalar), hueMask);

                    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                    // grabs extreme outer contours by endpoints (compresses horizontal, vertical, and diagonal segments)
                    CvInvoke.FindContours(hueMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                    int minArea = 10000;

                    for(int i=0; i<contours.Size; i++)
                    {
                        // returns rectangle for set of points
                        var bbox = CvInvoke.BoundingRectangle(contours[i]);
                        var area = bbox.Width * bbox.Height;
                        var ar = (float)bbox.Width / bbox.Height;

                        // modify condition
                        if (area > minArea && ar < 1.0)
                        {
                            // Generates rectangle to the frame
                            CvInvoke.Rectangle(hsv, bbox, new MCvScalar(0, 0, 255), 2);
                        }
                    }
                    //Mat frame = new Mat();
                    //CvInvoke.CvtColor(hueMask, frame, ColorConversion.Gray2Bgr);
                    pictureBox1.Image = hsv.ToBitmap();
                }
                finally
                {
                    channels[1].Dispose();
                    channels[2].Dispose();
                }
            }
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

        private void Application_Idle(object sender, EventArgs e)
        {
            try
            {
                // converts capture to matrix
                Mat frame = capture.QueryFrame();
                if (frame == null)
                {
                    Application.Idle -= Application_Idle;
                    return;
                }
                pictureBox1.Image = frame.ToBitmap();
                double FPS = getFPS.calculateFPS();
                Console.WriteLine(FPS);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

//capture.ImageGrabbed += Application_Idle;
//capture.Start();

//myTimer.Interval = 1000 / 30;
//myTimer.Tick -= new EventHandler(Application_Idle);
//myTimer.Tick += new EventHandler(Application_Idle);
//myTimer.Start();

//int frameNumber = 0;
//Stopwatch durSW = new Stopwatch();