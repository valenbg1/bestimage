using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BestImage
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
            e.Link.Visited = true;
        }

        private void AboutDialog_Load(object sender, EventArgs e)
        {
            linkLabel1.Links.Add(0, 5, "http://www.gnu.org/licenses/gpl-3.0.txt");
            linkLabel2.Links.Add(0, 6, "http://github.com/valenbg1/bestimage");
            linkLabel3.Links.Add(0, 7, "http://www.minsait.com/");
            linkLabel3.Links.Add(10, 5, "http://www.indracompany.com/");
            linkLabel4.Links.Add(104, 8, "http://github.com/charlesw");
            linkLabel4.Links.Add(115, 30, "http://github.com/charlesw/tesseract");
        }
    }
}
