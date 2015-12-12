namespace IrRemote
{
    internal class DriveState
    {
        public DriveState(int remoteState)
        {
            switch (remoteState)
            {
                case 0:// none
                    RightMotor = MotorDrive.Stop;
                    LeftMotor = MotorDrive.Stop;
                    break;
                case 1:// red up
                    RightMotor = MotorDrive.Stop;
                    LeftMotor = MotorDrive.Forward;
                    break;
                case 2:// red down
                    RightMotor = MotorDrive.Stop;
                    LeftMotor = MotorDrive.Backward;
                    break;
                case 3:// blue up
                    RightMotor = MotorDrive.Forward;
                    LeftMotor = MotorDrive.Stop;
                    break;
                case 4:// blue down
                    RightMotor = MotorDrive.Backward;
                    LeftMotor = MotorDrive.Stop;
                    break;
                case 5:// red up and blue up
                    RightMotor = MotorDrive.Forward;
                    LeftMotor = MotorDrive.Forward;
                    break;
                case 6:// red up and blue down
                    RightMotor = MotorDrive.Backward;
                    LeftMotor = MotorDrive.Forward;
                    break;
                case 7:// red down and blue up
                    RightMotor = MotorDrive.Forward;
                    LeftMotor = MotorDrive.Backward;
                    break;
                case 8:// red down and blue down
                    RightMotor = MotorDrive.Stop;
                    LeftMotor = MotorDrive.Stop;
                    break;
                case 9:// beacon
                    RightMotor = MotorDrive.Stop;
                    LeftMotor = MotorDrive.Stop;
                    break;
                case 10:// red up and red down
                    RightMotor = MotorDrive.Stop;
                    LeftMotor = MotorDrive.Stop;
                    break;
                case 11:// blue up and blue down
                    RightMotor = MotorDrive.Stop;
                    LeftMotor = MotorDrive.Stop;
                    break;
                    break;
            }
        }

        public MotorDrive LeftMotor { get; private set; }
        public MotorDrive RightMotor { get; private set; }
    }

    enum MotorDrive
    {
        Stop,
        Forward,
        Backward
    }
}
