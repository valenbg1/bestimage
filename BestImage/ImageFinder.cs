using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace BestImage
{
    public class ImageFinder
    {
        public const double ratio_threshold = 0.05;

        private DirectoryInfo dir;
        private int imgAreaRef;
        private double imgRatioRef;

        private FileInfo bestImg;
        private int bestArea;
        private double bestRatio;

        //private List<Scew> scewList;

        public ImageFinder(DirectoryInfo dir, int imgAreaRef, double imgRatioRef)
        {
            this.dir = dir;
            this.imgAreaRef = imgAreaRef;
            this.imgRatioRef = imgRatioRef;

            this.bestArea = 0;
            this.bestRatio = 0;

            //this.scewList = new List<Scew>(); 
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

                Scew scew;

                Pix imgP = Pix.LoadFromFile(file.FullName);
                imgP.Deskew(out scew);

                //scewList.Add(scew);
            }
        }

        public FileInfo bestImage()
        {
            setBestImageInDir(dir);

            foreach (DirectoryInfo childDir in dir.EnumerateDirectories())
                setBestImageInDir(childDir);

            imageSkew();

            return bestImg;
        }

        private void imageSkew()
        {
            //string scews = "Scew angles\tConfidences";
            //int i = 0;

            //foreach (Scew scew in scewList)
            //{
            //    scews += "\n" + scew.Angle + "\t\t" + scew.Confidence;

            //    if (++i % 3 == 0)
            //        scews += "\n";
            //}

            //MessageBox.Show(scews, "Scew angles", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
