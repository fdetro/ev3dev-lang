/*
 * C# API to the sensors, motors, buttons, LEDs and battery of the ev3dev
 * Linux kernel for the LEGO Mindstorms EV3 hardware
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *
 */

//~autogen autogen-header

// Sections of the following code were auto-generated based on spec v0.9.3-pre, rev 2.

//~autogen
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ev3dev
{
    public class Device
    {
        public static string SYS_ROOT = "/sys/";
        protected string _path;
        protected int _deviceIndex = -1;

        protected bool Connect(string classDir,
               string pattern,
               IDictionary<string, string[]> match)
        {

            int pattern_length = pattern.Length;

            if (!Directory.Exists(classDir))
            {
                return false;
            }

            var dirs = Directory.EnumerateDirectories(classDir);
            foreach (var currentFullDirPath in dirs)
            {
                var dirName = Path.GetFileName(currentFullDirPath);
                if (dirName.StartsWith(pattern))
                {
                    _path = Path.Combine(classDir, dirName);

                    bool bMatch = true;
                    foreach (var m in match)
                    {
                        var attribute = m.Key;
                        var matches = m.Value;
                        var strValue = GetAttrString(attribute);

                        if (matches.Any() && !string.IsNullOrEmpty(matches.First()) 
                            && !matches.Any(x=>x == strValue))
                        {
                            bMatch = false;
                            break;
                        }
                    }

                    if (bMatch)
                    {
                        return true;
                    }

                    _path = null;
                }
            }
            return false;

        }

        public bool Connected
        {
            get { return !string.IsNullOrEmpty(_path); }
        }

        public int DeviceIndex
        {
            get
            {
                if (!Connected)
                    throw new NotSupportedException("no device connected");

                if (_deviceIndex < 0)
                {
                    int f = 1;
                    _deviceIndex = 0;
                    foreach (char c in _path.Where(char.IsDigit))
                    {
                        _deviceIndex += (int)char.GetNumericValue(c) * f;
                        f *= 10;
                    }
                }

                return _deviceIndex;
            }
        }

        public int GetAttrInt(string name)
        {
            if (!Connected)
                throw new NotSupportedException("no device connected");

            using (StreamReader os = OpenStreamReader(name))
            {
                return int.Parse(os.ReadToEnd());
            }
        }

        public void SetAttrInt(string name, int value)
        {
            if (!Connected)
                throw new NotSupportedException("no device connected");

            using (StreamWriter os = OpenStreamWriter(name))
            {
                os.Write(value);
            }
        }

        public string GetAttrString(string name)
        {
            if (!Connected)
                throw new NotSupportedException("no device connected");

            using (StreamReader os = OpenStreamReader(name))
            {
                return os.ReadToEnd();
            }
        }

        public void SetAttrString(string name,
                              string value)
        {
            if (!Connected)
                throw new NotSupportedException("no device connected");

            using (StreamWriter os = OpenStreamWriter(name))
            {
                os.Write(value);
            }
        }

        public string GetAttrLine(string name)
        {
            if (!Connected)
                throw new NotSupportedException("no device connected");

            using (StreamReader os = OpenStreamReader(name))
            {
                return os.ReadLine();
            }
        }

        public string[] GetAttrSet(string name)
        {
            string s = GetAttrLine(name);
            return s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] GetAttrSet(string name, out string pCur)
        {
            string[] result = GetAttrSet(name);
            var bracketedValue = result.FirstOrDefault(s => s.StartsWith("["));
            pCur = bracketedValue.Substring(1, bracketedValue.Length - 2);
            return result;
        }
        
        public string GetAttrFromSet(string name)
        {
            string[] result = GetAttrSet(name);
            var bracketedValue = result.FirstOrDefault(s => s.StartsWith("["));
            var pCur = bracketedValue.Substring(1, bracketedValue.Length - 2);
            return pCur;
        }

        private StreamReader OpenStreamReader(string name)
        {
            return new StreamReader(Path.Combine(_path, name));
        }

        private StreamWriter OpenStreamWriter(string name)
        {
            return new StreamWriter(Path.Combine(_path, name));
        }
    }
    
//~autogen csharp-class-description classes>classes

    /// <summary> 
    /// The motor class provides a uniform interface for using motors with 
    /// positional and directional feedback such as the EV3 and NXT motors. 
    /// This feedback allows for precise control of the motors. This is the 
    /// most common type of motor, so we just call it `motor`.
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/drivers/tacho-motor-class/">http://www.ev3dev.org/docs/drivers/tacho-motor-class/</see>
    /// </summary>
    public partial class Motor : Device
    { 
#region propertyValues
        /// <summary> 
        /// Run the motor until another command is sent.
        /// </summary>
        public const string CommandRunForever = "run-forever";
        
        /// <summary> 
        /// Run to an absolute position specified by `position_sp` and then
        /// stop using the command specified in `stop_command`.
        /// </summary>
        public const string CommandRunToAbsPos = "run-to-abs-pos";
        
        /// <summary> 
        /// Run to a position relative to the current `position` value.
        /// The new position will be current `position` + `position_sp`.
        /// When the new position is reached, the motor will stop using
        /// the command specified by `stop_command`.
        /// </summary>
        public const string CommandRunToRelPos = "run-to-rel-pos";
        
        /// <summary> 
        /// Run the motor for the amount of time specified in `time_sp`
        /// and then stop the motor using the command specified by `stop_command`.
        /// </summary>
        public const string CommandRunTimed = "run-timed";
        
        /// <summary> 
        /// Run the motor at the duty cycle specified by `duty_cycle_sp`.
        /// Unlike other run commands, changing `duty_cycle_sp` while running *will*
        /// take effect immediately.
        /// </summary>
        public const string CommandRunDirect = "run-direct";
        
        /// <summary> 
        /// Stop any of the run commands before they are complete using the
        /// command specified by `stop_command`.
        /// </summary>
        public const string CommandStop = "stop";
        
        /// <summary> 
        /// Reset all of the motor parameter attributes to their default value.
        /// This will also have the effect of stopping the motor.
        /// </summary>
        public const string CommandReset = "reset";
        
        /// <summary> 
        /// Sets the normal polarity of the rotary encoder.
        /// </summary>
        public const string EncoderPolarityNormal = "normal";
        
        /// <summary> 
        /// Sets the inversed polarity of the rotary encoder.
        /// </summary>
        public const string EncoderPolarityInversed = "inversed";
        
        /// <summary> 
        /// With `normal` polarity, a positive duty cycle will
        /// cause the motor to rotate clockwise.
        /// </summary>
        public const string PolarityNormal = "normal";
        
        /// <summary> 
        /// With `inversed` polarity, a positive duty cycle will
        /// cause the motor to rotate counter-clockwise.
        /// </summary>
        public const string PolarityInversed = "inversed";
        
        /// <summary> 
        /// The motor controller will vary the power supplied to the motor
        /// to try to maintain the speed specified in `speed_sp`.
        /// </summary>
        public const string SpeedRegulationOn = "on";
        
        /// <summary> 
        /// The motor controller will use the power specified in `duty_cycle_sp`.
        /// </summary>
        public const string SpeedRegulationOff = "off";
        
        /// <summary> 
        /// Power will be removed from the motor and it will freely coast to a stop.
        /// </summary>
        public const string StopCommandCoast = "coast";
        
        /// <summary> 
        /// Power will be removed from the motor and a passive electrical load will
        /// be placed on the motor. This is usually done by shorting the motor terminals
        /// together. This load will absorb the energy from the rotation of the motors and
        /// cause the motor to stop more quickly than coasting.
        /// </summary>
        public const string StopCommandBrake = "brake";
        
        /// <summary> 
        /// Does not remove power from the motor. Instead it actively try to hold the motor
        /// at the current position. If an external force tries to turn the motor, the motor
        /// will ``push back`` to maintain its position.
        /// </summary>
        public const string StopCommandHold = "hold";
        
#endregion 
#region systemProperties
        /// <summary> 
        /// Sends a command to the motor controller. See `commands` for a list of
        /// possible values.
        /// </summary>
        public string Command 
        { 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns a list of commands that are supported by the motor
        /// controller. Possible values are `run-forever`, `run-to-abs-pos`, `run-to-rel-pos`,
        /// `run-timed`, `run-direct`, `stop` and `reset`. Not all commands may be supported.
        /// 
        /// - `run-forever` will cause the motor to run until another command is sent.
        /// - `run-to-abs-pos` will run to an absolute position specified by `position_sp`
        ///   and then stop using the command specified in `stop_command`.
        /// - `run-to-rel-pos` will run to a position relative to the current `position` value.
        ///   The new position will be current `position` + `position_sp`. When the new
        ///   position is reached, the motor will stop using the command specified by `stop_command`.
        /// - `run-timed` will run the motor for the amount of time specified in `time_sp`
        ///   and then stop the motor using the command specified by `stop_command`.
        /// - `run-direct` will run the motor at the duty cycle specified by `duty_cycle_sp`.
        ///   Unlike other run commands, changing `duty_cycle_sp` while running *will*
        ///   take effect immediately.
        /// - `stop` will stop any of the run commands before they are complete using the
        ///   command specified by `stop_command`.
        /// - `reset` will reset all of the motor parameter attributes to their default value.
        ///   This will also have the effect of stopping the motor.
        /// </summary>
        public string[] Commands 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the number of tacho counts in one rotation of the motor. Tacho counts
        /// are used by the position and speed attributes, so you can use this value
        /// to convert rotations or degrees to tacho counts. In the case of linear
        /// actuators, the units here will be counts per centimeter.
        /// </summary>
        public string CountPerRot 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the name of the driver that provides this tacho motor device.
        /// </summary>
        public string DriverName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the current duty cycle of the motor. Units are percent. Values
        /// are -100 to 100.
        /// </summary>
        public string DutyCycle 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Writing sets the duty cycle setpoint. Reading returns the current value.
        /// Units are in percent. Valid values are -100 to 100. A negative value causes
        /// the motor to rotate in reverse. This value is only used when `speed_regulation`
        /// is off.
        /// </summary>
        public string DutyCycleSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Sets the polarity of the rotary encoder. This is an advanced feature to all
        /// use of motors that send inversed encoder signals to the EV3. This should
        /// be set correctly by the driver of a device. It You only need to change this
        /// value if you are using a unsupported device. Valid values are `normal` and
        /// `inversed`.
        /// </summary>
        public string EncoderPolarity 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Sets the polarity of the motor. With `normal` polarity, a positive duty
        /// cycle will cause the motor to rotate clockwise. With `inversed` polarity,
        /// a positive duty cycle will cause the motor to rotate counter-clockwise.
        /// Valid values are `normal` and `inversed`.
        /// </summary>
        public string Polarity 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns the name of the port that the motor is connected to.
        /// </summary>
        public string PortName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the current position of the motor in pulses of the rotary
        /// encoder. When the motor rotates clockwise, the position will increase.
        /// Likewise, rotating counter-clockwise causes the position to decrease.
        /// Writing will set the position to that value.
        /// </summary>
        public string Position 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// The proportional constant for the position PID.
        /// </summary>
        public string PositionP 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// The integral constant for the position PID.
        /// </summary>
        public string PositionI 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// The derivative constant for the position PID.
        /// </summary>
        public string PositionD 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Writing specifies the target position for the `run-to-abs-pos` and `run-to-rel-pos`
        /// commands. Reading returns the current value. Units are in tacho counts. You
        /// can use the value returned by `counts_per_rot` to convert tacho counts to/from
        /// rotations or degrees.
        /// </summary>
        public string PositionSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns the current motor speed in tacho counts per second. Not, this is
        /// not necessarily degrees (although it is for LEGO motors). Use the `count_per_rot`
        /// attribute to convert this value to RPM or deg/sec.
        /// </summary>
        public string Speed 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Writing sets the target speed in tacho counts per second used when `speed_regulation`
        /// is on. Reading returns the current value.  Use the `count_per_rot` attribute
        /// to convert RPM or deg/sec to tacho counts per second.
        /// </summary>
        public string SpeedSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Writing sets the ramp up setpoint. Reading returns the current value. Units
        /// are in milliseconds. When set to a value > 0, the motor will ramp the power
        /// sent to the motor from 0 to 100% duty cycle over the span of this setpoint
        /// when starting the motor. If the maximum duty cycle is limited by `duty_cycle_sp`
        /// or speed regulation, the actual ramp time duration will be less than the setpoint.
        /// </summary>
        public string RampUpSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Writing sets the ramp down setpoint. Reading returns the current value. Units
        /// are in milliseconds. When set to a value > 0, the motor will ramp the power
        /// sent to the motor from 100% duty cycle down to 0 over the span of this setpoint
        /// when stopping the motor. If the starting duty cycle is less than 100%, the
        /// ramp time duration will be less than the full span of the setpoint.
        /// </summary>
        public string RampDownSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Turns speed regulation on or off. If speed regulation is on, the motor
        /// controller will vary the power supplied to the motor to try to maintain the
        /// speed specified in `speed_sp`. If speed regulation is off, the controller
        /// will use the power specified in `duty_cycle_sp`. Valid values are `on` and
        /// `off`.
        /// </summary>
        public string SpeedRegulationEnabled 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// The proportional constant for the speed regulation PID.
        /// </summary>
        public string SpeedRegulationP 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// The integral constant for the speed regulation PID.
        /// </summary>
        public string SpeedRegulationI 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// The derivative constant for the speed regulation PID.
        /// </summary>
        public string SpeedRegulationD 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Reading returns a list of state flags. Possible flags are
        /// `running`, `ramping` `holding` and `stalled`.
        /// </summary>
        public string[] State 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Reading returns the current stop command. Writing sets the stop command.
        /// The value determines the motors behavior when `command` is set to `stop`.
        /// Also, it determines the motors behavior when a run command completes. See
        /// `stop_commands` for a list of possible values.
        /// </summary>
        public string StopCommand 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns a list of stop modes supported by the motor controller.
        /// Possible values are `coast`, `brake` and `hold`. `coast` means that power will
        /// be removed from the motor and it will freely coast to a stop. `brake` means
        /// that power will be removed from the motor and a passive electrical load will
        /// be placed on the motor. This is usually done by shorting the motor terminals
        /// together. This load will absorb the energy from the rotation of the motors and
        /// cause the motor to stop more quickly than coasting. `hold` does not remove
        /// power from the motor. Instead it actively try to hold the motor at the current
        /// position. If an external force tries to turn the motor, the motor will 'push
        /// back' to maintain its position.
        /// </summary>
        public string[] StopCommands 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Writing specifies the amount of time the motor will run when using the
        /// `run-timed` command. Reading returns the current value. Units are in
        /// milliseconds.
        /// </summary>
        public string TimeSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
#endregion 
    } 
    /// <summary> 
    /// EV3 large servo motor
    /// 
    /// For online documentation see <see href=""></see>
    /// </summary>
    public partial class LargeMotor : Motor
    { 
    } 
    /// <summary> 
    /// EV3 medium servo motor
    /// 
    /// For online documentation see <see href=""></see>
    /// </summary>
    public partial class MediumMotor : Motor
    { 
    } 
    /// <summary> 
    /// The DC motor class provides a uniform interface for using regular DC motors 
    /// with no fancy controls or feedback. This includes LEGO MINDSTORMS RCX motors 
    /// and LEGO Power Functions motors.
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/drivers/dc-motor-class/">http://www.ev3dev.org/docs/drivers/dc-motor-class/</see>
    /// </summary>
    public partial class DcMotor : Device
    { 
#region propertyValues
        /// <summary> 
        /// Run the motor until another command is sent.
        /// </summary>
        public const string CommandRunForever = "run-forever";
        
        /// <summary> 
        /// Run the motor for the amount of time specified in `time_sp`
        /// and then stop the motor using the command specified by `stop_command`.
        /// </summary>
        public const string CommandRunTimed = "run-timed";
        
        /// <summary> 
        /// Run the motor at the duty cycle specified by `duty_cycle_sp`.
        /// Unlike other run commands, changing `duty_cycle_sp` while running *will*
        /// take effect immediately.
        /// </summary>
        public const string CommandRunDirect = "run-direct";
        
        /// <summary> 
        /// Stop any of the run commands before they are complete using the
        /// command specified by `stop_command`.
        /// </summary>
        public const string CommandStop = "stop";
        
        /// <summary> 
        /// With `normal` polarity, a positive duty cycle will
        /// cause the motor to rotate clockwise.
        /// </summary>
        public const string PolarityNormal = "normal";
        
        /// <summary> 
        /// With `inversed` polarity, a positive duty cycle will
        /// cause the motor to rotate counter-clockwise.
        /// </summary>
        public const string PolarityInversed = "inversed";
        
        /// <summary> 
        /// Power will be removed from the motor and it will freely coast to a stop.
        /// </summary>
        public const string StopCommandCoast = "coast";
        
        /// <summary> 
        /// Power will be removed from the motor and a passive electrical load will
        /// be placed on the motor. This is usually done by shorting the motor terminals
        /// together. This load will absorb the energy from the rotation of the motors and
        /// cause the motor to stop more quickly than coasting.
        /// </summary>
        public const string StopCommandBrake = "brake";
        
#endregion 
#region systemProperties
        /// <summary> 
        /// Sets the command for the motor. Possible values are `run-forever`, `run-timed` and
        /// `stop`. Not all commands may be supported, so be sure to check the contents
        /// of the `commands` attribute.
        /// </summary>
        public string Command 
        { 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns a list of commands supported by the motor
        /// controller.
        /// </summary>
        public string[] Commands 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the name of the motor driver that loaded this device. See the list
        /// of [supported devices] for a list of drivers.
        /// </summary>
        public string DriverName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Shows the current duty cycle of the PWM signal sent to the motor. Values
        /// are -100 to 100 (-100% to 100%).
        /// </summary>
        public string DutyCycle 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Writing sets the duty cycle setpoint of the PWM signal sent to the motor.
        /// Valid values are -100 to 100 (-100% to 100%). Reading returns the current
        /// setpoint.
        /// </summary>
        public string DutyCycleSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Sets the polarity of the motor. Valid values are `normal` and `inversed`.
        /// </summary>
        public string Polarity 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns the name of the port that the motor is connected to.
        /// </summary>
        public string PortName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Sets the time in milliseconds that it take the motor to ramp down from 100%
        /// to 0%. Valid values are 0 to 10000 (10 seconds). Default is 0.
        /// </summary>
        public string RampDownSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Sets the time in milliseconds that it take the motor to up ramp from 0% to
        /// 100%. Valid values are 0 to 10000 (10 seconds). Default is 0.
        /// </summary>
        public string RampUpSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Gets a list of flags indicating the motor status. Possible
        /// flags are `running` and `ramping`. `running` indicates that the motor is
        /// powered. `ramping` indicates that the motor has not yet reached the
        /// `duty_cycle_sp`.
        /// </summary>
        public string[] State 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Sets the stop command that will be used when the motor stops. Read
        /// `stop_commands` to get the list of valid values.
        /// </summary>
        public string StopCommand 
        { 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Gets a list of stop commands. Valid values are `coast`
        /// and `brake`.
        /// </summary>
        public string[] StopCommands 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Writing specifies the amount of time the motor will run when using the
        /// `run-timed` command. Reading returns the current value. Units are in
        /// milliseconds.
        /// </summary>
        public string TimeSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
#endregion 
    } 
    /// <summary> 
    /// The servo motor class provides a uniform interface for using hobby type 
    /// servo motors.
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/drivers/servo-motor-class/">http://www.ev3dev.org/docs/drivers/servo-motor-class/</see>
    /// </summary>
    public partial class ServoMotor : Device
    { 
#region propertyValues
        /// <summary> 
        /// Drive servo to the position set in the `position_sp` attribute.
        /// </summary>
        public const string CommandRun = "run";
        
        /// <summary> 
        /// Remove power from the motor.
        /// </summary>
        public const string CommandFloat = "float";
        
        /// <summary> 
        /// With `normal` polarity, a positive duty cycle will
        /// cause the motor to rotate clockwise.
        /// </summary>
        public const string PolarityNormal = "normal";
        
        /// <summary> 
        /// With `inversed` polarity, a positive duty cycle will
        /// cause the motor to rotate counter-clockwise.
        /// </summary>
        public const string PolarityInversed = "inversed";
        
#endregion 
#region systemProperties
        /// <summary> 
        /// Sets the command for the servo. Valid values are `run` and `float`. Setting
        /// to `run` will cause the servo to be driven to the position_sp set in the
        /// `position_sp` attribute. Setting to `float` will remove power from the motor.
        /// </summary>
        public string Command 
        { 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns the name of the motor driver that loaded this device. See the list
        /// of [supported devices] for a list of drivers.
        /// </summary>
        public string DriverName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Used to set the pulse size in milliseconds for the signal that tells the
        /// servo to drive to the maximum (clockwise) position_sp. Default value is 2400.
        /// Valid values are 2300 to 2700. You must write to the position_sp attribute for
        /// changes to this attribute to take effect.
        /// </summary>
        public string MaxPulseSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Used to set the pulse size in milliseconds for the signal that tells the
        /// servo to drive to the mid position_sp. Default value is 1500. Valid
        /// values are 1300 to 1700. For example, on a 180 degree servo, this would be
        /// 90 degrees. On continuous rotation servo, this is the 'neutral' position_sp
        /// where the motor does not turn. You must write to the position_sp attribute for
        /// changes to this attribute to take effect.
        /// </summary>
        public string MidPulseSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Used to set the pulse size in milliseconds for the signal that tells the
        /// servo to drive to the miniumum (counter-clockwise) position_sp. Default value
        /// is 600. Valid values are 300 to 700. You must write to the position_sp
        /// attribute for changes to this attribute to take effect.
        /// </summary>
        public string MinPulseSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Sets the polarity of the servo. Valid values are `normal` and `inversed`.
        /// Setting the value to `inversed` will cause the position_sp value to be
        /// inversed. i.e `-100` will correspond to `max_pulse_sp`, and `100` will
        /// correspond to `min_pulse_sp`.
        /// </summary>
        public string Polarity 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns the name of the port that the motor is connected to.
        /// </summary>
        public string PortName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Reading returns the current position_sp of the servo. Writing instructs the
        /// servo to move to the specified position_sp. Units are percent. Valid values
        /// are -100 to 100 (-100% to 100%) where `-100` corresponds to `min_pulse_sp`,
        /// `0` corresponds to `mid_pulse_sp` and `100` corresponds to `max_pulse_sp`.
        /// </summary>
        public string PositionSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Sets the rate_sp at which the servo travels from 0 to 100.0% (half of the full
        /// range of the servo). Units are in milliseconds. Example: Setting the rate_sp
        /// to 1000 means that it will take a 180 degree servo 2 second to move from 0
        /// to 180 degrees. Note: Some servo controllers may not support this in which
        /// case reading and writing will fail with `-EOPNOTSUPP`. In continuous rotation
        /// servos, this value will affect the rate_sp at which the speed ramps up or down.
        /// </summary>
        public string RateSp 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns a list of flags indicating the state of the servo.
        /// Possible values are:
        /// * `running`: Indicates that the motor is powered.
        /// </summary>
        public string[] State 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
#endregion 
    } 
    /// <summary> 
    /// Any device controlled by the generic LED driver. 
    /// See https://www.kernel.org/doc/Documentation/leds/leds-class.txt 
    /// for more details.
    /// 
    /// For online documentation see <see href=""></see>
    /// </summary>
    public partial class Led : Device
    { 
#region systemProperties
        /// <summary> 
        /// Returns the maximum allowable brightness value.
        /// </summary>
        public string MaxBrightness 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Sets the brightness level. Possible values are from 0 to `max_brightness`.
        /// </summary>
        public string Brightness 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns a list of available triggers.
        /// </summary>
        public string[] Triggers 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Sets the led trigger. A trigger
        /// is a kernel based source of led events. Triggers can either be simple or
        /// complex. A simple trigger isn't configurable and is designed to slot into
        /// existing subsystems with minimal additional code. Examples are the `ide-disk` and
        /// `nand-disk` triggers.
        /// 
        /// Complex triggers whilst available to all LEDs have LED specific
        /// parameters and work on a per LED basis. The `timer` trigger is an example.
        /// The `timer` trigger will periodically change the LED brightness between
        /// 0 and the current brightness setting. The `on` and `off` time can
        /// be specified via `delay_{on,off}` attributes in milliseconds.
        /// You can change the brightness value of a LED independently of the timer
        /// trigger. However, if you set the brightness value to 0 it will
        /// also disable the `timer` trigger.
        /// </summary>
        public string Trigger 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// The `timer` trigger will periodically change the LED brightness between
        /// 0 and the current brightness setting. The `on` time can
        /// be specified via `delay_on` attribute in milliseconds.
        /// </summary>
        public string DelayOn 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// The `timer` trigger will periodically change the LED brightness between
        /// 0 and the current brightness setting. The `off` time can
        /// be specified via `delay_off` attribute in milliseconds.
        /// </summary>
        public string DelayOff 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
#endregion 
    } 
    /// <summary> 
    /// Provides a generic button reading mechanism that can be adapted 
    /// to platform specific implementations. Each platform's specific 
    /// button capabilites are enumerated in the 'platforms' section 
    /// of this specification
    /// 
    /// For online documentation see <see href=""></see>
    /// </summary>
    public partial class Button : Device
    { 
    } 
    /// <summary> 
    /// The sensor class provides a uniform interface for using most of the 
    /// sensors available for the EV3. The various underlying device drivers will 
    /// create a `lego-sensor` device for interacting with the sensors. 
    ///  
    /// Sensors are primarily controlled by setting the `mode` and monitored by 
    /// reading the `value<N>` attributes. Values can be converted to floating point 
    /// if needed by `value<N>` / 10.0 ^ `decimals`. 
    ///  
    /// Since the name of the `sensor<N>` device node does not correspond to the port 
    /// that a sensor is plugged in to, you must look at the `port_name` attribute if 
    /// you need to know which port a sensor is plugged in to. However, if you don't 
    /// have more than one sensor of each type, you can just look for a matching 
    /// `driver_name`. Then it will not matter which port a sensor is plugged in to - your 
    /// program will still work.
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/drivers/lego-sensor-class/">http://www.ev3dev.org/docs/drivers/lego-sensor-class/</see>
    /// </summary>
    public partial class Sensor : Device
    { 
#region systemProperties
        /// <summary> 
        /// Sends a command to the sensor.
        /// </summary>
        public string Command 
        { 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns a list of the valid commands for the sensor.
        /// Returns -EOPNOTSUPP if no commands are supported.
        /// </summary>
        public string[] Commands 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the number of decimal places for the values in the `value<N>`
        /// attributes of the current mode.
        /// </summary>
        public string Decimals 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the name of the sensor device/driver. See the list of [supported
        /// sensors] for a complete list of drivers.
        /// </summary>
        public string DriverName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the current mode. Writing one of the values returned by `modes`
        /// sets the sensor to that mode.
        /// </summary>
        public string Mode 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns a list of the valid modes for the sensor.
        /// </summary>
        public string[] Modes 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the number of `value<N>` attributes that will return a valid value
        /// for the current mode.
        /// </summary>
        public string NumValues 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the name of the port that the sensor is connected to, e.g. `ev3:in1`.
        /// I2C sensors also include the I2C address (decimal), e.g. `ev3:in1:i2c8`.
        /// </summary>
        public string PortName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the units of the measured value for the current mode. May return
        /// empty string
        /// </summary>
        public string Units 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
#endregion 
    } 
    /// <summary> 
    /// A generic interface to control I2C-type EV3 sensors.
    /// 
    /// For online documentation see <see href=""></see>
    /// </summary>
    public partial class I2cSensor : Sensor
    { 
#region systemProperties
        /// <summary> 
        /// Returns the firmware version of the sensor if available. Currently only
        /// I2C/NXT sensors support this.
        /// </summary>
        public string FwVersion 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns the polling period of the sensor in milliseconds. Writing sets the
        /// polling period. Setting to 0 disables polling. Minimum value is hard
        /// coded as 50 msec. Returns -EOPNOTSUPP if changing polling is not supported.
        /// Currently only I2C/NXT sensors support changing the polling period.
        /// </summary>
        public string PollMs 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
#endregion 
    } 
    /// <summary> 
    /// LEGO EV3 color sensor.
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/sensors/lego-ev3-color-sensor/">http://www.ev3dev.org/docs/sensors/lego-ev3-color-sensor/</see>
    /// </summary>
    public partial class ColorSensor : Sensor
    { 
#region propertyValues
        /// <summary> 
        /// Reflected light. Red LED on.
        /// </summary>
        public const string ModeColReflect = "COL-REFLECT";
        
        /// <summary> 
        /// Ambient light. Red LEDs off.
        /// </summary>
        public const string ModeColAmbient = "COL-AMBIENT";
        
        /// <summary> 
        /// Color. All LEDs rapidly cycling, appears white.
        /// </summary>
        public const string ModeColColor = "COL-COLOR";
        
        /// <summary> 
        /// Raw reflected. Red LED on
        /// </summary>
        public const string ModeRefRaw = "REF-RAW";
        
        /// <summary> 
        /// Raw Color Components. All LEDs rapidly cycling, appears white.
        /// </summary>
        public const string ModeRgbRaw = "RGB-RAW";
        
#endregion 
    } 
    /// <summary> 
    /// LEGO EV3 ultrasonic sensor.
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/sensors/lego-ev3-ultrasonic-sensor/">http://www.ev3dev.org/docs/sensors/lego-ev3-ultrasonic-sensor/</see>
    /// </summary>
    public partial class UltrasonicSensor : Sensor
    { 
#region propertyValues
        /// <summary> 
        /// Continuous measurement in centimeters.
        /// LEDs: On, steady
        /// </summary>
        public const string ModeUsDistCm = "US-DIST-CM";
        
        /// <summary> 
        /// Continuous measurement in inches.
        /// LEDs: On, steady
        /// </summary>
        public const string ModeUsDistIn = "US-DIST-IN";
        
        /// <summary> 
        /// Listen.  LEDs: On, blinking
        /// </summary>
        public const string ModeUsListen = "US-LISTEN";
        
        /// <summary> 
        /// Single measurement in centimeters.
        /// LEDs: On momentarily when mode is set, then off
        /// </summary>
        public const string ModeUsSiCm = "US-SI-CM";
        
        /// <summary> 
        /// Single measurement in inches.
        /// LEDs: On momentarily when mode is set, then off
        /// </summary>
        public const string ModeUsSiIn = "US-SI-IN";
        
#endregion 
    } 
    /// <summary> 
    /// LEGO EV3 gyro sensor.
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/sensors/lego-ev3-gyro-sensor/">http://www.ev3dev.org/docs/sensors/lego-ev3-gyro-sensor/</see>
    /// </summary>
    public partial class GyroSensor : Sensor
    { 
#region propertyValues
        /// <summary> 
        /// Angle
        /// </summary>
        public const string ModeGyroAng = "GYRO-ANG";
        
        /// <summary> 
        /// Rotational speed
        /// </summary>
        public const string ModeGyroRate = "GYRO-RATE";
        
        /// <summary> 
        /// Raw sensor value
        /// </summary>
        public const string ModeGyroFas = "GYRO-FAS";
        
        /// <summary> 
        /// Angle and rotational speed
        /// </summary>
        public const string ModeGyroGAnda = "GYRO-G&A";
        
        /// <summary> 
        /// Calibration ???
        /// </summary>
        public const string ModeGyroCal = "GYRO-CAL";
        
#endregion 
    } 
    /// <summary> 
    /// LEGO EV3 infrared sensor.
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/sensors/lego-ev3-infrared-sensor/">http://www.ev3dev.org/docs/sensors/lego-ev3-infrared-sensor/</see>
    /// </summary>
    public partial class InfraredSensor : Sensor
    { 
#region propertyValues
        /// <summary> 
        /// Proximity
        /// </summary>
        public const string ModeIrProx = "IR-PROX";
        
        /// <summary> 
        /// IR Seeker
        /// </summary>
        public const string ModeIrSeek = "IR-SEEK";
        
        /// <summary> 
        /// IR Remote Control
        /// </summary>
        public const string ModeIrRemote = "IR-REMOTE";
        
        /// <summary> 
        /// IR Remote Control. State of the buttons is coded in binary
        /// </summary>
        public const string ModeIrRemA = "IR-REM-A";
        
        /// <summary> 
        /// Calibration ???
        /// </summary>
        public const string ModeIrCal = "IR-CAL";
        
#endregion 
    } 
    /// <summary> 
    /// LEGO NXT Sound Sensor
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/sensors/lego-nxt-sound-sensor/">http://www.ev3dev.org/docs/sensors/lego-nxt-sound-sensor/</see>
    /// </summary>
    public partial class SoundSensor : Sensor
    { 
#region propertyValues
        /// <summary> 
        /// Sound pressure level. Flat weighting
        /// </summary>
        public const string ModeDb = "DB";
        
        /// <summary> 
        /// Sound pressure level. A weighting
        /// </summary>
        public const string ModeDba = "DBA";
        
#endregion 
    } 
    /// <summary> 
    /// LEGO NXT Light Sensor
    /// 
    /// For online documentation see <see href="http://www.ev3dev.org/docs/sensors/lego-nxt-light-sensor/">http://www.ev3dev.org/docs/sensors/lego-nxt-light-sensor/</see>
    /// </summary>
    public partial class LightSensor : Sensor
    { 
#region propertyValues
        /// <summary> 
        /// Reflected light. LED on
        /// </summary>
        public const string ModeReflect = "REFLECT";
        
        /// <summary> 
        /// Ambient light. LED off
        /// </summary>
        public const string ModeAmbient = "AMBIENT";
        
#endregion 
    } 
    /// <summary> 
    /// Touch Sensor
    /// 
    /// For online documentation see <see href=""></see>
    /// </summary>
    public partial class TouchSensor : Sensor
    { 
    } 
    /// <summary> 
    /// A generic interface to read data from the system's power_supply class. 
    /// Uses the built-in legoev3-battery if none is specified.
    /// 
    /// For online documentation see <see href=""></see>
    /// </summary>
    public partial class PowerSupply : Device
    { 
#region systemProperties
        /// <summary> 
        /// The measured current that the battery is supplying (in microamps)
        /// </summary>
        public string MeasuredCurrent 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// The measured voltage that the battery is supplying (in microvolts)
        /// </summary>
        public string MeasuredVoltage 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// </summary>
        public string MaxVoltage 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// </summary>
        public string MinVoltage 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// </summary>
        public string Technology 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// </summary>
        public string Type 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
#endregion 
    } 
    /// <summary> 
    /// The `lego-port` class provides an interface for working with input and 
    /// output ports that are compatible with LEGO MINDSTORMS RCX/NXT/EV3, LEGO 
    /// WeDo and LEGO Power Functions sensors and motors. Supported devices include 
    /// the LEGO MINDSTORMS EV3 Intelligent Brick, the LEGO WeDo USB hub and 
    /// various sensor multiplexers from 3rd party manufacturers. 
    ///  
    /// Some types of ports may have multiple modes of operation. For example, the 
    /// input ports on the EV3 brick can communicate with sensors using UART, I2C 
    /// or analog validate signals - but not all at the same time. Therefore there 
    /// are multiple modes available to connect to the different types of sensors. 
    ///  
    /// In most cases, ports are able to automatically detect what type of sensor 
    /// or motor is connected. In some cases though, this must be manually specified 
    /// using the `mode` and `set_device` attributes. The `mode` attribute affects 
    /// how the port communicates with the connected device. For example the input 
    /// ports on the EV3 brick can communicate using UART, I2C or analog voltages, 
    /// but not all at the same time, so the mode must be set to the one that is 
    /// appropriate for the connected sensor. The `set_device` attribute is used to 
    /// specify the exact type of sensor that is connected. Note: the mode must be 
    /// correctly set before setting the sensor type. 
    ///  
    /// Ports can be found at `/sys/class/lego-port/port<N>` where `<N>` is 
    /// incremented each time a new port is registered. Note: The number is not 
    /// related to the actual port at all - use the `port_name` attribute to find 
    /// a specific port.
    /// 
    /// For online documentation see <see href=""></see>
    /// </summary>
    public partial class LegoPort : Device
    { 
#region systemProperties
        /// <summary> 
        /// Returns the name of the driver that loaded this device. You can find the
        /// complete list of drivers in the [list of port drivers].
        /// </summary>
        public string DriverName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// Returns a list of the available modes of the port.
        /// </summary>
        public string[] Modes 
        { 
            get 
            {
                return GetAttrSet("decimals");
            } 
        } 
        /// <summary> 
        /// Reading returns the currently selected mode. Writing sets the mode.
        /// Generally speaking when the mode changes any sensor or motor devices
        /// associated with the port will be removed new ones loaded, however this
        /// this will depend on the individual driver implementing this class.
        /// </summary>
        public string Mode 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// Returns the name of the port. See individual driver documentation for
        /// the name that will be returned.
        /// </summary>
        public string PortName 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
        /// <summary> 
        /// For modes that support it, writing the name of a driver will cause a new
        /// device to be registered for that driver and attached to this port. For
        /// example, since NXT/Analog sensors cannot be auto-detected, you must use
        /// this attribute to load the correct driver. Returns -EOPNOTSUPP if setting a
        /// device is not supported.
        /// </summary>
        public string SetDevice 
        { 
            set 
            {
                SetAttrString("command", value);
            } 
        } 
        /// <summary> 
        /// In most cases, reading status will return the same value as `mode`. In
        /// cases where there is an `auto` mode additional values may be returned,
        /// such as `no-device` or `error`. See individual port driver documentation
        /// for the full list of possible values.
        /// </summary>
        public string Status 
        { 
            get 
            {
                return GetAttrFromSet("decimals");
            } 
        } 
#endregion 
    } 
//~autogen
}