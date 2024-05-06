using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;

namespace PowerTrak {
    struct Analytics {
        public int pauseY;
        public double pauseTime;
        public List<double> pauseTimes;
        public double tempoTime;
        public List<double> tempoTimes;

        internal Analytics(int pauseY, double pauseTime, List<double> pauseTimes, double tempoTime, List<double> tempoTimes)
        {
            this.pauseY = pauseY;
            this.pauseTime = pauseTime;
            this.pauseTimes = pauseTimes;
            this.tempoTime = tempoTime;
            this.tempoTimes = tempoTimes;
        }
    }

    internal class BarTracker {
        internal Analytics analytics;
        Stopwatch tempoSW = new Stopwatch();
        Stopwatch pauseSW = new Stopwatch();
        bool goingUp = false;

        internal BarTracker() {
            analytics = new Analytics(0, 0, new List<double>(), 0, new List<double>());
        }

        // WIP
        internal bool UnrackStatus()
        {
            return true;
        }

        internal int TrackDownwards(int prevY, Rectangle bbox, int topY, int tolerance)
        {
            if (!tempoSW.IsRunning && bbox.Y - topY > 10) tempoSW.Restart();

            if (bbox.Y - prevY == 0)
            {
                goingUp = false;
                analytics.pauseY = bbox.Y;
                if (pauseSW.IsRunning) analytics.pauseTime += pauseSW.ElapsedMilliseconds;
                // Console.WriteLine($"Pause Time: {analytics.pauseTime}");
                
                pauseSW.Restart();
            }
            else { analytics.pauseTime = 0; /*Console.WriteLine("Pause timer reset");*/ }

            return bbox.Y;
        }

        // incorporate velocity tracking later
        internal void TrackUpwards(ref int prevY)
        {
            if (tempoSW.IsRunning && pauseSW.IsRunning)
            {
                analytics.tempoTime = tempoSW.ElapsedMilliseconds; tempoSW.Stop();
                analytics.pauseTime = pauseSW.ElapsedMilliseconds; pauseSW.Stop();

                analytics.tempoTime = analytics.tempoTime - analytics.pauseTime;
                Console.WriteLine("TempoSW stopped.");
            }

            if (!goingUp) 
            {
                Console.WriteLine($"Final Tempo Time: {analytics.tempoTime}");
                Console.WriteLine($"Final Pause Time: {analytics.pauseTime}");
                analytics.pauseTimes.Add(analytics.pauseTime);
                analytics.tempoTimes.Add(analytics.tempoTime);
                analytics.tempoTime = 0; analytics.pauseTime = 0;

                goingUp = true;
            }
        }
    }
}
