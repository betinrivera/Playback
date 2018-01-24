using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Windows.Threading;

namespace Playback
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Mp3FileReader reader;
        private WaveOut output;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += OnTimerTick;

            sldPosition.DragLeave += sldPosition_DragCompleted;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (reader != null)
            {
                lblPosition.Text = reader.CurrentTime.ToString();
                sldPosition.Value = reader.CurrentTime.TotalSeconds;
            }
        }
        private void btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog()== true)
            {
                txtRuta.Text = openFileDialog.FileName;
            }
        }

        private void btn_Play_Click(object sender, RoutedEventArgs e)
        {
            if (txtRuta.Text != null && txtRuta.Text != "")
            {
                output = new WaveOut();
                output.PlaybackStopped += OnPlaybackStop;
                reader = new Mp3FileReader(txtRuta.Text);
                output.Init(reader);
                output.Play();
                btn_Stop.IsEnabled = true;
                btn_Play.IsEnabled = false;
                lblDuration.Text = reader.TotalTime.ToString();
                lblPosition.Text = reader.CurrentTime.ToString();
                sldPosition.Maximum = reader.TotalTime.TotalSeconds;
                sldPosition.Value = 0;
                timer.Start();

            }
            else
            {
                //Avisarle al usuario que elija un archivo
            }
        }
        private void OnPlaybackStop(object sender, StoppedEventArgs e)
        {
            reader.Dispose();
            output.Dispose();
            timer.Stop();
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (output != null)
            {
                output.Stop();
                btn_Play.IsEnabled = true;
                btn_Stop.IsEnabled = false;
            }
        }

        private void sldPosition_DragCompleted(object sender, RoutedEventArgs e)
        {
            if (reader != null)
            {
                reader.CurrentTime = TimeSpan.FromSeconds(sldPosition.Value);
            }
        }

    }
}