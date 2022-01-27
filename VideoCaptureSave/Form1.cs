using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using DarrenLee.Media;


namespace VideoCaptureSave
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private FilterInfoCollection CaptureDevices;
        private VideoCaptureDevice VideoSource;

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Visible = false;
            imageNameTextBox.Visible = false;
            CaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevices)
            {
                comboBox1.Items.Add(Device.Name);
            }
            comboBox1.SelectedIndex = 0;
            VideoSource = new VideoCaptureDevice();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            VideoSource = new VideoCaptureDevice(CaptureDevices[comboBox1.SelectedIndex].MonikerString);
            VideoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
            VideoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            VideoSource.Stop();
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
            pictureBox2.Image = null;
            pictureBox2.Invalidate();
            label1.Text = string.Empty;
            label2.Visible = false;
            imageNameTextBox.Visible = false;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            VideoSource.Stop();

        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image!=null)
            {
                pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
                label1.Text = string.Empty;
                label2.Visible = true;
                imageNameTextBox.Visible = true;
                imageNameTextBox.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("No Image To Capture");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (VideoSource.IsRunning == true)
            {
                VideoSource.Stop();
            }
            Application.Exit(null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image!=null && imageNameTextBox.Text!=string.Empty)
            {
                pictureBox2.Image.Save(Path.Combine(@"C:\Users\HP Pavilion\source\repos\VideoCaptureSave\VideoCaptureSave\Images\", Path.GetFileName(imageNameTextBox.Text + ".jpeg")), ImageFormat.Jpeg);
                label1.Text = "Image " + imageNameTextBox.Text +" Saved Successfully";
                label2.Visible = true;
                imageNameTextBox.Visible = true;
            }
            else
            {
                MessageBox.Show("No Image To Save Or FileName Not Assigned");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (VideoSource.IsRunning == true)
            {
                VideoSource.Stop();
            }
            Application.Exit(null);
        }
    }
}
