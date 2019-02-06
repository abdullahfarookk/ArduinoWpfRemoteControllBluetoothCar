
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using BluetoothSample.Shared.Model;
using FirstFloor.ModernUI.Presentation;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Text;
using System.IO;

namespace BluetoothSample
{
    public partial class MainWindow
    {

        
        BluetoothClient bluetoothClient;
        private NetworkStream _stream;
        private StreamWriter _sender;
        private int _counter=1;
        private NetworkStream _detechedStream;
        private bool flag ;

        public override async void BeginInit()
        {

            base.BeginInit();
          
        }

        private async void InitilizeBluetooth()
        {
            bluetoothClient = new BluetoothClient();
            Devices.ItemsSource = new List<string> { "Searching" };
           var items = await GetBluetoothDevices();
            Devices.ItemsSource = items;
            

        }



        public MainWindow()
        {
            InitializeComponent();
            AppearanceManager.Current.AccentColor = Colors.DarkTurquoise;
            InitilizeBluetooth();

            //   Devices.ItemsSource = new List<string>() { "Searching..." };
            //DispatcherTimer dt = new DispatcherTimer();
            //dt.Tick += Read_Bluetooth;
            //dt.Interval = new TimeSpan(0, 0, 1); // execute every hour
            //dt.Start();

            //   Read_Bluetooth();
        }    
        private async Task<IList<Device>> GetBluetoothDevices()
        {
            // for not block the UI it will run in a different threat
          return await Task.Run(() =>
            {
                var devices = new List<Device>();
                using (var bluetoothClient = new BluetoothClient())
                {
                    var array = bluetoothClient.DiscoverDevices();
                    var count = array.Length;
                    for (var i = 0; i < count; i++)
                    {
                        devices.Add(new Device(array[i]));
                    }
                }
                return devices;
            });
            
        }

        //private void Read_Bluetooth(object sender, EventArgs e)
        private void Read_Bluetooth()
        {
            if (_stream == null)
            {
                return;
            }
            var data = new byte[100];
            Task.Run(() =>
            {
                while (true)
                {
                    if (flag==false)
                    {
                        return;
                    }       
                    try
                    {
                        using (var receiver = new StreamReader(_stream))
                        {
                            var stri = receiver.ReadLine();

                            //`  _outpuStream.Flush();

                            // var stri2 = System.Text.Encoding.UTF8.GetString(data);
                            // var map = 5 / 2.5 *Int32.Parse( stri);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                //        DistanceText.Text = data[0].ToString()+"c";
                                //        if ( data[0]<=30)
                                //        {
                                //            DistanceBar.Foreground = Brushes.Red;
                                //        }
                                //        else
                                //        {
                                //            DistanceBar.Foreground = Brushes.YellowGreen;
                                //        }
                                //        DistanceBar.Value =map;


                                ResultValue.Text += stri;

                            });
                        }
                       
                            
                        
                        

                        _counter++;
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ResultValue.Text = "Not Received Any Data";
                        });

                    }

                }


            });
            // code to execute periodically


        }


            private async void Up_button(object sender, System.Windows.RoutedEventArgs e)
            {
                try
                {
                    //  var stream = _client.GetStream();
                    if (_stream == null)
                    {
                        Trace.WriteLine(" OutputStream not Initilized");
                        return;
                    }
                    await Task.Run(() =>
                    {
                        //using (var _sender = new StreamWriter(_stream))
                        //{
                        //}
                            _sender.Write("F");
                            _sender.Flush();
                    
                        //var buffer = System.Text.Encoding.UTF8.GetBytes("F");
                        //_stream.Write(buffer, 0, buffer.Length);
                        //_stream.Flush();

                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }




            }

        private async void Left_button(object sender, RoutedEventArgs e)
        {
            try
            {

                if (_stream == null)
                {
                    Trace.WriteLine(" OutputStream not Initilized");
                    return;
                }
                await Task.Run(() =>
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes("L");
                    _stream.Write(buffer, 0, buffer.Length);
                    _stream.Flush();
                    // _outpuStream.Close();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Right_button(object sender, RoutedEventArgs e)
        {
            try
            {
                //  var stream = _client.GetStream();
                if (_stream == null)
                {
                    Trace.WriteLine(" OutputStream not Initilized");
                    return;
                }
                await Task.Run(() =>
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes("R");
                    _stream.Write(buffer, 0, buffer.Length);
                    _stream.Flush();
                    // _outpuStream.Close();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Down_button(object sender, RoutedEventArgs e)
        {
            try
            {
                //  var stream = _client.GetStream();
                if (_stream == null)
                {
                    Trace.WriteLine(" OutputStream not Initilized");
                    return;
                }
                await Task.Run(() =>
                {
                    var buffer = System.Text.Encoding.UTF8.GetBytes("B");
                    _stream.Write(buffer, 0, buffer.Length);
                    _stream.Flush();
                    // _outpuStream.Close();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void Connect_Clicked(object sender, RoutedEventArgs e)
        {
            flag = false;
            Status.Fill = Brushes.Red;
            var device = Devices.SelectedItem as Device;
            if (device == null)
            {
                return;
            }
             bluetoothClient = new BluetoothClient();
            var ep = new BluetoothEndPoint(device.DeviceInfo.DeviceAddress,
                BluetoothService.SerialPort);

            await Task.Run(() =>
            {
                try
                {
                    bluetoothClient.Connect(ep);


                    // if all is ok to send
                    if (bluetoothClient.Connected)
                    {
                        _stream = bluetoothClient.GetStream();
                        _sender = new StreamWriter(_stream);
                        
                        Application.Current.Dispatcher.Invoke(() =>
                        {

                            Status.Fill = Brushes.GreenYellow;
                        });
                        Read_Bluetooth();
                        flag = true;
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Device Respond ni kr ri, ya pir Serial Communication Service Device pr available ni ha "+ex.Message);
                }
            });

        }
    }
}
