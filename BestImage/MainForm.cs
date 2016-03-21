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
using Tesseract;

namespace BestImage
{
    public partial class MainForm : Form
    {
        private FolderBrowserDialog folderBrowserDialog;
        private int heightRef;
        private int widthRef;

        public MainForm()
        {
            InitializeComponent();
            this.folderBrowserDialog = new FolderBrowserDialog();

            string prefPath;

            PreferencesSaver.loadPreferences(out prefPath, out this.heightRef, out this.widthRef);

            this.textBox1.Text = prefPath;
            this.textBox2.Text = this.heightRef.ToString();
            this.textBox3.Text = this.widthRef.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = textBox1.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (checkAndSetArguments())
            {
                FileInfo bestImg = new ImageFinder(new DirectoryInfo(textBox1.Text),
                    heightRef * widthRef,
                    ((double)widthRef) / ((double)heightRef))
                    .bestImage();

                if (bestImg != null)
                {
                    Image img = Image.FromFile(bestImg.FullName);
                    Scew scew;
                    Pix.LoadFromFile(bestImg.FullName).Deskew(out scew);

                    MessageBox.Show("The image with better size is " +
                                    bestImg.FullName + ".\n\n" + "Resolution: " + img.Width + "x" +
                                    img.Height + "; skew angle: " + scew.Angle + ".", "Result",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("No image found", "Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                PreferencesSaver.savePreferences(textBox1.Text, heightRef, widthRef);
            }
            else
                MessageBox.Show("Incorrect arguments", "Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

            Cursor.Current = Cursors.Default;
        }

        private bool checkAndSetArguments()
        {
            if (!new DirectoryInfo(textBox1.Text).Exists)
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
}