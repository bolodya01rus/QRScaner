using AForge.Video;
using AForge.Video.DirectShow;
using CommunityToolkit.Mvvm.ComponentModel;
using QRScaner.Behaviors;
using QRScaner.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
using ZXing.Windows.Compatibility;

namespace QRScaner.ViewModels;

public class MainViewModel : ObservableObject
{
   
            
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoDevice;
        DispatcherTimer timer = new();
        public ObservableCollection<string> CameraList { get; set; }

        private int cameraListSelectedIndex = 0;
        public int CameraListSelectedIndex { get => cameraListSelectedIndex; set => SetProperty(ref cameraListSelectedIndex, value); }

        public MainViewModel()
        {
            CameraList = new ObservableCollection<string>();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
                CameraList.Add(filterInfo.Name);
            videoDevice = new VideoCaptureDevice();


        }

        private void timerTick(object? sender, EventArgs e)
        {
            textOutput = string.Empty;
            if (ImageOutput != null)
            {

                var reader = new BarcodeReader();
                if (reader != null)
                {
                    var result = reader.Decode(ImageOutput);
                    if (result != null)
                    {
                        textOutput += result.Text;
                        timer.Stop();
                        videoDevice.SignalToStop();
                        OnPropertyChanged(nameof(textOutput));

                    }
                }
            }
        }
        private Bitmap _image;

        public Bitmap ImageOutput
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged("Image");
            }
        }
        private double _Height;
        public double Height
        {
            get { return _Height; }
            set
            {
                if (value == _Height) return;
                _Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private double _Width;
        public double Width
        {
            get { return _Width; }
            set
            {
                if (value == _Width) return;
                _Width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        private LambdaCommand scanQRCommand;
        public ICommand ScanQRCommand => scanQRCommand ??= new LambdaCommand(ScanQR);

        private void ScanQR(object commandParameter)
        {
            try
            {
                if (cameraListSelectedIndex >= 0 && !videoDevice.IsRunning)
                {
                    textOutput = string.Empty;
                    OnPropertyChanged(nameof(textOutput));
                    videoDevice = new VideoCaptureDevice(filterInfoCollection[cameraListSelectedIndex].MonikerString);
                    videoDevice.NewFrame += new NewFrameEventHandler(CaptureDevice_NewFrame);
                    videoDevice.Start();
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Проверьте подключение камеры"); ;
            }
       
        }
      
        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Image image = (Bitmap)eventArgs.Frame.Clone();

            int pictureBoxWidth = 640;
            int pictureBoxHeight = 480;

            int imageWidth = image.Width;
            int imageHeight = image.Height;

            if (imageWidth > pictureBoxWidth || imageHeight > pictureBoxHeight)
            {
                float aspectRatio = Math.Min((float)pictureBoxWidth / imageWidth, (float)pictureBoxHeight / imageHeight);
                int newWidth = (int)(imageWidth * aspectRatio);
                int newHeight = (int)(imageHeight * aspectRatio);
                Image scaledImage = image.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
                image.Dispose();
                image = scaledImage;
            }
            ImageOutput = (Bitmap)image;
            OnPropertyChanged(nameof(ImageOutput));

        }

        private LambdaCommand openFileCommand;
        public ICommand OpenFileCommand => openFileCommand ??= new LambdaCommand(OpenFile);


        private void OpenFile(object commandParameter)
        {
            videoDevice.SignalToStop();
            textOutput = string.Empty;
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {

                openFileDialog.Filter = "Image Files(*.PNG;*.BMP;*.JPG;*.GIF)|*.PNG;*.BMP;*.JPG;*.GIF";

                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        filePath = openFileDialog.FileName;

                        var barcodeReader = new BarcodeReader
                        {
                            AutoRotate = true,
                            Options = new DecodingOptions { TryHarder = true }
                        };
                        Bitmap bitmap = new Bitmap(Image.FromFile(filePath));
                        var result = barcodeReader.Decode(bitmap);

                        ImageOutput = bitmap;
                        // OnPropertyChanged(nameof(ImageOutput));
                        if (result != null)
                        {
                            textOutput += result.Text;

                        }
                        else
                            textOutput = "QR код не распознан";

                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Неверный формат файла");
                    }
                }
                else
                {
                    ImageOutput = null;

                }
                OnPropertyChanged(nameof(ImageOutput));
                OnPropertyChanged(nameof(textOutput));
            }
        }


        private string textOutput;

        public string TextOutput { get => textOutput; set => SetProperty(ref textOutput, value); }

        private LambdaCommand cleanFormCommand;
        public ICommand CleanFormCommand => cleanFormCommand ??= new LambdaCommand(CleanForm);

        private void CleanForm(object commandParameter)
        {
            videoDevice.SignalToStop();
            ImageOutput = null;
            OnPropertyChanged(nameof(ImageOutput));
            textOutput = string.Empty;
            OnPropertyChanged(nameof(textOutput));
        }

        private LambdaCommand windowCloseCommand;
        public ICommand WindowCloseCommand => windowCloseCommand ??= new LambdaCommand(WindowClose);

        private void WindowClose(object commandParameter)
        {
            videoDevice.SignalToStop();
            textOutput = string.Empty;
            
        }
    }



