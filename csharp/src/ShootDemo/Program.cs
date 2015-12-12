using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ev3dev;

namespace ShootDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Device.SYS_ROOT = "c:/temp/ev3-sensors";
            Motor m = new LargeMotor(Outputs.OUTPUT_D);

            var sw = Stopwatch.StartNew();
            Console.WriteLine("started");
            m.Reset();
            Console.WriteLine("RunTimed " + sw.ElapsedMilliseconds);

            m.DutyCycleSp = 100;
            m.TimeSp = 1000;
            m.RunTimed();
            Console.WriteLine("RunTimed 1 done " + sw.ElapsedMilliseconds);
            Thread.Sleep(1000);
            Console.WriteLine("Timer 1 1000 done " + sw.ElapsedMilliseconds);

            m.DutyCycleSp = -100;
            m.TimeSp = 1000;
            m.RunTimed();
            Console.WriteLine("RunTimed 2 done " + sw.ElapsedMilliseconds);
            Thread.Sleep(1000);
            Console.WriteLine("Timer 2 1000 done " + sw.ElapsedMilliseconds);

            m.StopCommand = "coast";
            m.Stop();
        }
    }
}
