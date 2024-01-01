using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;

namespace PowerTrak
{
    internal class BarTracker
    {
        public int pauseY = 0;

        double tempoTime;
        public List<double> tempoTimes = new List<double>();
        Stopwatch tempoSW = new Stopwatch();

        double pauseTime;
        public List<double> pauseTimes = new List<double>();
        Stopwatch pauseSW = new Stopwatch();

        int currRep = 1;
        int nextRep = 1;

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
                currRep = nextRep;
                pauseY = bbox.Y;
                if (pauseSW.IsRunning) pauseTime += pauseSW.ElapsedMilliseconds;
                Console.WriteLine($"Pause Time: {pauseTime}");
                pauseSW.Restart();
            }
            else { pauseTime = 0; Console.WriteLine("Pause timer reset"); }

            return bbox.Y;
        }

        // incorporate velocity tracking later
        internal int TrackUpwards(Rectangle bbox)
        {
            if (tempoSW.IsRunning && pauseSW.IsRunning)
            {
                tempoTime = tempoSW.ElapsedMilliseconds; tempoSW.Stop();
                pauseTime = pauseSW.ElapsedMilliseconds; pauseSW.Stop();

                tempoTime = tempoTime - pauseTime;
                Console.WriteLine("TempoSW stopped.");
            }

            if (currRep == nextRep) 
            {
                Console.WriteLine($"Final Pause Time: {pauseTime}");
                pauseTimes.Add(pauseTime);
                tempoTimes.Add(tempoTime);
                tempoTime = 0; pauseTime = 0;

                nextRep += 1;
            }
            return bbox.Y;
        }
    }
}
