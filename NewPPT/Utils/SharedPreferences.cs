using System;
using Microsoft.Win32;

namespace WeCastConvertor.Utils
{
    internal static class SharedPreferences
    {
        private const string AppKeyPath = "HKEY_CURRENT_USER\\Software\\WeKast";

        public static string Login
        {
            get { return GetValue("Login"); }
            set { SetValue("Login", value); }
        }

        public static string Password
        {
            get { return GetValue("password"); }
            set { SetValue("password", value); }
        }

        public static bool IsSet()
        {
            return "".Equals(Login) && "".Equals(Password);
        }

        private static string GetValue(string name)
        {
            return (string) Registry.GetValue(AppKeyPath, name, "");
        }

        private static void SetValue(string name, string value)
        {
            Registry.SetValue(AppKeyPath, name, value);
        }
    }
}
