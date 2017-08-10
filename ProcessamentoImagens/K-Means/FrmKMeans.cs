using ProcessamentoImagens.K_Means;
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
using System.Drawing.Imaging;

namespace ProcessamentoImagens
{
    public partial class FrmKMeans : Form
    {

        private Image image;
        public FrmKMeans(Bitmap image)
        {
            DialogResult result = System.Windows.Forms.DialogResult.OK;
            Boolean flag = true;
            InitializeComponent();
            this.image = image;
            while (this.image == null && result == System.Windows.Forms.DialogResult.OK && flag)
            {
                result = MessageBox.Show("Abra uma imagem antes de continuar","Ocorreu um erro",MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (result == System.Windows.Forms.DialogResult.OK)
                    flag = abrir();
            }
            if (result == System.Windows.Forms.DialogResult.Cancel)
                Close();
            else
            {
                picImagem.Image = this.image;
                ShowDialog();
            }
        }

        private Boolean abrir()
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Arquivos de Imagem (*.jpg;*.gif;*.bmp;*.png)|*.jpg;*.gif;*.bmp;*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog1.FileName);
                picImagem.Image = image;
                picImagem.SizeMode = PictureBoxSizeMode.Normal;
                return true;
            }
            else
                return false;
        }
        public void kmeans(Bitmap imgSrc, Bitmap imgDst, int k)
        {
            //int k = combobox
            Cluster[] c = Cluster.gerarCluster(imgSrc, k);

            //lock dados bitmap destino
            BitmapData bitmapDataDst = imgDst.LockBits(new Rectangle(0, 0, imgDst.Width, imgDst.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* dst;
                
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < c[i].pontos.Count; j++)
                    {
                        dst = P1(imgDst, bitmapDataDst, c[i].pontos[j].X, c[i].pontos[j].Y);
                        *(dst++) = (byte)c[i].b;
                        *(dst++) = (byte)c[i].g;
                        *(dst++) = (byte)c[i].r;
                    }
                }
            }

            imgDst.UnlockBits(bitmapDataDst);
        }

        private void btnSegmentar_Click(object sender, EventArgs e)
        {
            Bitmap img = (Bitmap)picImagem.Image;
            Bitmap dst = new Bitmap(image);
            int k = (int) numK.Value;
            kmeans(img, dst, k);
            picImagem.Image = dst;
        }
    }
}
