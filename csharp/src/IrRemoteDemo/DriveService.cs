using System;
using ev3dev;

namespace IrRemote
{
    internal class DriveService
    {
        private readonly Motor rightMotor;
        private readonly Motor leftMotor;
        public DriveState DriveState { get; private set; }

        public DriveService(string rightMotorPort, string leftMotorPort)
        {
            this.DriveState = new DriveState(0);
            this.rightMotor = new LargeMotor(rightMotorPort);
            this.leftMotor = new LargeMotor(leftMotorPort);

            if (!rightMotor.Connected)
                this.rightMotor = new MediumMotor(rightMotorPort);

            if (!leftMotor.Connected)
                this.leftMotor = new MediumMotor(leftMotorPort);

            if (rightMotor.Connected)
                rightMotor.StopCommand = Motor.StopCommandCoast;
            if (leftMotor.Connected)
                leftMotor.StopCommand = Motor.StopCommandCoast;
        }

        internal void Drive(DriveState driveState)
        {
            if (driveState.LeftMotor != DriveState.LeftMotor)
            {
                Apply(leftMotor, driveState.LeftMotor);
            }

            if (driveState.RightMotor != DriveState.RightMotor)
            {
                Apply(rightMotor, driveState.RightMotor);
            }

            DriveState = driveState;
        }

        private static void Apply(Motor motor, MotorDrive drive)
        {
            if (!motor.Connected)
                return;
            if (drive == MotorDrive.Stop)
            {
                motor.Stop();
                return;
            }

            if (drive == MotorDrive.Forward)
            {
                motor.DutyCycleSp = 100;
                motor.RunForever();
                return;
            }

            if (drive == MotorDrive.Backward)
            {
                motor.DutyCycleSp = -100;
                motor.RunForever();
                return;
            }
        }
    }
}