using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection _connection;

        public MainWindow()
        {
            InitializeComponent();
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44394/TestHub")
                .Build();
            _connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };
        }

        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            
            _connection.On<string>("Connected",
                                   (connectionid) =>
            {
                //MessageBox.Show(connectionid);
                tbMain.Text = connectionid;
            });

            _connection.On<string>("Posted",
                                   (value) =>
            {
                Dispatcher.BeginInvoke((Action)(() =>

                {
                    messagesList.Items.Add(value);
                }));
            });
            try
            {
                await _connection.StartAsync();
                messagesList.Items.Add("Connection started");
                btnConnect.IsEnabled = false;
                //sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }
    }
}