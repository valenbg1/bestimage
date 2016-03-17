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

namespace BestImage
{
    public partial class Form1 : Form
    {
        private FolderBrowserDialog folderBrowserDialog1;

        public Form1()
        {
            InitializeComponent();
            this.folderBrowserDialog1 = new FolderBrowserDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
