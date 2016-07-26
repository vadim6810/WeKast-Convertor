using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeCastConvertor.Utils
{
    static class SharedPreferences
    {
        private const string appKeyPath = "HKEY_CURRENT_USER\\Software\\WeKast";

        public static string login
        {
            get { return (string)Registry.GetValue(appKeyPath, "login", ""); }
            set { Registry.SetValue(appKeyPath, "login", value); }
        }

        public static string password
        {
            get { return (string)Registry.GetValue(appKeyPath, "password", ""); }
            set { Registry.SetValue(appKeyPath, "password", value); }
        }
    }
}
