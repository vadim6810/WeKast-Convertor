using Microsoft.Win32;

namespace WeCastConvertor.Utils
{
    static class SharedPreferences
    {
        private const string AppKeyPath = "HKEY_CURRENT_USER\\Software\\WeKast";

        public static string Login
        {
            get { return (string)Registry.GetValue(AppKeyPath, "Login", ""); }
            set { Registry.SetValue(AppKeyPath, "Login", value); }
        }

        public static string Password
        {
            get { return (string)Registry.GetValue(AppKeyPath, "password", ""); }
            set { Registry.SetValue(AppKeyPath, "password", value); }
        }
    }
}
