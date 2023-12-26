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

            if (bbox.Y - prevY == 0) 
            {
                pauseY = bbox.Y;
                if (pauseSW.IsRunning) pauseTime += pauseSW.ElapsedMilliseconds;
                pauseSW.Restart();
            }
            pauseTime = 0;

            return bbox.Y;
        }

        // incorporate velocity tracking later
        internal void TrackUpwards(int pauseY, Rectangle bbox)
        {
            if (tempoSW.IsRunning && pauseSW.IsRunning)
            {
                tempoTime = tempoSW.ElapsedMilliseconds; tempoSW.Stop();
                pauseTime = pauseSW.ElapsedMilliseconds; pauseSW.Stop();

                tempoTime = tempoTime - pauseTime;
                Console.WriteLine("TempoSW stopped.");
            }
            return;
        }
    }
}
