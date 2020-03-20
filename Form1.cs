using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Data.Sql;
using System.Data.SqlClient;

namespace last_fucking_try
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private FilterInfoCollection CaptureDevices;
        private VideoCaptureDevice videoSource;

        private void Form1_Load(object sender, EventArgs e)
        {
            CaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevices)
            {
                comboBox1.Items.Add(Device.Name);
            }
            comboBox1.SelectedIndex = 0;
            videoSource = new VideoCaptureDevice();

            string connectionString;
            SqlConnection con;
            connectionString = @"Data Source=CND8263QPP\SEKHARSQL; Initial Catalog=Db;User ID=sa;Password=KlassLyx;";
            con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand command;
            SqlDataReader dataReader;
            string sql, output = "";

            sql = "SELECT Text FROM Db";
            command = new SqlCommand(sql, con);
            dataReader = command.ExecuteReader();

            dataReader.Read();
            button1.Text = "Klicka inte här annars....... får du fangelse för inträde i SÄPOs databas.....";
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            videoSource = new VideoCaptureDevice(CaptureDevices[comboBox1.SelectedIndex].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(VideoSource_NewFrame);
            videoSource.Start();
            
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            videoSource.Stop();
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
            pictureBox2.Image = null;
            pictureBox2.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            videoSource.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (videoSource.IsRunning == true)
            {
                videoSource.Stop();
            }
            Application.Exit(null);
        }
    }
}
