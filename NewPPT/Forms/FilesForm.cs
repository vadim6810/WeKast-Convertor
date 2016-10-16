using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeCastConvertor.Forms
{
    public partial class FilesForm : Form
    {
        ArrayList presantetions = new ArrayList();

        public FilesForm()
        {
            InitializeComponent();
        }

        private void FilesForm_Load(object sender, EventArgs e)
        {
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, 300);
        }
    }

    internal class TPresantationElement
    {
        int id;
        String presantationName;
        
    }
}
