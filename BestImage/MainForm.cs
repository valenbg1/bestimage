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
            this.numericUpDown1.Text = this.heightRef.ToString();
            this.numericUpDown2.Text = this.widthRef.ToString();
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
            this.Enabled = false;

            if (checkAndSetArguments())
            {
                FileInfo bestImg = new ImageFinder(this,
                    new DirectoryInfo(textBox1.Text),
                    heightRef * widthRef,
                    ((double)widthRef) / ((double)heightRef))
                    .bestImage();

                progressBar1.Value = progressBar1.Maximum;

                if (bestImg != null)
                    new ResultDialog(bestImg).ShowDialog();
                else
                    MessageBox.Show("No image found", "Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                PreferencesSaver.savePreferences(textBox1.Text, heightRef, widthRef);
            }
            else
                MessageBox.Show("Incorrect arguments", "Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

            GC.Collect();
            progressBar1.Value = 0;
            Cursor.Current = Cursors.Default;
            this.Enabled = true;
        }

        private bool checkAndSetArguments()
        {
            if (!new DirectoryInfo(textBox1.Text).Exists)
                return false;

            try
            {
                heightRef = Int32.Parse(numericUpDown1.Text);
            }
            catch
            {
                return false;
            }

            try
            {
                widthRef = Int32.Parse(numericUpDown2.Text);
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

        public void incProgressBar()
        {
            progressBar1.Increment(1);
        }

        public void setProgressBarMaxValue(int maxValue)
        {
            progressBar1.Maximum = maxValue;
        }
    }
}