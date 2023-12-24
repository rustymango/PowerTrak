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
    internal class TrackBar
    {
        internal static void BoundingBoxes(Mat frame)
        {
            // specs of background subtractor
            IBackgroundSubtractor backgroundSubtractor = new BackgroundSubtractorMOG2();
            Mat smoothFrame = new Mat();
            // reduces image noise, so smaller movements not detected, size is tolerance of what gets muted
            CvInvoke.GaussianBlur(frame, smoothFrame, new Size(3, 3), 1);

            // removes non-moving background from image and transfers result to "foreground mask"
            Mat foregroundMask = new Mat();
            backgroundSubtractor.Apply(smoothFrame, foregroundMask);

            // filters out pixels of certain specs from foreground mask
            CvInvoke.Threshold(foregroundMask, foregroundMask, 200, 240, ThresholdType.Binary);

            // MorphOp.Close fills in holes (dilate-->erode/shrink), Mat.Ones = array of "1" values where location/type is satisfied,
            // also reflects upside down?
            CvInvoke.MorphologyEx(foregroundMask, foregroundMask, MorphOp.Close, Mat.Ones(7, 3, DepthType.Cv8U, 1),
                new Point(-1, -1), 1, BorderType.Reflect, new MCvScalar(0));

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            // grabs extreme outer contours by endpoints (compresses horizontal, vertical, and diagonal segments)
            CvInvoke.FindContours(foregroundMask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
            int minArea = 10000;
            for (int i = 0; i < contours.Size; i++)
            {
                // returns rectangle for set of points
                var bbox = CvInvoke.BoundingRectangle(contours[i]);
                var area = bbox.Width * bbox.Height;
                var ar = (float)bbox.Width / bbox.Height;

                // modify condition
                if (area > minArea && ar < 1.0)
                {
                    // Generates rectangle to the frame
                    CvInvoke.Rectangle(frame, bbox, new MCvScalar(0, 0, 255), 2);
                }
            }
        }
    }
}
