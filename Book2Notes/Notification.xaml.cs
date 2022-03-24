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

namespace Book2Notes
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public static readonly DependencyProperty _message = DependencyProperty.Register
            (
            "Message", typeof(string), typeof(Notification), new PropertyMetadata(string.Empty)
            );

        public static readonly DependencyProperty _isSuccess = DependencyProperty.Register
            (
            "IsSuccess", typeof(bool), typeof(Notification), new PropertyMetadata(true)
            );

        public Notification(string message, bool isSuccess)
        {
            InitializeComponent();
            Message = message;
            IsSuccess = isSuccess;

            Image.Source = isSuccess ? new BitmapImage(new Uri("Assets\\success.png", UriKind.Relative)) 
                : new BitmapImage(new Uri("Assets\\error.png", UriKind.Relative));

            TextLabel.Content = message;
        }

        public string Message
        { 
            get { return (string)GetValue(_message); }
            set { SetValue(_message, value); }
        }

        public bool IsSuccess
        {
            get { return (bool)GetValue(_isSuccess); }
            set { SetValue(_isSuccess, value); }
        }
    }
}
