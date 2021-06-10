using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SentenceAndWordCounter.Core
{
    public static class Tools
    {
        public static string readFile(string path)
        {
            if (!File.Exists(path)) return null;
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
