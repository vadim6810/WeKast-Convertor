﻿using System;
using System.Threading;
using System.Windows.Forms;
using WeCastConvertor.Utils;

namespace WeCastConvertor
{
    public partial class FrmMain : Form, ILogger 
    {
        //EventLogger _el;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var thread = new Thread(Convert);
            thread.Start();
        }

        private void Convert() => Converter.Converter.Convert(new DebugLogger());

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
            var maxLineCount = 100;
            if (lstLog.Items.Count < maxLineCount + 10)
                return;
            for (var i = 0; i < 5; i++)
            {
                lstLog.Items.RemoveAt(0);
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //_el.DetachEvents();
        }
    }
}