using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChatClient.MVVM.ViewModel
{
    class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        public string userName { get; set; }

        public string message { get; set; }

        private Server server;
        public MainViewModel() 
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();
            server = new Server();
            server.connectedEvent += UserConnected;
            server.msgReceivedEvent += MessageReceived;
            server.userDisconnectEvent += UserConnected;
            ConnectToServerCommand = new RelayCommand(item => server.ConnectToServer(userName), o => !string.IsNullOrEmpty(userName));

            SendMessageCommand = new RelayCommand(item => server.SendMessageToServer(message), o => !string.IsNullOrEmpty(message));
        }

        private void MessageReceived()
        {
            var msg = server.packetReader.ReadMesage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        private void UserConnected()
        {
            var user = new UserModel
            {
                UserName = server.packetReader.ReadMesage(),
                ID = server.packetReader.ReadMesage(),
            };
            if (!Users.Any(x => x.ID == user.ID)) 
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
        
    }
}
