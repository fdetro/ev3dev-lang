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
//~autogen csharp-class-drivers classes>classes

    /// <summary> 
    /// The motor class provides a uniform interface for using motors with 
    /// positional and directional feedback such as the EV3 and NXT motors. 
    /// This feedback allows for precise control of the motors. This is the 
    /// most common type of motor, so we just call it `motor`.
    /// </summary>
    public partial class Motor
    {  
    } 
    
    /// <summary> 
    /// EV3 large servo motor
    /// </summary>
    public partial class LargeMotor
    { 
        public const string DriverLegoEv3LMotor = "lego-ev3-l-motor";  
    } 
    
    /// <summary> 
    /// EV3 medium servo motor
    /// </summary>
    public partial class MediumMotor
    { 
        public const string DriverLegoEv3MMotor = "lego-ev3-m-motor";  
    } 
    
    /// <summary> 
    /// The DC motor class provides a uniform interface for using regular DC motors 
    /// with no fancy controls or feedback. This includes LEGO MINDSTORMS RCX motors 
    /// and LEGO Power Functions motors.
    /// </summary>
    public partial class DcMotor
    {  
    } 
    
    /// <summary> 
    /// The servo motor class provides a uniform interface for using hobby type 
    /// servo motors.
    /// </summary>
    public partial class ServoMotor
    {  
    } 
    
    /// <summary> 
    /// Any device controlled by the generic LED driver. 
    /// See https://www.kernel.org/doc/Documentation/leds/leds-class.txt 
    /// for more details.
    /// </summary>
    public partial class Led
    {  
    } 
    
    /// <summary> 
    /// Provides a generic button reading mechanism that can be adapted 
    /// to platform specific implementations. Each platform's specific 
    /// button capabilites are enumerated in the 'platforms' section 
    /// of this specification
    /// </summary>
    public partial class Button
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
    /// </summary>
    public partial class Sensor
    {  
    } 
    
    /// <summary> 
    /// A generic interface to control I2C-type EV3 sensors.
    /// </summary>
    public partial class I2cSensor
    { 
        public const string DriverNxtI2cSensor = "nxt-i2c-sensor";  
    } 
    
    /// <summary> 
    /// LEGO EV3 color sensor.
    /// </summary>
    public partial class ColorSensor
    { 
        public const string DriverLegoEv3Color = "lego-ev3-color";  
    } 
    
    /// <summary> 
    /// LEGO EV3 ultrasonic sensor.
    /// </summary>
    public partial class UltrasonicSensor
    { 
        public const string DriverLegoEv3Us = "lego-ev3-us"; 
        public const string DriverLegoNxtUs = "lego-nxt-us";  
    } 
    
    /// <summary> 
    /// LEGO EV3 gyro sensor.
    /// </summary>
    public partial class GyroSensor
    { 
        public const string DriverLegoEv3Gyro = "lego-ev3-gyro";  
    } 
    
    /// <summary> 
    /// LEGO EV3 infrared sensor.
    /// </summary>
    public partial class InfraredSensor
    { 
        public const string DriverLegoEv3Ir = "lego-ev3-ir";  
    } 
    
    /// <summary> 
    /// LEGO NXT Sound Sensor
    /// </summary>
    public partial class SoundSensor
    { 
        public const string DriverLegoNxtSound = "lego-nxt-sound";  
    } 
    
    /// <summary> 
    /// LEGO NXT Light Sensor
    /// </summary>
    public partial class LightSensor
    { 
        public const string DriverLegoNxtLight = "lego-nxt-light";  
    } 
    
    /// <summary> 
    /// Touch Sensor
    /// </summary>
    public partial class TouchSensor
    { 
        public const string DriverLegoEv3Touch = "lego-ev3-touch"; 
        public const string DriverLegoNxtTouch = "lego-nxt-touch";  
    } 
    
    /// <summary> 
    /// A generic interface to read data from the system's power_supply class. 
    /// Uses the built-in legoev3-battery if none is specified.
    /// </summary>
    public partial class PowerSupply
    {  
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
    /// </summary>
    public partial class LegoPort
    {  
    } 
    
//~autogen
}