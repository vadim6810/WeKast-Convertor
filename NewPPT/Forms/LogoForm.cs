using System;
using System.Drawing;
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
                    LoginDialogStartupOrExit(FormStartPosition.CenterParent, authResult.Message);
                }
            }
        }

        private static void LoginDialogStartupOrExit(FormStartPosition startPosition = FormStartPosition.CenterScreen,
            string message = null)
        {
            var login = LoginForm.GetInstance();// {StartPosition = startPosition};
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