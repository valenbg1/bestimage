using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Tesseract;

namespace BestImage
{
    public partial class ResultDialog : Form
    {
        public ResultDialog(FileInfo fimg)
        {
            InitializeComponent();

            Image img = Image.FromFile(fimg.FullName);
            Scew scew;
            Pix.LoadFromFile(fimg.FullName).Deskew(out scew);

            pictureBox1.Image = img;
            textBox1.Text = "The best image found is:\r\n" +
                            fimg.FullName + ".\r\n" + "Resolution: " + img.Width + "x" +
                            img.Height + "; skew angle: " + scew.Angle + ".";
        }
    }
}
