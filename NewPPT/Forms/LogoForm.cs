using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeCastConvertor.Converter;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{
    public partial class LogoForm : Form
    {
        private bool showed = false;
        public LogoForm()
        {
            InitializeComponent();
        }

        private void LogoForm_Load(object sender, EventArgs e)
        {
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, 0);


            //Form newMDIChild = new Form();
            //// Set the Parent Form of the Child window.
            //newMDIChild.MdiParent = this;
            //// Display the new form.
            //newMDIChild.Show();
            //PictureBox newPicture = new PictureBox();
            //newPicture.Image = WeCastConvertor.Properties.Resources.logo;
            //newPicture.Click += pctLogo_Click;
            //Bitmap image = new Bitmap(WeCastConvertor.Properties.Resources.logo, true);
            //Graphics g = this.CreateGraphics();
            //g.DrawImage(image, new Point(5, 5));
        }

        private void pctLogo_Click(object sender, EventArgs e)
        {
            //if (showed)
            //    DisableForm();
            //else
            ShowForm();
        }

        private void ShowForm()
        {
            //IsMdiContainer = true;
            //Height = 300;
            MdiConvert mdiConvert = new MdiConvert(this);
            //mdiConvert.MdiParent = this;
            mdiConvert.Show();
            //BackColor = SystemColors.Desktop;
            //pctExit.Visible = true;
            //showed = true;
        }

        private void DisableForm()
        {
            //BackColor = TransparencyKey;
            pctExit.Visible = false;
            showed = false;

        }

        private void pctExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void pnlDrop_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                // Пропускаем неподдерживаемые форматы
                if (!Array.Exists(Wrapper.SupportedFormats, s => s.Equals(Path.GetExtension(file)))) continue;

                Console.WriteLine(file);
                AppendLog(file);
                var presentation = new Presentation() { SourcePath = file };
                await Convert(presentation);
            }
        }

        private void pnlDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private async Task<bool> Convert(Forms.Presentation presentation)
        {
            try
            {
                //InProgress++;
                //gridData.Add(presentation);
                await Wrapper.ConvertAsync(presentation);
                if (presentation.Convert != 100) return false;
                return await WeKastServerApi.Instance.Upload(presentation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                //InProgress--;
            }
        }

        public void AppendLog(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), s);
                return;
            }
            //LogWindow.Items.Add(s);
            //CheckForTrim();
            //LogWindow.SelectedIndex = LogWindow.Items.Count - 1;
            //LogWindow.SelectedIndex = -1;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pnlDrop_Paint(object sender, PaintEventArgs e)
        {
            Pen yellowPen = new Pen(Color.Yellow, 5);
            //Graphics g = pnlDrop.CreateGraphics();
            e.Graphics.DrawRectangle(yellowPen, 10, 10, 390, 170);
            //' Draw a blue square
            //g.DrawRectangle(Pens.Yellow, 5, 5, 390, 170);
        }
    }
}
