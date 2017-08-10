using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessamentoImagens.K_Means
{
    public class Cluster
    {
        #region atributos
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
        public List<Point> pontos { get; set; }
        private int mediaR, mediaG, mediaB;
        #endregion



        public Cluster() { r = 0; g = 0; b = 0; pontos = new List<Point>(); }

        public static int euclidiana(Cluster a, Color b)
        {
            return Math.Abs((a.r - b.R) + (a.g-b.G) + (a.b - b.B));
        }

        public  static void gerarCentroide(int k, Cluster[] c)
        {
            Random rnd = new Random();
            for (int i = 0; i < k; i++)
            {
                c[i].r = rnd.Next(255); ;
                c[i].g = rnd.Next(255); ;
                c[i].b = rnd.Next(255); ;
            }
        }
        public static Cluster[] gerarCluster(Bitmap imgSrc, int k)
        {
            Cluster[] c = new Cluster[k];
            Boolean[] convergiu = new Boolean[k];
            int loop;
            int w = imgSrc.Width, h = imgSrc.Height;
            int padding; //distancia entre o pixel e o cluster
            int min, pos, dist, count = 0;
            Int32 r, g, b;
            Color px = Color.FromArgb(0,0,0);

            for (int i = 0; i < k; i++)
            {
                c[i] = new Cluster();
                convergiu[i] = false;
            }
            gerarCentroide(k, c);
            //lock dados bitmap origem 
            BitmapData bitmapDataSrc = imgSrc.LockBits(new Rectangle(0, 0, w, h),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            

            padding = bitmapDataSrc.Stride - (w * 3);

            unsafe
            {
                byte* src;
                do
                {
                    loop = 0;
                    src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                    for (int t = 0; t < k; t++)
                        c[t].pontos.Clear();

                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            b = *(src++);  
                            g = *(src++);
                            r = *(src++);
                            min = 999; pos = 0;


                            for (int i = 0; i < k; i++)
                            {
                                px = Color.FromArgb(r, g, b);

                                dist = euclidiana(c[i], px);
                                if (dist < min)
                                {
                                    min = dist;
                                    pos = i;
                                }
                            }
                            c[pos].pontos.Add(new Point(x, y));
                            c[pos].mediaR += px.R;
                            c[pos].mediaG += px.G;
                            c[pos].mediaB += px.B;

                        }

                        src += padding;
                    }
                   
                    for (int i = 0; i < k; i++)
                    {
                        if (c[i].pontos.Count > 0)
                        {
                            c[i].mediaR /= c[i].pontos.Count;
                            c[i].mediaG /= c[i].pontos.Count;
                            c[i].mediaB /= c[i].pontos.Count;
                            if (c[i].r == c[i].mediaR && c[i].g == c[i].mediaG && c[i].b == c[i].mediaB)
                                convergiu[i] = true;
                            else
                            {
                                c[i].b = c[i].mediaB;
                                c[i].g = c[i].mediaG;
                                c[i].r = c[i].mediaR;
                                loop++;
                            }
                        }
                    }
                } while (count++ < 100);
            
            }
            imgSrc.UnlockBits(bitmapDataSrc);
            return c;

        }
    }
}
