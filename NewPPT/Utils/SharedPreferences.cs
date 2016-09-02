using System;
using Microsoft.Win32;

namespace WeCastConvertor.Utils
{
    internal static class SharedPreferences
    {
        private const string AppKeyPath = @"HKEY_CURRENT_USER\Software\WeKast";

        private const string LoginName = "Login";
        private const string PasswordName = "Password";

        public static string Login
        {
            get { return GetValue(LoginName); }
            set { SetValue(LoginName, value); }
        }

        public static string Password
        {
            get { return GetValue(PasswordName); }
            set { SetValue(PasswordName, value); }
        }

        public static bool IsSet()
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
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
