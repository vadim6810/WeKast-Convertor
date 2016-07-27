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

        private async void save_button_Click(object sender, EventArgs e)
        {

            var api = WeKastServerAPI.Instance;
            api.serverUrl = @"http://78.153.150.254/";
            api.login = login_textbox.Text;
            api.password = password_textbox.Text;

            Cursor = Cursors.WaitCursor;
            login_group.Enabled = false;
            var res = await api.auth();
            login_group.Enabled = true;

            Cursor = Cursors.Default;


            if (res) {
                SharedPreferences.login = api.login;
                SharedPreferences.password = api.password;
                Close();
            } else {
                api.login = SharedPreferences.login;
                api.password = SharedPreferences.password;
                MessageBox.Show("Please check login and password", "Auth faild", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
    }
}
