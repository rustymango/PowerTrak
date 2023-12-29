using System;
using System.Diagnostics;

namespace PowerTrak
{
    internal class getFPS
    {
        internal static Stopwatch sw = new Stopwatch();
        static double avgDur = 0;
        static int fpsStart = 0;
        static double avgFPS = 0;
        static double fps1Sec = 0;

        internal static double calculateFPS()
        {
            if (sw.ElapsedMilliseconds == 0)
            {
                sw.Start();
            }
            if (sw.ElapsedMilliseconds - fpsStart > 1000)
            {
                fpsStart = (int)sw.ElapsedMilliseconds;
                avgFPS = Math.Ceiling(0.3 * avgFPS) + Math.Ceiling(0.7 * fps1Sec);
                fps1Sec = 0;
            }

            fps1Sec += 1;
            return avgFPS;
        }

        internal static double calculateAvgDur(double newDur)
        {
            avgDur = 0.7 * avgDur + 0.3 * newDur;
            return avgDur;
        }
    }
}