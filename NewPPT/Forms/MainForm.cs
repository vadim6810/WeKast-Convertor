using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{

    public partial class MainForm : Form, ILogger 
    {
        // Constructor
        public MainForm()
        {
            InitializeComponent();
        }

       

        // Form Load Event
        private void MainForm_Load(object sender, EventArgs e)
        {
            AllowDrop = true;
            DragEnter += DragEnterEvent;
            DragDrop += DropFileEvent;
            
        }

        // Form Close Event
        private void MainForm_Close(object sender, FormClosingEventArgs e)
        {
            //_el.DetachEvents();
        }

        // Mouse Enter With Object Event
        private static void DragEnterEvent(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        // Drop Event
        private void DropFileEvent(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files) {
                Debug.WriteLine(file);
                AppendLog(file);
                gridData.Add(new Presentation() { SourcePath = file });
                //var thread = new Thread(Convert);
                //thread.Start();
            }
        }

        // Click 
        private void button1_Click(object sender, EventArgs e)
        {
            //var thread = new Thread(Convert);
            //thread.Start();
            gridData.Add(new Presentation() { SourcePath = "Msasd", EzsPath = "eewqweq" });
        }

        private void Convert() => Converter.Converter.Convert(new DebugLogger());


        
        // Logger
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
            var maxLineCount = 100;
            if (lstLog.Items.Count < maxLineCount + 10)
                return;
            for (var i = 0; i < 5; i++)
            {
                lstLog.Items.RemoveAt(0);
            }
        }

        


        // Menu events
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm
            {
                StartPosition = FormStartPosition.CenterParent
            };
            login.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class Presentation
    {
        public string SourcePath { get; set; }
        public string EzsPath { get; set; }
        public int Upload { get; set; }
        public int Convert { get; set; }
    }

}