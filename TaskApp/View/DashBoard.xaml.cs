using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskApp.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TaskApp.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashBoard : Page
    {
        public PassData pd = new PassData();
        public DashBoard()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            pd = e.Parameter as PassData;
            string pic = "Assets/" + pd.emp.id + ".jpg";
            dp.ProfilePicture = new BitmapImage(new Uri(this.BaseUri, pic));
            Empname.Text = pd.emp.name;
            myframe.Navigate(typeof(TaskList), pd);
        }

        private void Hamburger_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }

        private async void Content_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Task.IsSelected)
            {
                myframe.Navigate(typeof(TaskList), pd);
            }
            else if (Teams.IsSelected)
            {
                myframe.Navigate(typeof(TeamDetails), pd);
            }
            else if (Members.IsSelected)
            {
                myframe.Navigate(typeof(MembersDetails), pd);
            }
            else if (Settings.IsSelected)
            {
                // myframe.Navigate(typeof(Settings), pd);
            }
            else if (Logout.IsSelected)
            {
                Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
