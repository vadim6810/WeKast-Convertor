using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{
    public partial class MainForm : Form, ILogger
    {
        //EventLogger _el;
        public MainForm()
        {
            InitializeComponent();
        }

        private void DragEnterEvent(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void DropFileEvent(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                Debug.WriteLine(file);
                AppendLog(file);
                if (file == null) continue;
                var thread = new Thread(() => Convert(file));
                thread.Start();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var thread = new Thread(() => Convert(null));
            thread.Start();
        }

        private static void Convert(string file) => Converter.Converter.Convert(file, new DebugLogger());

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        public void AppendLog(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), s);
                return;
            }
            lstLog.Items.Add(s);
            CheckForTrim();
            lstLog.SelectedIndex = lstLog.Items.Count - 1;
            lstLog.SelectedIndex = -1;
        }

        private void CheckForTrim()
        {
            const int maxLineCount = 100;
            if (lstLog.Items.Count < maxLineCount + 10)
                return;
            for (var i = 0; i < 5; i++)
            {
                lstLog.Items.RemoveAt(0);
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var login = new LoginForm { StartPosition = FormStartPosition.CenterParent };
            login.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AboutForm { StartPosition = FormStartPosition.CenterParent };
            form.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}