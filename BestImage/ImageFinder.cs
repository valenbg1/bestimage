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
        public const double area_threshold = 0.05;

        private MainForm mform;

        private DirectoryInfo dir;
        private int imgAreaRef;
        private double imgRatioRef;

        private FileInfo bestImg;
        private int bestArea;
        private double bestRatio;
        private float bestSkew;

        public ImageFinder(MainForm mform, DirectoryInfo dir, int imgAreaRef, double imgRatioRef)
        {
            this.mform = mform;

            this.dir = dir;
            this.imgAreaRef = imgAreaRef;
            this.imgRatioRef = imgRatioRef;

            this.bestArea = 0;
            this.bestRatio = 0;
            this.bestSkew = float.MaxValue;
        }

        public FileInfo bestImage()
        {
            FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);
            mform.setProgressBarMaxValue(files.Length);
            int i = 0;

            foreach (FileInfo file in files)
            {
                if (i++ % 10 == 0)
                    GC.Collect();

                mform.incProgressBar();

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

                Scew scew;
                Pix.LoadFromFile(file.FullName).Deskew(out scew);

                if ((Math.Abs(scew.Angle) <= Math.Abs(bestSkew)) &&
                    ((((double)Math.Abs(imgAreaRef - imgArea)) * (1.0 - area_threshold)) < ((double)Math.Abs(imgAreaRef - bestArea))) &&
                    (Math.Abs(imgRatioRef - imgRatio) < Math.Abs(imgRatioRef - bestRatio)))
                {
                    bestArea = imgArea;
                    bestRatio = imgRatio;
                    bestSkew = scew.Angle;
                    bestImg = file;
                }
            }


            return bestImg;
        }
    }
}