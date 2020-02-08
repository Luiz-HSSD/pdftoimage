using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pdftoimage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int largura, altura;
            if (!int.TryParse(textBox1.Text, out largura))
            {
                MessageBox.Show("largura errada", "pdf para imagem - Luiz-HSSD");
                return;
            }
            if (!int.TryParse(textBox2.Text, out altura))
            {
                MessageBox.Show("altura errada", "pdf para imagem - Luiz-HSSD");
                return;
            }
            var dev = new OpenFileDialog();
            dev.Filter = "arquivos pdf|*.pdf";
            dev.ShowDialog();
            string pdf_filename = dev.FileName;
            string png_filename = "convertido-" + DateTime.Now.Ticks + ".png";
            
            if (!string.IsNullOrEmpty(dev.FileName) && int.TryParse(textBox1.Text,out largura)  )
            { 
            cs_pdf_to_image.Pdf2Image.PrintQuality = 100;
            //cs_pdf_to_image.Pdf2Image.
            List<string> errors = cs_pdf_to_image.Pdf2Image.Convert(pdf_filename, png_filename);
            Bitmap img = (Bitmap)Bitmap.FromFile(png_filename);
            img = ResizeImage(img, largura, altura);

            img.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\" + png_filename, ImageFormat.Png);
            MessageBox.Show("realizado com sucesso", "pdf para imagem - Luiz-HSSD");
            }
            else
            {
                MessageBox.Show("ponha o caminho do pdf", "pdf para imagem - Luiz-HSSD");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int largura, altura;
            if (!int.TryParse(textBox1.Text, out largura))
            {
                MessageBox.Show("largura errada", "redimensionar imagem - Luiz-HSSD");
                return;
            }
            if (!int.TryParse(textBox2.Text, out altura))
            {
                MessageBox.Show("altura errada", "redimensionar imagem - Luiz-HSSD");
                return;
            }
            var dev = new OpenFileDialog();
            dev.Filter = "arquivos imagem|*.png;*.jpg";
            dev.ShowDialog();
            string png_filename = "redimensionado-"+DateTime.Now.Ticks+".png";
            if (!string.IsNullOrEmpty(dev.FileName) && int.TryParse(textBox1.Text, out largura))
            {
                Bitmap img = (Bitmap)Bitmap.FromFile(dev.FileName);

                img = ResizeImage(img, int.Parse(textBox1.Text), int.Parse(textBox2.Text));

                img.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\" + png_filename, ImageFormat.Png);
                MessageBox.Show("realizado com sucesso", "redimensionar imagem - Luiz-HSSD");
            }
            else
            {
                MessageBox.Show("ponha o caminho do imagem", "redimensionar imagem - Luiz-HSSD");
            }
        }
    }
}
