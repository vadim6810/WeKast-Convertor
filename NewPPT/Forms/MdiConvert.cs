using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WeCastConvertor.Forms
{
    public partial class MdiConvert : Form
    {
        private readonly LogoForm parentForm;

        public MdiConvert()
        {
            InitializeComponent();
        }

        public MdiConvert(LogoForm logoForm)
        {
            InitializeComponent();
            parentForm = logoForm;
        }

        private void MdiConvert_Load(object sender, EventArgs e)
        {
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, 0);
        }

        private void pctExit_Click(object sender, EventArgs e)
        {
            parentForm.Close();
        }

        private void pctLogo_Click(object sender, EventArgs e)
        {
            Close();
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

    }
}