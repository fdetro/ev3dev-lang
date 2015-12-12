using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ev3dev;

namespace SensorDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        public void Main(string[] args)
        {
            Device.SYS_ROOT = "c:/temp/ev3-sensors";
            Console.WriteLine("Reading sensor data");
            Sensor s = new Sensor(string.Empty);

            if (s.Modes.Any())
                Console.WriteLine("Avaliable sensor modes: " + s.Modes.Aggregate((x, y) => x + " " + y));

            while (true)
            {
                Console.WriteLine("Sensor value: " + s.GetInt());

                Thread.Sleep(2000);
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                        break;
                }
            }
        }
    }
}
