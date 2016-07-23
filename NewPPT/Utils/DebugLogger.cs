using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCastConvertor.Utils
{
    class DebugLogger : ILogger
    {
        public void AppendLog(string s)
        {
            Debug.WriteLine(s);
        }
    }
}
