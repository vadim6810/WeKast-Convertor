using System;
using System.Windows.Forms;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{
    public partial class LoginForm : Form
    {    

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            login_textbox.Text = SharedPreferences.Login;
            password_textbox.Text = SharedPreferences.Password;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {

            var api = WeKastServerApi.Instance;
            api.Login = login_textbox.Text;
            api.Password = password_textbox.Text;

            Cursor = Cursors.WaitCursor;
            login_group.Enabled = false;
            var res = await api.Auth();
            login_group.Enabled = true;

            Cursor = Cursors.Default;


            if (res) {
                SharedPreferences.Login = api.Login;
                SharedPreferences.Password = api.Password;
                DialogResult = DialogResult.OK;
                Close();
            } else {
                api.Login = SharedPreferences.Login;
                api.Password = SharedPreferences.Password;
                MessageBox.Show(@"Please check login and password", @"Auth faild", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
