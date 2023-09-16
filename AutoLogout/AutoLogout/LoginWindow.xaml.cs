using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AutoLogout.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoLogout
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public event Action<string> LoginSuccess;
        private readonly HttpClient _httpClient;
        public LoginWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5080/api/auth/"); // 后端API的地址
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取用户输入的用户名和密码
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            // 构建登录凭据
            var credentials = new UserCredentials { Username = username, Password = password };

            // 发送登录请求到后端
            var response = await _httpClient.PostAsync("login", new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                // 登录成功，处理响应，通常是获取和存储令牌
                var jwtToken = await response.Content.ReadAsStringAsync();

                // 在实际应用中，您应该处理返回的令牌
                // 触发LoginSuccess事件，传递令牌给UserWindow
                LoginSuccess?.Invoke(jwtToken);

                MessageBox.Show("登录成功！");
                // 关闭登录窗口
                Close();
               
            }
            else
            {
                // 登录失败，显示错误消息
                MessageBox.Show("登录失败！");
            }
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // 发送登出请求到后端
            var response = await _httpClient.PostAsync("logout", null);

            if (response.IsSuccessStatusCode)
            {
                // 登出成功，处理响应，通常是清除令牌
                MessageBox.Show("已登出！");
            }
            else
            {
                // 登出失败，显示错误消息
                MessageBox.Show("登出失败！");
            }
        }
    }
}