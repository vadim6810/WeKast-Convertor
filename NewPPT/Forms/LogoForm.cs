using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeCastConvertor.Forms
{
    public partial class LogoForm : Form
    {
        public LogoForm()
        {
            InitializeComponent();
        }

        private void LogoForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show(Width.ToString());
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,0);
            //Left = ;
            //Top = 50;
        }

        private void pctLogo_Click(object sender, EventArgs e)
        {

            var mainForm = new MainForm
            {
                StartPosition = FormStartPosition.Manual
            };
            mainForm.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - mainForm.Width, 0);
            mainForm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
