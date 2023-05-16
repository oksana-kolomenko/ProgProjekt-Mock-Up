using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Video;
using AForge.Controls;
using AForge.Video.DirectShow;
using NAudio.CoreAudioApi;
using DirectShowLib;
//using VisioForge.Libs.DirectShowLib;
//using VisioForge.Shared.DirectShowLib;

namespace Mock_Up_PP
{
    //public class Devices
    //{
    //    internal void get_audio_devices()
    //    {
    //        MMDeviceEnumerator deviceManager = new MMDeviceEnumerator();
    //        var devices = deviceManager.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active); //zum array machen

    //        // Loop through the list of audio playback devices and print information about each one
    //        foreach (var device in devices)
    //        {
    //            Console.WriteLine("Device Name: {0}", device);
    //        }
    //    }
    //    internal void get_recording_devices()
    //    {
    //            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
    //        foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All))
    //        {
    //            Console.WriteLine("{0}, {1}", device.FriendlyName, device.State);
    //        }
    //    }
    //}

    public partial class NewWindow : Window
    {
        public NewWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }
    }
    public partial class MainWindow : Window
    {
        // Declare VideoCaptureDevice and VideoSourcePlayer instances    
        private VideoCaptureDevice _videoDevice;
        private VideoSourcePlayer _videoPlayer;
        private void ShowCameraOutput()
        {
        // Initialize video device with default camera
        _videoDevice = new VideoCaptureDevice();
        _videoDevice.VideoResolution = _videoDevice.VideoCapabilities[0];

        // Initialize video player control and start playing
        _videoPlayer = new VideoSourcePlayer();
        _videoPlayer.VideoSource = _videoDevice;
        _videoPlayer.Start();
        }
        private void SetComboBoxItem(ComboBox comboBox, List<string> items)
        {
            // loop through the list of audio playback devices and print information about each one
            comboBox.Items.Clear();
            foreach (var item in items)
            {
                comboBox.Items.Add(item);
            }
            comboBox.SelectedIndex = 0;  
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewWindow newWindow = new NewWindow(); // Create a new instance of the NewWindow class
            newWindow.Show(); // Show the new window
        }
        public MainWindow()
        {
            InitializeComponent();
            Uri iconUri = new Uri("D:/HTW Ingenieurinformatik/3 Semester/Programmierprojekt/icon.ico");
            this.Icon = BitmapFrame.Create(iconUri);

            //
            MMDeviceEnumerator devicemanager = new MMDeviceEnumerator();
            var devices = devicemanager.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active); //zum array machen
            List<string> speaker_devices = new List<string>();

            foreach (var device in devices)
            {
                speaker_devices.Add(device.ToString());
            }

            SetComboBoxItem(comboBox3, speaker_devices);

            //
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            List<string> microphone_devices = new List<string>();
            foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                microphone_devices.Add(device.ToString());
            }

            SetComboBoxItem(comboBox2, microphone_devices);

            //
            List<string> videoDevices = new List<string>();

            // Enumerate the video capture devices using DirectShowLib
            DsDevice[] video_devices = DsDevice.GetDevicesOfCat(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
            foreach (DsDevice device in video_devices)
            {
                videoDevices.Add(device.Name);
            }
            SetComboBoxItem(comboBox1, videoDevices);

        }

        private List<string> pictures = new List<string>()
        {
            "D:/HTW Ingenieurinformatik/3 Semester/Programmierprojekt/sit.jpg",
            "D:/HTW Ingenieurinformatik/3 Semester/Programmierprojekt/talking.jpg",
            "D:/HTW Ingenieurinformatik/3 Semester/Programmierprojekt/phone.jpg"
        };
        private int currentPictureIndex = 0;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPicture(currentPictureIndex);
        }
        private void LoadPicture(int index)
        {
            string path = pictures[index];
            BitmapImage image = new BitmapImage(new Uri(path));
            pictureBox.Source = image;

            if (index < 0 || index >= pictures.Count)
            {
                return;
            }

            if (index == 0)
            {
                int volume = 90;
                slider_volume.Value = volume;
                statusBar.Items.Clear();
                statusBar.Items.Add(new StatusBarItem { Content = $"Alone in the room, volume at {volume}%." });
                ShowCameraOutput();
            }
            if (index == 1)
            {
                int volume = 50;
                slider_volume.Value = volume;
                statusBar.Items.Clear();
                statusBar.Items.Add(new StatusBarItem { Content = $"Two people chatting, volume at {volume}%." });
            }
            if (index == 2)
            {
                int volume = 30;
                slider_volume.Value = volume;
                statusBar.Items.Clear();
                statusBar.Items.Add(new StatusBarItem { Content = $"Talking on phone, volume at {volume}%." });
            }

            //BitmapImage defaultImage = new BitmapImage(new Uri("D:/HTW Ingenieurinformatik/3 Semester/Programmierprojekt/sit.jpg", UriKind.Relative));
            //pictureBox.Source = defaultImage;
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            currentPictureIndex--;
            if (currentPictureIndex < 0)
            {
                currentPictureIndex = pictures.Count - 1;
            }
            LoadPicture(currentPictureIndex);
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            currentPictureIndex++;
            if (currentPictureIndex >= pictures.Count)
            {
                currentPictureIndex = 0;
            }
            LoadPicture(currentPictureIndex);
        }


    }
}
////pictureBox.Source = new BitmapImage(new Uri("D:/HTW Ingenieurinformatik/3 Semester/Programmierprojekt/Logo.jpg"));


//Access Camera
//InitializeComponent();
//VideoCaptureDevice videoSource = new VideoCaptureDevice();
//videoSource = new VideoCaptureDevice(FilterInfoCollection[0].MonikerString);
//videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
//videoSource.Start();

//VideoDrawing videoDrawing = new VideoDrawing();
//videoDrawing.Rect = new Rect(0, 0, 640, 480);
//videoDrawing.Player = new MediaFoundationPlayer(); // You can use any player here
//videoDrawing.Player.Open(new Uri("video.avi", UriKind.Relative));
//videoDrawing.Player.Play();

//DrawingBrush drawingBrush = new DrawingBrush(videoDrawing);
//drawingBrush.Stretch = Stretch.Uniform;

//Canvas canvas = new Canvas();
//canvas.Background = drawingBrush;

//this.Content = canvas;