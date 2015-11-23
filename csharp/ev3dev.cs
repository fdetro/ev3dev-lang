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
}