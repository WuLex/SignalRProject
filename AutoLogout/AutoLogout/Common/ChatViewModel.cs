using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using AutoLogout.Models;

namespace AutoLogout.Common
{
    public class ChatViewModel
    {
        private HubConnection _hubConnection;

        public ObservableCollection<MessageViewModel> Messages { get; set; }
        public MessageViewModel CurrentMessage { get; set; }

        public ICommand SendMessageCommand { get; }
        public ICommand LogoutCommand { get; }

        public ChatViewModel()
        {
            Messages = new ObservableCollection<MessageViewModel>();
            CurrentMessage = new MessageViewModel();

            SendMessageCommand = new RelayCommand(SendMessage);
            LogoutCommand = new RelayCommand(Logout);

            InitializeSignalR();
        }

        private async void InitializeSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5080/chatHub") // SignalR Hub URL
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var newMessage = new MessageViewModel { User = user, Text = message };
                    Messages.Add(newMessage);
                });
            });

            _hubConnection.On("Logout", () =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // 处理退出逻辑
                    Messages.Clear();
                });
            });

            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(CurrentMessage.User) && !string.IsNullOrWhiteSpace(CurrentMessage.Text))
            {
                await _hubConnection.SendAsync("SendMessage", CurrentMessage.User, CurrentMessage.Text);
                CurrentMessage.Text = string.Empty;
            }
        }

        private async void Logout()
        {
            await _hubConnection.SendAsync("Logout");
        }
    }
}
