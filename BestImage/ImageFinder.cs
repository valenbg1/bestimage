using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Tesseract;

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

        public void bestImage(object sender, DoWorkEventArgs e)
        {
            FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);
            mform.SetProgressBarMaxValue(files.Length);

            foreach (FileInfo file in files)
            {
                if (((BackgroundWorker)sender).CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                mform.IncProgressBar();

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
                Pix pix1 = Pix.LoadFromFile(file.FullName);
                Pix pix2 = pix1.Deskew(out scew);

                if ((Math.Abs(scew.Angle) <= Math.Abs(bestSkew)) &&
                    ((((double)Math.Abs(imgAreaRef - imgArea)) * (1.0 - area_threshold)) < ((double)Math.Abs(imgAreaRef - bestArea))) &&
                    (Math.Abs(imgRatioRef - imgRatio) < Math.Abs(imgRatioRef - bestRatio)))
                {
                    bestArea = imgArea;
                    bestRatio = imgRatio;
                    bestSkew = scew.Angle;
                    bestImg = file;
                }

                img.Dispose();
                pix1.Dispose();
                pix2.Dispose();
            }

            e.Result = bestImg;
        }
    }
}