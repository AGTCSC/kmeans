using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ProcessamentoImagens
{
    public partial class frmPrincipal : Form
    {
        private Image image;
        private Bitmap imageBitmap;

        public frmPrincipal()
        {
            InitializeComponent();
        }
        
        private bool check() 
        {
            if (image != null)
                return true;

            MessageBox.Show("Abra uma imagem antes de utilizar um dos métodos.");
            return false;
        
        }
        private void btnOpenImage(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Arquivos de Imagem (*.jpg;*.gif;*.bmp;*.png)|*.jpg;*.gif;*.bmp;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog.FileName);
                pictBoxImg.Image = image;
                pictBoxImg.SizeMode = PictureBoxSizeMode.Normal;
            }

        }
        private void btnSaveImage(object sender, EventArgs e)
        {
            saveFileDialog.FileName = openFileDialog.FileName;
            saveFileDialog.Filter = "Image |*.jpg;*.png;*.bmp";
            ImageFormat format = ImageFormat.Png;

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string formatchosen = System.IO.Path.GetExtension(saveFileDialog.FileName);
                switch(formatchosen)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                pictBoxImg.Image.Save(saveFileDialog.FileName, format);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Software develop by Augusto Cesar\n\ngithub.com/agtcsc");
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            Bitmap img = (Bitmap)pictBoxImg.Image;
            Bitmap dst = new Bitmap(image);
            int k = (int)numK.Value;
            kmeans(img, dst, k);
            picImagem.Image = dst;
        }
    }
}
