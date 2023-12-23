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
        IBackgroundSubtractor backgroundSubtractor;
        int frameCount = 0;
        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

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

                        // specs of background subtractor
                        //backgroundSubtractor = new BackgroundSubtractorMOG2();

                        //capture.ImageGrabbed += Application_Idle;
                        //capture.Start();

                        //myTimer.Interval = 1000 / 30;
                        //myTimer.Tick -= new EventHandler(Application_Idle);
                        //myTimer.Tick += new EventHandler(Application_Idle);
                        //myTimer.Start();

                        //int frameNumber = 0;
                        //Stopwatch durSW = new Stopwatch();

                        //for (; ; )
                        //{
                        //    durSW.Restart();
                        //    capture.Read(frame);
                        //    pictureBox1.Image = frame.ToBitmap();

                        //    double dur = durSW.ElapsedMilliseconds;
                        //    Console.WriteLine($"avg time per frame {getFPS.calculateAvgDur(dur)} ms. fps {getFPS.calculateFPS()}. frameNo = {frameNumber++}");


                        //    if (CvInvoke.WaitKey(1) == 27)
                        //        Environment.Exit(0);
                        //    Console.WriteLine(dur);
                        //    if (dur < 14)
                        //    {
                        //        Thread.Sleep(16 - (int)Math.Floor(dur));
                        //    }
                        //}

                        Bitmap[] imageArray = GetVideoFrames();
                        int i = 0;

                        foreach ( Bitmap image in imageArray )
                        {
                            pictureBox1.Image = imageArray[i];
                            pictureBox1.Refresh();
                            i++;
                            Thread.Sleep(15);
                        }
                    }
                }
            }

            catch (Exception ex) { 
                MessageBox.Show(ex.Message);
            }
        }

        private void Application_Idle(object sender, EventArgs e) {
            try {
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

                //double sleepTime=0;
                //if(FPS<)

                //Thread.Sleep(1000 / (int)Math.Floor(FPS));
                //Thread.Sleep(47);
                //myTimer.Interval = 1000 / (int)Math.Floor(FPS);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            frameCount += 1;
            //Console.WriteLine(frameCount);
        }

        private Bitmap[] GetVideoFrames()
        {
            int frames = (int)capture.Get(CapProp.FrameCount);
            Bitmap[] imageArray = new Bitmap[frames];
            Mat frame = new Mat();

            for (int i = 0; i < frames; i++)
            {
                capture.Read(frame);
                if (frame == null)
                {
                    return imageArray;
                }
                imageArray[i] = frame.ToBitmap();
            } 
            return imageArray;
        }

        private void cancel_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
//Mat smoothFrame = new Mat();
//// reduces image noise, so smaller movements not detected, size is tolerance of what gets muted
//CvInvoke.GaussianBlur(frame, smoothFrame, new Size(3, 3), 1);

//// removes non-moving background from image and transfers result to "foreground mask"
//Mat foregroundMask = new Mat();
//backgroundSubtractor.Apply(smoothFrame, foregroundMask);

//// filters out pixels of certain specs from foreground mask
//CvInvoke.Threshold(foregroundMask, foregroundMask, 200, 240, ThresholdType.Binary);

//// MorphOp.Close fills in holes (dilate-->erode/shrink), Mat.Ones = array of "1" values where location/type is satisfied,
//// also reflects upside down?
//CvInvoke.MorphologyEx(foregroundMask, foregroundMask, MorphOp.Close, Mat.Ones(7,3,DepthType.Cv8U,1), 
//    new Point(-1,-1), 1, BorderType.Reflect, new MCvScalar(0));

//VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
//// grabs extreme outer contours by endpoints (compresses horizontal, vertical, and diagonal segments)
//CvInvoke.FindContours(foregroundMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
//int minArea=100000;
//for (int i=0; i<contours.Size; i++)
//{
//    // returns rectangle for set of points
//    var bbox = CvInvoke.BoundingRectangle(contours[i]);
//    var area = bbox.Width * bbox.Height;
//    var ar = (float)bbox.Width / bbox.Height;

//    // modify condition
//    if (area > minArea && ar<1.0) {
//        // Generates rectangle to the frame
//        CvInvoke.Rectangle(frame, bbox, new MCvScalar(0, 0, 255), 2);
//    }
//}