using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageProcessor
{
    public partial class Form1 : Form
    {
        Color pixel;
        Bitmap loaded;
        Bitmap processed;
        int count = 0;
        bool _isGray = false;

        public Form1()
        {
            InitializeComponent();

            this.Width = 600;
            this.Height = 400;

        }

        private void initialize(int x, int y, Color pixel)
        {
            processed.SetPixel(x, y, pixel);
        }


        private void initializeGrayScale(int x, int y, int grayScale)
        {
            processed.SetPixel(x, y, Color.FromArgb(grayScale, grayScale, grayScale));
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.ShowDialog();

            String file_name = openDialog.FileName;
            loaded = (Bitmap)Image.FromFile(file_name);

            pictureBox1.Image = loaded;
        }

        private void cloneToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                    initialize(x, y, pixel);
                }
            }
            pictureBox2.Image = processed;
        }

        private void invertToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            if (count == 0)
            {
                processed = new Bitmap(loaded.Width, loaded.Height);

                for (int x = 0; x < loaded.Width; x++)
                {
                    for (int y = 0; y < loaded.Height; y++)
                    {
                        pixel = loaded.GetPixel(x, y);
                        initialize(loaded.Width - x - 1, y, pixel);
                    }
                }
                count++;
            }
            else
            {
                processed = new Bitmap(loaded.Width, loaded.Height);

                for (int x = 0; x < loaded.Width; x++)
                {
                    for (int y = 0; y < loaded.Height; y++)
                    {
                        pixel = loaded.GetPixel(x, y);
                        initialize(x, y, pixel);
                    }
                }
                count = 0;
            }
            pictureBox2.Image = processed;
        }

        private void grayscaleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            if (!_isGray)
            {
                for (int x = 0; x < loaded.Width; x++)
                {
                    for (int y = 0; y < loaded.Height; y++)
                    {
                        pixel = loaded.GetPixel(x, y);
                        int grayScale = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                        initializeGrayScale(x, y, grayScale);
                    }
                }
                _isGray = true;
            }
            else
            {
                for (int x = 0; x < loaded.Width; x++)
                {
                    for (int y = 0; y < loaded.Height; y++)
                    {
                        pixel = loaded.GetPixel(x, y);
                        initialize(x, y, pixel);
                    }
                }
                _isGray = false;
            }
            pictureBox2.Image = processed;
        }
    }
}
