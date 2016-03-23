using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tesseract;

namespace BestImage
{
    public partial class ResultDialog : Form
    {
        FileInfo fimg;

        public ResultDialog(FileInfo fimg)
        {
            InitializeComponent();

            this.fimg = fimg;

            Image img = Image.FromFile(fimg.FullName);
            Scew scew;
            Pix.LoadFromFile(fimg.FullName).Deskew(out scew);

            pictureBox1.Image = img;

            textBox1.Text = "The best image found is:\r\n" +
                            fimg.FullName + ".\r\n" + "Resolution: " + img.Width + "x" +
                            img.Height + "; skew angle: " + scew.Angle + ".";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Enabled = false;

            Process expl = new Process();
            expl.StartInfo = new ProcessStartInfo("explorer", "/select, " + fimg.FullName);
            expl.Start();
            expl.WaitForInputIdle();

            Cursor.Current = Cursors.Default;
            this.Enabled = true;
        }
    }
}