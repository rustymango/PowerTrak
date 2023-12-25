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
    internal class BarTracker
    {
        public double tempoTime;
        private Stopwatch tempoSW = new Stopwatch();
        public int pauseY=0;
        public double pauseTime;
        private Stopwatch pauseSW = new Stopwatch();

        internal bool UnrackStatus()
        {
            return true;
        }

        internal int TrackDownwards(int prevY, Rectangle bbox, int tolerance)
        {
            if(!tempoSW.IsRunning) tempoSW.Restart();

            if (bbox.Y - prevY < tolerance) 
            {
                pauseY = bbox.Y;
                tempoTime = tempoSW.ElapsedMilliseconds;

                tempoSW.Stop();
                if (!pauseSW.IsRunning) pauseSW.Restart();
                Console.WriteLine("TempoSW stopped.");
            }

            return bbox.Y;
        }

        // incorporate velocity tracking later
        internal int TrackUpwards(int pauseY, Rectangle bbox)
        {
            pauseTime = pauseSW.ElapsedMilliseconds;
            pauseSW.Stop();
            return 0;
        }
    }
}
