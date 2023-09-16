using AutoLogout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AutoLogout
{
    /// <summary>
    /// UserMvvmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserMvvmWindow : Window
    {
        public UserMvvmWindow()
        {
            InitializeComponent();
            DataContext = new UserViewModel();
        }
    }
}
