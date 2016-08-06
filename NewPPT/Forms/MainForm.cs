using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeCastConvertor.Converter;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{
    public partial class MainForm : Form, ILogger
    {
        public int InProgress { get; set; }

        // Constructor
        public MainForm()
        {
            InProgress = 0;
            InitializeComponent();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private async void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                Debug.WriteLine(file);
                AppendLog(file);
                var presentation = new Presentation() { SourcePath = file };
                await Convert(presentation);
            }
        }


        private async Task<bool> Convert(Presentation presentation)
        {
            InProgress++;
            gridData.Add(presentation);
            await Wrapper.ConvertAsync(presentation);
            if (presentation.Convert == 100)
            {               
                var result = await WeKastServerAPI.Instance.Upload(presentation);
                InProgress--;
                return result;
            }
            else
            {
                return false;
            }
            
        }
        
        // Logger
        public void AppendLog(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), s);
                return;
            }
            LogWindow.Items.Add(s);
            CheckForTrim();
            LogWindow.SelectedIndex = LogWindow.Items.Count - 1;
            LogWindow.SelectedIndex = -1;
        }

        private void CheckForTrim()
        {
            const int maxLineCount = 100;
            if (LogWindow.Items.Count < maxLineCount + 10)
                return;
            for (var i = 0; i < 5; i++)
            {
                LogWindow.Items.RemoveAt(0);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (InProgress > 0)
            {
                e.Cancel = true;
            }   
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            var login = new LoginForm { StartPosition = FormStartPosition.CenterParent };
            login.ShowDialog();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            var form = new AboutForm { StartPosition = FormStartPosition.CenterParent };
            form.ShowDialog();
        }

        private void ExitButton_Click(object sender, EventArgs e)
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