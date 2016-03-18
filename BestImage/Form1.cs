using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BestImage
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog folderBrowserDialog;
        private int heightRef;
        private int widthRef;

        public Form1()
        {
            InitializeComponent();
            this.folderBrowserDialog = new FolderBrowserDialog();
            this.heightRef = 0;
            this.widthRef = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkAndSetArguments())
            {
                FileInfo bestImg = new ImageFinder(new DirectoryInfo(textBox1.Text),
                    heightRef * widthRef,
                    ((double)widthRef) / ((double)heightRef))
                    .bestImage();

                if (bestImg != null)
                    MessageBox.Show("The image with better size is " + bestImg.FullName, "Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("No image found", "Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                MessageBox.Show("Incorrect arguments", "Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool checkAndSetArguments()
        {
            if (textBox1.Text != "")
            {
                try
                {
                    new DirectoryInfo(textBox1.Text);
                } catch
                {
                    return false;
                }
            } else
                return false;

            try
            {
                heightRef = UInt16.Parse(textBox2.Text);
            }
            catch
            {
                return false;
            }

            try
            {
                widthRef = UInt16.Parse(textBox3.Text);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog();
        }
    }

    public class ImageFinder
    {
        public const double ratio_threshold = 0.05;

        private DirectoryInfo dir;
        private int imgAreaRef;
        private double imgRatioRef;

        private FileInfo bestImg;
        private int bestArea;
        private double bestRatio;

        public ImageFinder(DirectoryInfo dir, int imgAreaRef, double imgRatioRef)
        {
            this.dir = dir;
            this.imgAreaRef = imgAreaRef;
            this.imgRatioRef = imgRatioRef;

            this.bestArea = 0;
            this.bestRatio = 0;
        }

        private void setBestImageInDir(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                Image img;

                try
                {
                    img = Image.FromFile(file.FullName);
                }
                catch
                {
                    continue;
                }

                int imgArea = img.Height * img.Width;
                double imgRatio = ((double)img.Width) / ((double)img.Height);

                if ((Math.Abs(imgRatioRef - imgRatio) <= Math.Abs(imgRatioRef - bestRatio)) &&
                    ((((double)Math.Abs(imgAreaRef - imgArea)) * (1.0 - ratio_threshold)) <= ((double)Math.Abs(imgAreaRef - bestArea))))
                {
                    bestArea = imgArea;
                    bestRatio = imgRatio;
                    bestImg = file;
                }
            }
        }

        public FileInfo bestImage()
        {
            setBestImageInDir(dir);

            foreach (DirectoryInfo childDir in dir.EnumerateDirectories())
                setBestImageInDir(childDir);

            return bestImg;
        }
    }
}