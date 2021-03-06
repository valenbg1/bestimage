﻿using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BestImage
{
    public partial class MainForm : Form
    {
        delegate void SetProgressBarMaxValueCallback(int maxValue);
        delegate void IncProgressBarCallback();
        private BackgroundWorker bgw;

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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutDialog().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = textBox1.Text;

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CheckAndSetArguments())
            {
                this.UseWaitCursor = true;
                button2.Enabled = false;

                ImageFinder imgFinder = new ImageFinder(this,
                    new DirectoryInfo(textBox1.Text),
                    heightRef * widthRef,
                    ((double)widthRef) / ((double)heightRef));

                bgw = new BackgroundWorker();
                bgw.DoWork += new DoWorkEventHandler(imgFinder.bestImage);
                bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ShowResult);
                bgw.RunWorkerAsync();

                PreferencesSaver.savePreferences(textBox1.Text, heightRef, widthRef);
            }
            else
                MessageBox.Show("Incorrect arguments", "Result",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool CheckAndSetArguments()
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (bgw != null)
            //    bgw.CancelAsync();
        }

        public void IncProgressBar()
        {
            if (progressBar1.InvokeRequired)
                Invoke(new IncProgressBarCallback(IncProgressBar));
            else
                progressBar1.Increment(1);
        }

        public void SetProgressBarMaxValue(int maxValue)
        {
            if (progressBar1.InvokeRequired)
            {
                Invoke(new SetProgressBarMaxValueCallback(SetProgressBarMaxValue),
                        new object[] { maxValue });
            }
            else
                progressBar1.Maximum = maxValue;
        }

        private void ShowResult(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                return;

            progressBar1.Value = progressBar1.Maximum;

            if (e.Result != null)
                new ResultDialog((FileInfo)e.Result).ShowDialog();
            else
                MessageBox.Show("No image found", "Result",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            GC.Collect();
            progressBar1.Value = 0;
            this.UseWaitCursor = false;
            button2.Enabled = true;
        }
    }
}