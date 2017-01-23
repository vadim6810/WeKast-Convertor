using System;
using System.Windows.Forms;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{
    public partial class LoginForm : Form
    {
        static LoginForm _instance;

        private LoginForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            login_textbox.Text = SharedPreferences.Login;
            password_textbox.Text = SharedPreferences.Password;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            //Close();
            Hide();
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {

            var api = WeKastServerApi.Instance;
            api.Login = login_textbox.Text;
            api.Password = password_textbox.Text;

            Cursor = Cursors.WaitCursor;
            LoginGroup.Enabled = false;
            var res = await api.Auth();
            LoginGroup.Enabled = true;

            Cursor = Cursors.Default;


            if (res.Status == 0)
            {
                SharedPreferences.Login = api.Login;
                SharedPreferences.Password = api.Password;
                DialogResult = DialogResult.OK;
                //Close();
                Hide();
            }
            else
            {
                SetMessage(res.Message);
                api.Login = SharedPreferences.Login;
                api.Password = SharedPreferences.Password;
                MessageBox.Show(@"Please check login and password", @"Auth faild", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        internal void SetMessage(string message)
        {
            throw new NotImplementedException();
        }

        public static LoginForm GetInstance()
        {
            return _instance ?? (_instance = new LoginForm());
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instance = null;
        }
    }
}
