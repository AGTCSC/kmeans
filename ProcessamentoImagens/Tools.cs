using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessamentoImagens
{
    class Tools
    {
        public static unsafe byte* getPixel(Bitmap img, BitmapData bitmapDataSrc, int x, int y)
        {
            if (x >= 0 && y >= 0 && x < img.Width && y < img.Height)
            {
                byte* srcB = (byte*)bitmapDataSrc.Scan0.ToPointer();
                srcB = (byte*)((int)srcB + y * img.Width * 3 + y * (bitmapDataSrc.Stride - (img.Width * 3)) + (x * 3));  //(deslocamento vertical) * (padding) + (deslocmento horizonta)
                return srcB;
            }
            else
                return null;
        }
    }
}
