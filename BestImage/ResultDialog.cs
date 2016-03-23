using System;
using System.ComponentModel;
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
            this.UseWaitCursor = true;
            pictureBox1.Enabled = false;

            BackgroundWorker bgw = new BackgroundWorker();

            bgw.DoWork += new DoWorkEventHandler(
                delegate (object se, DoWorkEventArgs ev)
                {
                    Process expl = new Process();
                    expl.StartInfo = new ProcessStartInfo("explorer", "/select, " + fimg.FullName);
                    expl.Start();
                    expl.WaitForInputIdle();
                });

            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object se, RunWorkerCompletedEventArgs ev)
                {
                    this.UseWaitCursor = false;
                    pictureBox1.Enabled = true;
                });

            bgw.RunWorkerAsync();
        }
    }
}