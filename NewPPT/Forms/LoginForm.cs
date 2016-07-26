using Microsoft.Win32;
using System;
using System.Windows.Forms;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{
    public partial class LoginForm : Form
    {    
        const string appKeyPath = "HKEY_CURRENT_USER\\Software\\WeKast";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Console.WriteLine(Registry.GetValue(appKeyPath, "login", "no"));
            login_textbox.Text = SharedPreferences.login;
            password_textbox.Text = SharedPreferences.password;
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            SharedPreferences.login = login_textbox.Text;
            SharedPreferences.password = password_textbox.Text;
        }
    }
}
