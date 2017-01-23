using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{

    public partial class LogoForm : Form
    {

        private readonly MdiConvert _mdiConvert;

        public LogoForm()
        {
            InitializeComponent();

            _mdiConvert = new MdiConvert(this);

        }

        private async void LogoForm_Load(object sender, EventArgs e)
        {
            try
            {
                Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, 0);
                if (!SharedPreferences.IsSet())
                {
                    LoginDialogStartupOrExit();
                }
                else
                {
                    //Cursor = Cursors.WaitCursor;
                    var authResult = await WeKastServerApi.Instance.Auth();
                    //Cursor = Cursors.Default;
                    if (authResult.Status != 0)
                    {
                        LoginDialogStartupOrExit(authResult.Message);
                    }
                }
            }
            catch (HttpRequestException exception)
            {
                var message = new StringBuilder().AppendLine(exception.Message).AppendLine(exception.InnerException?.Message).ToString();
                MessageBox.Show( message, @"Http request exception",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }

        }

        private static void LoginDialogStartupOrExit(string message = null)
        {
            var login = LoginForm.GetInstance();
            if (message != null)
                login.SetMessage(message);
            var dialogResult = login.ShowDialog();
            if (dialogResult == DialogResult.Cancel)
            {
                Application.Exit();
            }
        }

        private void pctLogo_Click(object sender, EventArgs e)
        {
            _mdiConvert.Show();
            Hide();
        }

        private void pctExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}