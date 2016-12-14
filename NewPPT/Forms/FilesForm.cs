using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WeCastConvertor.Properties;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{
    public partial class FilesForm : Form
    {
        private static readonly ArrayList Presantations = new ArrayList();
        private Dictionary<string, Image> Previews { get; } = new Dictionary<string, Image>();

        private readonly MdiConvert _parentForm;

        public FilesForm()
        {
            InitializeComponent();
        }

        public FilesForm(MdiConvert mdiConvert)
        {
            InitializeComponent();
            _parentForm = mdiConvert;
            LoadPresantationList();
        }

        private void FilesForm_Load(object sender, EventArgs e)
        {
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, _parentForm.Height);
            LoadPresantationList();
        }

        public async void LoadPresantationList()
        {
            Cursor = Cursors.WaitCursor;
            var api = WeKastServerApi.Instance;
            var serverPresantations = await api.ListAsync();
            Presantations.Clear();
            foreach (var pres in serverPresantations.Answer)
            {
                var date = DateTime.ParseExact(pres.Date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                Presantations.Add(new PresantationTamplate(pres.Id, System.IO.Path.GetFileNameWithoutExtension(pres.Name), pres.Hash, pres.Type, pres.Size, date));
                if (!Previews.ContainsKey(pres.Hash))
                {
                    //TODO throws exception
                    Previews[pres.Hash] = await WeKastServerApi.Instance.Preview(pres.Id);
                }
            }
            ShowFileList();
            Cursor = DefaultCursor;
        }

        private void ShowFileList()
        {
            lblFiles.Text = $"Files({Presantations.Count})";
            tblContent.RowCount = 2;
            tblContent.RowStyles.Clear();
            tblContent.Controls.Clear();
            tblContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            foreach (PresantationTamplate pres in Presantations)
            {
                // For Add New Row (Loop this code for add multiple rows)
                tblContent.RowCount = tblContent.RowCount + 1;
                tblContent.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                int insertedRow = tblContent.RowCount - 2;
                //Image preview;
                if (Previews.ContainsKey(pres.Hash))
                    tblContent.Controls.Add(getPictureBox(Previews[pres.Hash]), 0, insertedRow);
                tblContent.Controls.Add(GetPresLayout(pres), 1, insertedRow);
                tblContent.Controls.Add(GetIconLayout(), 2, insertedRow);
            }
        }

        private Control GetIconLayout()
        {
            var result = new TableLayoutPanel
            {
                RowCount = 1,
                ColumnCount = 3
            };
            result.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            result.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            result.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            result.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33));
            PictureBox showing = getPictureBox(Resources.show_active);
            showing.Click += ShowHideAdditionalPanel;
            result.Controls.Add(showing, 0, 0);
            result.Controls.Add(getPictureBox(Resources.favorite_passiv), 1, 0);
            result.Controls.Add(getPictureBox(Resources.menu), 2, 0);
            result.Dock = DockStyle.Fill;
            return result;
        }

        private void ShowHideAdditionalPanel(object sender, EventArgs e)
        {
            var senderPicture = (PictureBox)sender;
            Debug.WriteLine(sender.ToString());
            TableLayoutPanel parentPanel = (TableLayoutPanel)senderPicture.Parent;
            Debug.WriteLine(parentPanel.ToString());
            if (parentPanel.RowCount == 1)
            {
                ShowAdditionalPanel(parentPanel);
                senderPicture.Image = Resources.hide;
            }
            else
            {
                HideAdditionalPanel(parentPanel);
                senderPicture.Image = Resources.show_active;
            }
        }

        private void HideAdditionalPanel(TableLayoutPanel panel)
        {
            for (var i = 0; i < panel.ColumnCount; i++)
            {
                var control = panel.GetControlFromPosition(i, 1);
                panel.Controls.Remove(control);
                control.Dispose();
            }
            panel.RowCount = 1;
        }

        private void ShowAdditionalPanel(TableLayoutPanel panel)
        {
            panel.RowCount = 2;
            panel.RowStyles.Clear();
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            panel.Controls.Add(getPictureBox(Resources.folder), 0, 1);
            panel.Controls.Add(getPictureBox(Resources.edit), 1, 1);
            var deletePicture = getPictureBox(Resources.delete_active);
            deletePicture.Click += DeletePresantation;
            panel.Controls.Add(deletePicture, 2, 1);
        }

        private async void DeletePresantation(object sender, EventArgs e)
        {
            var senderPicture = (PictureBox)sender;
            var presId = GetPresId(senderPicture);
            Debug.WriteLine("Presentation Id = " + presId);
            var responce = await WeKastServerApi.Instance.Delete(presId);
            //responce.Wait();
            if (responce)
                LoadPresantationList();
        }

        private int GetPresId(PictureBox senderPicture)
        {
            var panelIcons = (TableLayoutPanel)senderPicture.Parent;
            var filesPanel = (TableLayoutPanel)panelIcons.Parent;
            var raw = filesPanel.GetRow(panelIcons);
            Debug.WriteLine("raw = " + raw);
            return ((PresantationTamplate)Presantations[raw - 1]).Id;
        }

        private Control GetPresLayout(PresantationTamplate pres)
        {
            TableLayoutPanel result = new TableLayoutPanel();

            result.ForeColor = Color.Gray;
            result.RowCount = 2;
            result.ColumnCount = 3;
            result.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            result.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            result.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            result.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            result.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            var label = GetNewLabel(pres.Name);
            label.ForeColor = Color.White;
            result.Controls.Add(label, 0, 0);
            label.Dock = DockStyle.Left;
            label.TextAlign = ContentAlignment.MiddleLeft;
            result.SetColumnSpan(label, 3);
            result.Controls.Add(GetNewLabel(pres.Type), 0, 1);
            result.Controls.Add(GetNewLabel(Utils.Utils.GetFileSize(pres.Size)), 1, 1);
            result.Controls.Add(GetNewLabel(pres.Date.ToShortDateString()), 2, 1);
            result.Dock = DockStyle.Fill;
            return result;
        }

        //private string GetFileSize(int size)
        //{
        //    var len = size;
        //    string[] sizes = { "", "K", "M", "G" };
        //    var order = 0;
        //    while (len >= 1024 && ++order < sizes.Length)
        //    {
        //        len = len / 1024;
        //    }
        //    return $"{len}{sizes[order]}";
        //}

        private Label GetNewLabel(string name)
        {
            var result = new Label { Text = name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, AutoSize = true };
            //result.Click += tableLayoutPanel2_Click;
            return result;
        }

        private PictureBox getPictureBox(Image preview)
        {
            var result = new PictureBox
            {
                Image = preview,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            return result;
        }

        private void lblFiles_Click(object sender, EventArgs e)
        {
            LoadPresantationList();
        }

        private void FilesForm_Activated(object sender, EventArgs e)
        {
        }
    }

    internal class PresantationTamplate
    {
        public PresantationTamplate(int id, string name, string hash, string type, int size, DateTime date)
        {
            Id = id;
            Name = name;
            Hash = hash;
            Type = type.Equals("unknown") ? "unk" : type;
            Size = size;
            Date = date;
        }

        public int Size { get; }

        public string Type { get; }

        public int Id { get; }

        public string Name { get; }

        public string Hash { get; }
        public DateTime Date { get; }
    }
}