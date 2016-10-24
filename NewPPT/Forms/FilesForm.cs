﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using WeCastConvertor.Properties;
using WeCastConvertor.Utils;

namespace WeCastConvertor.Forms
{
    public partial class FilesForm : Form
    {
        private static readonly ArrayList Presantations = new ArrayList();
        private Dictionary<string, Image> Previews { get; } = new Dictionary<string, Image>();

        private MdiConvert parentForm { get; }

        public FilesForm()
        {
            InitializeComponent();
        }

        public FilesForm(MdiConvert mdiConvert)
        {
            InitializeComponent();
            parentForm = mdiConvert;
        }

        private void FilesForm_Load(object sender, EventArgs e)
        {
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, parentForm.Height);
            LoadPresantationList();
        }

        private async void LoadPresantationList()
        {
            Cursor = Cursors.WaitCursor;
            var api = WeKastServerApi.Instance;
            var serverPresantations = await api.ListAsync();
            Presantations.Clear();
            foreach (var pres in serverPresantations.Answer)
            {
                Presantations.Add(new PresantationTamplate(pres.Id, System.IO.Path.GetFileNameWithoutExtension(pres.Name), pres.Hash));
                if (!Previews.ContainsKey(pres.Hash))
                {
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
            result.Controls.Add(getPictureBox(Resources.show), 0, 0);
            result.Controls.Add(getPictureBox(Resources.favorite_passiv), 1, 0);
            result.Controls.Add(getPictureBox(Resources.menu), 2, 0);
            result.Dock = DockStyle.Fill;
            return result;
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
            result.Controls.Add(GetNewLabel("ppt"), 0, 1);
            result.Controls.Add(GetNewLabel("100M"), 1, 1);
            result.Controls.Add(GetNewLabel(DateTime.Now.ToShortDateString()), 2, 1);
            result.Dock = DockStyle.Fill;
            return result;
        }

        private Label GetNewLabel(string name)
        {
            var result = new Label { Text = name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, AutoSize = true };
            result.Click += tableLayoutPanel2_Click;
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

        private void tableLayoutPanel2_Click(object sender, EventArgs e)
        {
            var row = tblContent.GetRow((Control)sender);
            Debug.WriteLine("Panel row: {0}", row);
            int presIndex = row - 2;
            var pres = (PresantationTamplate)Presantations[presIndex];
            Debug.WriteLine("Presantation name: " + pres.Name);
        }

    }

    internal class PresantationTamplate
    {
        public PresantationTamplate(int id, string name, string hash)
        {
            Id = id;
            Name = name;
            Hash = hash;
        }

        public int Id { get; }

        public string Name { get; }

        public string Hash { get; }

    }
}