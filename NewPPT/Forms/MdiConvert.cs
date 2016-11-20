﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeCastConvertor.Converter;
using WeCastConvertor.Properties;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{
    public partial class MdiConvert : Form
    {
        private readonly FilesForm _filesForm;

        private readonly LogoForm _parentForm;

        public MdiConvert()
        {
            _filesForm = new FilesForm(this);
            InProgress = 0;
            InitializeComponent();
            CheckLogin();
        }

        public MdiConvert(LogoForm logoForm)
        {
            InitializeComponent();
            _parentForm = logoForm;
            _filesForm = new FilesForm(this);
            AttachHandlers();
        }

        private void AttachHandlers()
        {
            ProcessHandler.StatusChanged += ShowStatus;
            ProcessHandler.ProgressChanged += ShowProgress;
            ProcessHandler.SizeChanged += ShowFileSize;
        }

        public int InProgress { get; set; }

        private void MdiConvert_Load(object sender, EventArgs e)
        {
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, 0);
        }

        private void pctExit_Click(object sender, EventArgs e)
        {
            _parentForm.Close();
        }

        private void pctLogo_Click(object sender, EventArgs e)
        {
            _parentForm.Show();
            _filesForm.Hide();
            pictureBox1.Image = Resources.show_active;
            Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var greyDoted = new Pen(Color.Gray, 3) {DashPattern = new float[] {5, 5}};
            e.Graphics.DrawRectangle(greyDoted, 5, 5, 410, 160);
        }

        private void pctSelectFiles_Paint(object sender, PaintEventArgs e)
        {
            // Set a new rectangle to the same size as the button's 
            // ClientRectangle property.
            var box = (PictureBox) sender;
            var newRectangle = box.ClientRectangle;

            // Draw the button's border.
            var x = newRectangle.X;
            var y = newRectangle.Y;
            var width = newRectangle.Width;
            var height = newRectangle.Height;
            var radius = height/2;
            var regionPath = new GraphicsPath();
            regionPath.AddLine(x + radius, y, x + width - 2*radius, y); // Line
            regionPath.AddArc(x + width - 2*radius, y, radius*2, radius*2, 270, 180); // Corner
            regionPath.AddLine(x + radius, y + height, x + width - 2*radius, y + height); // Line
            regionPath.AddArc(x, y, radius*2, radius*2, 90, 180); // Corner

            regionPath.CloseFigure();

            // Set the button's Region property to the newly created 
            box.Region = new Region(regionPath);
        }

        private void pctSelectFiles_MouseDown(object sender, MouseEventArgs e)
        {
            var box = (PictureBox) sender;
            box.BorderStyle = BorderStyle.FixedSingle;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var box = (PictureBox) sender;
            box.BorderStyle = BorderStyle.None;
        }

        private async void pnlDrop_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                // Пропускаем неподдерживаемые форматы
                if (!Array.Exists(Wrapper.SupportedFormats, s => s.Equals(Path.GetExtension(file)))) continue;

                Console.WriteLine(file);
                AppendLog(file);
                var presentation = new Presentation {SourcePath = file};
                await Convert(presentation);
            }
            _filesForm.LoadPresantationList();
        }

        private void pnlDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private async Task<bool> Convert(Presentation presentation)
        {
            try
            {
                InProgress++;
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
                InProgress--;
            }
        }

        private void ShowStatus(string message)
        {
            if (ControlInvokeRequired(lblStatusMessage, () => ShowStatus(message))) return;
            lblStatusMessage.Text = message;
        }

        private void ShowProgress(int value)
        {
            if (ControlInvokeRequired(pgsStatusProgress, () => ShowProgress(value))) return;
            //pgsStatusProgress.Maximum = 100;
            pgsStatusProgress.Show();
            pgsStatusProgress.Value = value;
            if (pgsStatusProgress.Value == pgsStatusProgress.Maximum)
                pgsStatusProgress.Hide();
        }

        private void ShowFileSize(string uploaded, string total)
        {
            if (ControlInvokeRequired(lblStatusSize, () => ShowFileSize(uploaded, total))) return;
            lblStatusSize.Show();
            lblStatusSize.Text = uploaded+@"/"+total;
            if (uploaded.Equals(total))
                lblStatusSize.Hide();
        }


        private bool ControlInvokeRequired(Control c, Action a)
        {
            if (c.InvokeRequired) c.Invoke(new MethodInvoker(delegate { a(); }));
            else return false;

            return true;
        }

        public void AppendLog(string s)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), s);
            }
            //LogWindow.Items.Add(s);
            //CheckForTrim();
            //LogWindow.SelectedIndex = LogWindow.Items.Count - 1;
            //LogWindow.SelectedIndex = -1;
        }

        private async void pctSelectFiles_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = @"Now Supported Formats (*.pptx, *.ppt)|*.pptx;*.ppt" +
                         @"|All Supported Formats (*.pptx, *.ppt, *.pdf, *.doc, *.docx)|*.pptx;*.ppt;*.pdf;*.doc;*.docx" +
                         @"|All PowerPoint Presentations (*.pptx;*.ppt;*.pptm;*.ppsx;*.pps;*.ppsm;*.potx;*.pot;*.potm;*.odp)|*.pptx;*.ppt;*.pptm;*.ppsx;*.pps;*.ppsm;*.potx;*.pot;*.potm;*.odp",
                Title = @"Select presentation for convert"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            var presentation = new Presentation {SourcePath = openFileDialog.FileName};
            await Convert(presentation);
        }

        private void pctSettings_Click(object sender, EventArgs e)
        {
            var settingsForm = new LoginForm();
            settingsForm.Show();
        }

        private void CheckLogin()
        {
            var login = new LoginForm {StartPosition = FormStartPosition.CenterParent};
            login.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("this: {0}", this);
            if (_filesForm.Visible == false)
            {
                _filesForm.Show();
                pictureBox1.Image = Resources.hide;
            }
            else
            {
                _filesForm.Hide();
                pictureBox1.Image = Resources.show_active;
            }
        }
    }
}