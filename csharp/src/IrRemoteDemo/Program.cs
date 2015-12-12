using System;
using System.Threading;
using ev3dev;

namespace IrRemote
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InfraredSensor s = new InfraredSensor(Inputs.INPUT_4);

            var driveService1 = new DriveService(Outputs.OUTPUT_A, Outputs.OUTPUT_D);
            var driveService2 = new DriveService(Outputs.OUTPUT_B, Outputs.OUTPUT_C);

            s.SetIrRemote();

            while (true)
            {
                Console.WriteLine("Sensor value: " + s.GetInt());

                Thread.Sleep(100);
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                        break;
                }

                int value0 = s.GetInt(0);

                DriveState driveState1 = new DriveState(value0);
                driveService1.Drive(driveState1);

                int value1 = s.GetInt(1);

                DriveState driveState2 = new DriveState(value1);
                driveService2.Drive(driveState2);

            }
        }
    }
}
