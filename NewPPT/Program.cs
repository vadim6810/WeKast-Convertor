using System;
using System.Drawing;
using System.Windows.Forms;
using WeCastConvertor.Forms;

namespace WeCastConvertor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogoForm());
            //Application.Run(new MainForm());
            //Application.Run(new LoginForm());
            //Application.Run(new Form1());
        }
    }
}
