using System;
using System.Drawing;
using System.Windows.Forms;
using HNUDIP;
using WebCamLib;

namespace ImageProcessor
{
    public partial class Form1 : Form   
    {
        Color pixel;
        Bitmap loaded;
        Bitmap processed;
        int count = 0;
        bool _isGray = false;
        bool _isSepia = false;
        bool _isInverted = false;

        public Form1()
        {
            InitializeComponent();

            this.Width = 1050;
            this.Height = 600;

            if (processed == null && loaded == null)
            {
                saveToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                cloneToolStripMenuItem.Enabled = false;
                grayscaleToolStripMenuItem.Enabled = false;
                testToolStripMenuItem.Enabled = false;
                histogramToolStripMenuItem.Enabled = false;
                sepiaToolStripMenuItem.Enabled = false;
            }

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

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                String file_name = openDialog.FileName;
                loaded = new Bitmap(file_name);

                pictureBox1.Image = loaded;
                loadedImage();

            }
        }

        private void loadedImage()
        {
            if(loaded != null)
            {
                saveToolStripMenuItem.Enabled = true;
                cloneToolStripMenuItem.Enabled = true;
                grayscaleToolStripMenuItem.Enabled = true; 
                testToolStripMenuItem.Enabled = true;
                histogramToolStripMenuItem.Enabled = true;
                sepiaToolStripMenuItem.Enabled = true;
            }
        }

        // BASIC COPY
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


        // GRAYSCALE
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
                        int grayScale = ((pixel.R + pixel.G + pixel.B) / 3);    
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

        // INVERSION
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            if (!_isInverted)
            {
                for (int x = 0; x < loaded.Width; x++)
                {
                    for (int y = 0; y < loaded.Height; y++)
                    {
                        pixel = loaded.GetPixel(x, y);
                        Color invertedColor = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                        processed.SetPixel(x, y, invertedColor);
                    }
                    _isInverted = true;
                }
            }
            else
            {
                for (int x = 0; x < loaded.Width; x++)
                {
                    for (int y = 0; y < loaded.Height; y++)
                    {
                        pixel = loaded.GetPixel(x, y);
                        processed.SetPixel(x, y, pixel);
                        initialize(x, y, pixel);
                    }
                }
                _isInverted = false;
            }
            pictureBox2.Image = processed;
        }

        // VERTICAL
        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
            {
                for(int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, loaded.Height - y - 1, pixel);
                }
            }
            pictureBox2.Image = processed;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        // Histogram
        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    int gray = (pixel.R + pixel.G + pixel.B) / 3;
                    processed.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
                
            Color sample;
            int[] histData = new int[256];

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    sample = loaded.GetPixel(x, y);
                    histData[sample.R] = histData[sample.R] + 1;
                }
            }
                

            // Size of my histogram
            Bitmap myData = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    myData.SetPixel(x, y, Color.White);
                }
            }
                
            for (int x = 0; x < loaded.Width; x++)
            {
                int temp = loaded.Height - 1;
                for (int y = 0; y < Math.Min(histData[x] / 5, loaded.Height); y++)
                {
                    myData.SetPixel(x, temp - y, Color.Black);
                }
            }
                
            pictureBox2.Image = myData;
        }


        // Save Image
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (processed != null)
            {
                if(saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    processed.Save(saveFileDialog.FileName);
                }
                return;
            }
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                loaded.Save(saveFileDialog.FileName);
            }
        }

        // Sepia image

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            if(!_isSepia)
            {
                for (int x = 0; x < loaded.Width; x++)
                {
                    for (int y = 0; y < loaded.Height; y++)
                    {
                        Color pixel = loaded.GetPixel(x, y);

                        int sepiaR = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                        int sepiaG = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                        int sepiaB = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                        processed.SetPixel(x, y, Color.FromArgb(Math.Min(255, sepiaR), Math.Min(255, sepiaG), Math.Min(255, sepiaB)));
                    }
                }
                _isSepia = true;
            } else
            {
                for (int x = 0; x < loaded.Width; x++)
                {
                    for (int y = 0; y < loaded.Height; y++)
                    {
                        pixel = loaded.GetPixel(x, y);
                        processed.SetPixel(x, y, pixel);
                        initialize(x, y, pixel);
                    }
                }
                _isSepia = false;
            }
            pictureBox2.Image = processed;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        Bitmap imageA, imageB, imageC;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                String file_name = openDialog.FileName;
                imageA = new Bitmap(file_name);

                pictureBox1.Image = imageA;
                loadedImage();

            }
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                String file_name = openDialog.FileName;
                imageB = new Bitmap(file_name);

                pictureBox2.Image = imageB;
                loadedImage();

            }
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {

            Color myGreen = Color.FromArgb(0, 255, 0);
            int greygreen = (myGreen.R + myGreen.G + myGreen.B) / 3;
            int threshold = 5;

            imageC = new Bitmap(imageB.Width, imageB.Width);

            for(int x = 0; x < imageA.Width; x++)
            {
                for(int y = 0; y < imageA.Height; y++)
                {
                    Color new_pixel = imageA.GetPixel(x, y);
                    Color backpixel = imageB.GetPixel(x, y);
                    int grey = (new_pixel.R + new_pixel.G + new_pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);

                    if (subtractvalue > threshold)
                        imageC.SetPixel(x, y, new_pixel);
                    else
                        imageC.SetPixel(x, y, backpixel);

                }
            }
            pictureBox3.Image = imageC;
        }

        Device[] devices = DeviceManager.GetAllDevices();
        Device cam = DeviceManager.GetDevice(0);
        Boolean isSubtract = false;

        private void timer1_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();

            int threshold = 100;

            if (data != null)
            {
                bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));

                // Check if the retrieved data is a valid image
                if (bmap != null)
                {
                    Bitmap b = new Bitmap(bmap);

                    ImageProcess2.BitmapFilter.Subtract(b, imageB, Color.Green, threshold);

                    pictureBox3.Image = b;
                }
                else
                {
                    // Handle case where clipboard data is not a valid image
                    Console.WriteLine("Clipboard data is not a valid image.");
                }
            }
            else
            {
                // Handle case where clipboard data is not available
                Console.WriteLine("Clipboard data is not available.");
            }
        }

        private void btnVidSubtract_Click(object sender, EventArgs e)
        {
            isSubtract = !isSubtract;
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));

            if (imageB != null && bmap.Size == imageB.Size)
            {
                timer1.Enabled = isSubtract;
            }
            else if(imageB == null)
            {
                Console.WriteLine("Background is null");
            }
            else
            {
                Console.WriteLine(imageB.Size);
                Console.WriteLine(imageB.Size); 
            }
        }

        private void btnCamera_Click(object sender, EventArgs e)
        {
            cam.ShowWindow(pictureBox1);
        }
    }
}