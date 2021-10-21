using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace SimpleMessenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPAddress serverIp = IPAddress.Parse("127.0.0.1");
        private int serverPort = 3333;
        private Socket clientSocket;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            try
            {
                clientSocket.Connect(new IPEndPoint(serverIp, serverPort));

                if (clientSocket.Connected)
                    btnConnect.IsEnabled = false;

                Task.Run(() =>
                {
                    while (clientSocket.Connected)
                    {
                        byte[] buffer = new byte[1024000];
                        int length = clientSocket.Receive(buffer);
                        Dispatcher.Invoke(() => { txtBlock.Text += Encoding.UTF8.GetString(buffer, 0, length) + "\n"; });
                    }
                });

            }
            catch
            {
                clientSocket.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ((clientSocket?.Connected).GetValueOrDefault())
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(inputBox.Text));
                inputBox.Clear();
            }
        }
    }
}
