using System;
using System.Windows.Forms;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace PowerTrak
{
    public partial class Form1 : Form
    {
        VideoCapture capture = null;
        IBackgroundSubtractor backgroundSubtractor;

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
                    if (capture != null )
                    {
                        Mat frame = new Mat();
                        capture.Read(frame);
                        pictureBox1.Image = frame.ToBitmap();

                        backgroundSubtractor = new BackgroundSubtractorMOG2();
                        Application.Idle += Application_Idle;
                    }
                }
            }

            catch (Exception ex) { 
                MessageBox.Show(ex.Message);
            }
        }

        private void Application_Idle(object sender, EventArgs e) {
            try {
                Mat frame = capture.QueryFrame();
                if (frame.IsEmpty)
                {
                    Application.Idle -= Application_Idle;
                    return;
                }

                Mat smoothFrame = new Mat();
                CvInvoke.GaussianBlur(frame, smoothFrame, new Size(3, 3), 1);

                Mat foregroundMask = new Mat();
                backgroundSubtractor.Apply(smoothFrame, foregroundMask);

                CvInvoke.Threshold(foregroundMask, foregroundMask, 200, 240, ThresholdType.Binary);
                CvInvoke.MorphologyEx(foregroundMask, foregroundMask, Emgu.CV.CvEnum.MorphOp.Close,
                    Mat.Ones(7,3,Emgu.CV.CvEnum.DepthType.Cv8U, 1), new Point(-1,-1), 1, BorderType.Reflect, new Emgu.CV.Structure.MCvScalar(0));

                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(foregroundMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                int minArea = 10000;
                for (int i=0; i<contours.Size; i++)
                {
                    var bbox = CvInvoke.BoundingRectangle(contours[i]);
                    var area = bbox.Width * bbox.Height;
                    var ar = (float)bbox.Width / bbox.Height;

                    if (area > minArea && ar<1.0) {
                        CvInvoke.Rectangle(frame, bbox, new MCvScalar(0, 0, 255), 2);
                    }
                }

                pictureBox1.Image = frame.ToBitmap();

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
