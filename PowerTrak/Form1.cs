using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace PowerTrak
{
    public partial class Form1 : Form
    {
        string plateColour;

        public Form1() {
            InitializeComponent();
            InitializerComboBox();
        }

        private void Form1_Load(object sender, System.EventArgs e) {
            Logo.Image = Image.FromFile(@"C:\Users\vinny\source\repos\PowerTrak\PowerTrak\images\PowerTrak.JPG");
        }

        private async void getVideo_Click(object sender, System.EventArgs e) {
            VideoCapture capture = null;

            try {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Video Files (*.mp4;)|*.mp4";

                if (dialog.ShowDialog()==DialogResult.OK) {
                    capture = new VideoCapture(dialog.FileName);
                    if (capture == null) return;

                    // initializes empty matrix frame
                    Mat frame = new Mat();
                    Stopwatch sw = new Stopwatch();
                    // Stopwatch sw2 = new Stopwatch();
                    BarTracker barTracker = new BarTracker();

                    int topY = 0; int prevY = 0;
                    int frameCount = (int)capture.Get(CapProp.FrameCount);
                    Image<Gray, byte>[] hueMasks = new Image<Gray, byte>[frameCount];

                    Mat[] imageArray = MaskVideoFrames(hueMasks, frameCount, capture, plateColour);
                    for (int i = 0; i < imageArray.Length; i++)
                    {
                        Image<Bgr, byte> image = imageArray[i].ToImage<Bgr, byte>();
                        using (image)
                        {
                            prevY = FindBar(hueMasks[i], image, barTracker, prevY, ref topY);

                            pictureBox1.Image = image.ToBitmap();
                            pictureBox1.Refresh();

                            if (sw.ElapsedMilliseconds < 22) await Task.Delay(22 - (int)Math.Floor((double)sw.ElapsedMilliseconds));
                            sw.Restart();
                            //Console.WriteLine($"frame duration: {sw2.ElapsedMilliseconds}");
                            //sw2.Restart();
                        }
                    }

                    Analytics analytics = barTracker.analytics;
                    List<double> tempoTimes = analytics.tempoTimes;
                    List<double> pauseTimes = analytics.pauseTimes;
                    double pauseY = analytics.pauseY;

                    string pauseResults = ""; string tempoResults = ""; 
                    for (int i=0; i < tempoTimes.Count; i++) {
                        tempoResults += $"\nRep {i+1}: {tempoTimes[i]}ms\n";
                        pauseResults += $"\nRep {i+1}: {pauseTimes[i]}ms\n";
                    }

                    TempoTimer.Text = $"Tempo Times: {tempoResults}";
                    PauseTimer.Text = $"Pause Times: {pauseResults}";
                }
            }

            catch (Exception ex) { 
                MessageBox.Show(ex.Message);
            }
        }

        internal int FindBar(Image<Gray, byte> hueMask, Image<Bgr, byte> hsv, BarTracker barTracker, int prevY, ref int topY) {
            // Convert the image to HSV
            // "using" keyword disposes object after code block is run
            using (hueMask) {
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                
                // grabs extreme outer contours by endpoints (compresses horizontal, vertical, and diagonal segments)
                CvInvoke.FindContours(hueMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                int minArea = 10000;
                int maxArea = 500000;

                for (int i = 0; i < contours.Size; i++) {
                    // returns rectangle for set of points
                    var bbox = CvInvoke.BoundingRectangle(contours[i]);
                    var area = bbox.Width * bbox.Height;
                    var ar = (float)bbox.Width / bbox.Height;

                    if (area > minArea && area < maxArea && Math.Abs(ar-1) < 0.2) {
                        // Generates rectangle to the frame
                        CvInvoke.Rectangle(hsv, bbox, new MCvScalar(0, 0, 255), 2);

                        if (topY == 0) topY = bbox.Y;

                        if (bbox.Y - topY <= 5) prevY = bbox.Y + 3;
                        Console.WriteLine($"Top Y: {topY}, Current Y: {bbox.Y}, Prev Y: {prevY}, Pause Y: {barTracker.analytics.pauseY}");
                        
                        if (barTracker.UnrackStatus() && bbox.Y >= prevY) prevY = barTracker.TrackDownwards(prevY, bbox, topY, 5);

                        int pauseY = barTracker.analytics.pauseY;
                        if (pauseY != 0 && pauseY - bbox.Y > 10) barTracker.TrackUpwards(ref prevY);
                    }
                }
                //pictureBox1.Image = hueMask.ToBitmap();
            }
            return prevY;
        }

        private Mat[] MaskVideoFrames(Image<Gray, byte>[] hueMasks, int frameCount, VideoCapture capture, String plateColour) {
            // switch to Image since preprocessing returns hsv frames
            Mat[] imageArray = new Mat[frameCount];
            Mat frame = new Mat();
            Stopwatch durSW = new Stopwatch();
            durSW.Restart();

            // incorporate error handling FIX
            if (plateColour == "null") plateColour = "red";
            for (int i = 0; i < frameCount; i++) {
                //durSW.Restart();
                if (frame == null) {
                    return imageArray;
                }

                capture.Read(frame);
                imageArray[i] = frame.Clone();

                //realFPS = FPSTracker.calculateFPS();
                //avgFrameDuration = durSW.ElapsedMilliseconds;
                //Console.WriteLine($"avg time per frame {getFPS.calculateAvgDur(avgFrameDuration)} ms. fps {realFPS}. frameNo = {frameNumber++}");
                //if (durSW.ElapsedMilliseconds < 16) Thread.Sleep(16 - (int)Math.Floor((double)durSW.ElapsedMilliseconds));

                Preprocessing.FilterFrames(frame.Clone(), hueMasks, i, plateColour);
            }
            durSW.Stop();
            Console.WriteLine($"Preprocessing Time: {durSW.ElapsedMilliseconds}");
            return imageArray;
        }

        private void cancel_Click(object sender, System.EventArgs e) {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            ComboBox comboBox = (ComboBox)sender;
            plateColour = comboBox.SelectedItem.ToString();
        }

        private void InitializerComboBox() {
            ComboBox comboBox = new ComboBox();

            comboBox1.Items.Add("red");
            comboBox1.Items.Add("blue");
            comboBox1.Items.Add("yellow");
            comboBox1.Items.Add("green");
            comboBox1.Items.Add("black");

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        internal void pictureBox1_Click(object sender, EventArgs e) { }

        private void TempoTimer_Click(object sender, EventArgs e) { }

        private void PauseTimer_Click(object sender, EventArgs e) { }

        private void Logo_Click(object sender, EventArgs e) { }
    }
}