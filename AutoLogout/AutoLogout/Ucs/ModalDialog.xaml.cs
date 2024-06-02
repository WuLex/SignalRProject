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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoLogout.Ucs
{
    /// <summary>
    /// CustomDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ModalDialog : UserControl
    {
        public static readonly DependencyProperty DialogContentProperty =
         DependencyProperty.Register("DialogContent", typeof(string), typeof(ModalDialog), new PropertyMetadata(string.Empty));

        public string DialogContent
        {
            get { return (string)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }
        
        public ModalDialog()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
