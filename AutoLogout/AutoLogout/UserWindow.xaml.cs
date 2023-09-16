using AutoLogout.Common;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace AutoLogout
{
    /// <summary>
    /// UserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserWindow : Window
    {
        private readonly HubConnection _hubConnection;
        private readonly HttpClient _httpClient;
        private string _token;
        private string _username;

        public UserWindow()
        {
            InitializeComponent();

            #region 检查登录token并请求token 
            CheckAndShowLoginWindow();
            // 创建HTTP客户端
            //_httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("http://localhost:5080/api/auth"); // 后端API的地址

            //if (!string.IsNullOrEmpty(_token))
            //{
            //    // 获取JWT令牌（您可能需要在登录窗口中输入用户名和密码）
            //    _token = "your_jwt_token";

            //    // 启动SignalR连接
            //    StartSignalRConnection();
            //}

            #endregion 检查登录token并请求token

            // 创建SignalR连接
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5080/chatHub") // 服务器的SignalR Hub URL
            .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                // 处理并显示接收到的消息
                Dispatcher.Invoke(() =>
                {
                    chatTextBox.AppendText($"{user}: {message}\n");
                });
            });
            _hubConnection.On<string, string>("Logout", (user, message) =>
            {
                // 处理并显示接收到的消息
                Dispatcher.Invoke(() =>
                {
                    chatTextBox.AppendText($"{_username}: Logout\n");
                    //清空用户token，退出
                    _token = null;
                    WebLogout();
                    //CheckAndShowLoginWindow();
                    // 登出成功，关闭窗口或执行其他操作
                    //MessageBox.Show("已登出！");
                    // 在登出成功后关闭应用程序
                    //Application.Current.Shutdown();
                });
            });

            // 启动SignalR连接
            StartSignalRConnection();
        }


        /// <summary>
        /// 检查是否登录，未登录就弹出登录窗体
        /// </summary>
        private void CheckAndShowLoginWindow()
        {
            if (string.IsNullOrEmpty(_token))
            {
                var loginWindow = new LoginWindow();
                loginWindow.LoginSuccess += OnLoginSuccess;
                loginWindow.ShowDialog();
            }
        }

        private void OnLoginSuccess(string token)
        {
            _token = token;
            // 解析JWT令牌并获取用户名
            string username = JwtHelper.GetUsernameFromToken(token);
            if (!string.IsNullOrEmpty(username))
            {
                // 在这里使用用户名，例如，将其显示在UI上
                // 示例：userNameLabel.Content = "Welcome, " + username;
                _username = username;
            }
            else
            {
                // 处理无法获取用户名的情况
                MessageBox.Show("无法获取用户名！");
            }
        }

        private async void StartSignalRConnection()
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法连接到聊天服务器：{ex.Message}");
            }
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取用户输入的消息
            string message = inputTextBox.Text;

            CheckAndShowLoginWindow();

            // 发送消息到服务器
            await _hubConnection.InvokeAsync("SendMessage", _username, message);

            // 清空输入框
            inputTextBox.Text = "";
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            //// 发送登出请求到后端（您需要在后端实现清除令牌的功能）
            //var request = new HttpRequestMessage(HttpMethod.Post, "logout");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            //var response = await _httpClient.SendAsync(request);

            //if (response.IsSuccessStatusCode)
            //{
            //    // 登出成功，关闭窗口或执行其他操作
            //    MessageBox.Show("已登出！");
            //}
            //else
            //{
            //    // 登出失败，显示错误消息
            //    MessageBox.Show("登出失败！");
            //}
        }

        public void WebLogout()
        {
            // 弹出登录窗体
            LoginWindow loginForm = new LoginWindow();

            // 当前用户窗体隐藏
            this.Hide();

            // 等待登录成功
            loginForm.LoginSuccess += (token) =>
            {
                _token = token;
                // 解析JWT令牌并获取用户名
                string username = JwtHelper.GetUsernameFromToken(token);
                if (!string.IsNullOrEmpty(username))
                {
                    _username = username;
                }
                else
                {
                    MessageBox.Show("无法获取用户名！");
                }
                // 显示当前用户窗体
                this.Show();
            };

            // 显示登录窗体
            loginForm.ShowDialog();
        }
    }
}