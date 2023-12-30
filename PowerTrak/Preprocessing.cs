using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace PowerTrak
{
    internal class Preprocessing
    {
        internal static void FilterFrames(Mat frame, Image<Gray, byte>[] hueMasks, int i, string plateColour)
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

                int[] grayValues = GetGrayScales(plateColour);
                CvInvoke.InRange(hueMask, new ScalarArray(new Gray(grayValues[0]).MCvScalar), new ScalarArray(new Gray(grayValues[1]).MCvScalar), hueMask);
                CvInvoke.InRange(saturationMask, new ScalarArray(new Gray(grayValues[2]).MCvScalar), new ScalarArray(new Gray(grayValues[3]).MCvScalar), saturationMask);
                CvInvoke.InRange(valueMask, new ScalarArray(new Gray(grayValues[4]).MCvScalar), new ScalarArray(new Gray(grayValues[5]).MCvScalar), valueMask);

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

        internal static void SmoothFrames(Image<Gray, byte> inputMask, Image<Gray, byte> outputMask)
        {
            // reduces image noise, so smaller movements not detected, size is tolerance of what gets muted
            CvInvoke.GaussianBlur(inputMask, outputMask, new Size(3, 3), 1);
            // MorphOp.Close fills in holes (dilate-->erode/shrink), Mat.Ones = array of "1" values where location/type is satisfied,
            // also reflects upside down?
            CvInvoke.MorphologyEx(inputMask, outputMask, MorphOp.Close, Mat.Ones(7, 3, DepthType.Cv8U, 1),
                new Point(-1, -1), 1, BorderType.Reflect, new MCvScalar(0));
        }

        private static int[] GetGrayScales(string colour)
        {
            int[] grayValues;
            switch(colour) {
                case "blue": grayValues = new int[] {100,140,95,255,95,255}; return grayValues;
                case "yellow": grayValues = new int[] {15, 30, 70, 255, 70, 255}; return grayValues;
                case "red": grayValues = new int[] {140, 200, 50, 255, 50, 255}; return grayValues;
                case "green": grayValues = new int[] {40, 80, 50, 255, 50, 255}; return grayValues;
                case "black": grayValues = new int[] {100,120,0,20,0,180}; return grayValues;
            }
            return new int[6];
        }
    }
}
