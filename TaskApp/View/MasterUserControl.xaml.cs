using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskApp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace TaskApp.View
{
    public sealed partial class MasterUserControl : UserControl
    {
        public TaskDetails TaskDetails { get { return this.DataContext as TaskDetails; } }
        public MasterUserControl()
        {
            this.InitializeComponent();
        }
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Today;
            string[] spli = dt.ToString().Split(' ');
            string today = spli[0];
            dt = dt.AddDays(-1);
            spli = dt.ToString().Split(' ');
            string yesterday = spli[0];
            string[] time = TaskDetails.createdDate.ToString().Split(' ');
            if (time[0] == today)
                date1.Text = "Today";
            else if (time[0] == yesterday)
                date1.Text = "Yesterday";
            else
                date1.Text = TaskDetails.createdDate.ToString("MMMM dd");
            coloTheStatus();
        }
        private void text_TextChanged(object sender, TextChangedEventArgs e)
        {
            coloTheStatus();
        }
        private void coloTheStatus()
        {
            if (text.Text == "High")
            {
                Priority.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (text.Text == "Medium")
            {
                Priority.Foreground = new SolidColorBrush(Colors.Gray);
            }
            else if (text.Text == "Low")
            {
                Priority.Foreground = new SolidColorBrush(Colors.SeaGreen);
            }
            else if(text.Text=="None")
                Priority.Foreground = new SolidColorBrush(Colors.Black);
        }
    }
}
