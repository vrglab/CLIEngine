using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CLIEngine.IO
{
    public class IniFile
    {

        public object this[string key]
        {
            get { return Read(key); }
            set { Write(key, value.ToString()); }
        }

        public object this[string key, string section]
        {
            get { return Read(key, section); }
            set { Write(key, value.ToString(), section); }
        }


        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal,
            int Size, string FilePath);

        public IniFile(string IniPath = null)
        {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName;
        }

        public string Read(string Key, string Section = null)
        {
            StringBuilder RetVal = new StringBuilder();
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, RetVal.Capacity, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }

        public Dictionary<string, string> ReadSection(string Section = null)
        {
            Dictionary<string, string> sectionData = new Dictionary<string, string>();
            StringBuilder keysBuffer = new StringBuilder(); // Adjust the buffer size as needed

            int bytesRead = GetPrivateProfileString(Section ?? EXE, null, "", keysBuffer, keysBuffer.Capacity, Path);

            if (bytesRead > 0)
            {
                string[] keys = keysBuffer.ToString().Split('\0');

                foreach (var key in keys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        string value = Read(key, Section);
                        sectionData[key] = value;
                    }
                }
            }

            return sectionData;
        }
    }
}
