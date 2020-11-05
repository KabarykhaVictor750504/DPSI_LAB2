using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COSI2
{
    public partial class Form1 : Form
    {
        private string imagePath;

        private readonly ImageProcessor imageProcessor;

        private Bitmap image;
        public Form1()
        {
            InitializeComponent();

            this.imageProcessor = new ImageProcessor();
        }

        private void button1_click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "png files (*.png)|*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.imagePath = openFileDialog.FileName;
                    if (this.imagePath != null && this.imagePath != string.Empty)
                    {
                        this.imagePathBox.Text = this.imagePath;
                        this.LoadImage();
                    }
                }
            }
        }

        private async void button2_click(object sender, EventArgs e)
        {
            var picture = imageProcessor.ToGrayScale(image);
            this.GrayScale.Image = this.imageProcessor.Resize(picture, this.GrayScale.Width, this.GrayScale.Height);
            var pictureBinary = imageProcessor.ToBinary(picture);
            this.BinaryScale.Image = this.imageProcessor.Resize(pictureBinary, this.BinaryScale.Width, this.BinaryScale.Height);
            var result = imageProcessor.FirePicture(pictureBinary);
            //this.matrix.Text = imageProcessor.ToStringLabel(result.Item1, pictureBinary.Width, pictureBinary.Height);
            var squares = imageProcessor.Square(result.Item1,result.Item2, pictureBinary.Width, pictureBinary.Height);
            var perimetrs = imageProcessor.Perimetr(result.Item1,result.Item2, pictureBinary.Width, pictureBinary.Height);
            var compacts = imageProcessor.Compact(perimetrs,squares);
            Dictionary<int, (int, int, float)>  clastersVector = this.imageProcessor.ToDictionary(perimetrs,squares,compacts,result.Item2);
            Dictionary<int, List<int>> resultClastirize;
            imageProcessor.Clasterize(clastersVector, 2,out resultClastirize);
            this.claster.Image = this.imageProcessor.Resize(this.imageProcessor.ToPrintClasterize(result.Item1, pictureBinary, resultClastirize), this.BinaryScale.Width, this.BinaryScale.Height);
            //this.BinaryScale.Image = this.imageProcessor.Resize(imageProcessor.ToArea(result.Item1, pictureBinary.Width, pictureBinary.Height, this.BinaryScale.Width, this.BinaryScale.Height);
        }


        private void LoadImage()
        {
            image = new Bitmap(this.imagePath);
            this.originalPicture.Image = this.imageProcessor.Resize(image, this.originalPicture.Width, this.originalPicture.Height);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
