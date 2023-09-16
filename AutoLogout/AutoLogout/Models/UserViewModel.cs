using AutoLogout.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace AutoLogout.Models
{
    internal class UserViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _messages;
        private string _newMessage;
        private ICommand _sendMessageCommand;
        private ICommand _logoutCommand;

        public UserViewModel()
        {
            _messages = new ObservableCollection<string>();
            _sendMessageCommand = new UserRelayCommand(SendMessage);
            _logoutCommand = new UserRelayCommand(Logout);
        }

        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public string NewMessage
        {
            get { return _newMessage; }
            set
            {
                _newMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendMessageCommand
        {
            get { return _sendMessageCommand; }
        }

        public ICommand LogoutCommand
        {
            get { return _logoutCommand; }
        }

        private void SendMessage(object parameter)
        {
            // 实现发送消息逻辑，将消息添加到Messages集合中
            Messages.Add("You: " + NewMessage);
            NewMessage = "";
        }

        private void Logout(object parameter)
        {
            // 实现登出逻辑
            MessageBox.Show("Logged out!");
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
