using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class Localization
    {
        public static void Set(string text)
        {
            values.Clear();
            Add(text);
        }
        public static void Add(string text)
        {
            string key = "", val = "";
            int status = 0;
            foreach (char c in text)
            {
                if (c == '"') status++;
                else if (status == 1) key += c;
                else if (status == 4) val += c;
                else if (status == 2 && c == '=') status++;
                if (status == 5)
                {
                    values.Add(key.Replace("\\dq", "\""), val.Replace("\\dq", "\""));
                    key = val = "";
                    status = 0;
                }
            }
        }
        private static Dictionary<string, string> values = new Dictionary<string, string>();
        public static string Get(string index) => values.ContainsKey(index) ? values[index] : string.Empty ;
    }
}
