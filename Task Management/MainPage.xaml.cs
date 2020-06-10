using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Task_Management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Employee emp;
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void Hamburger_Click(object sender, RoutedEventArgs e)
        {
            Split.IsPaneOpen = !Split.IsPaneOpen;
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (all.IsSelected)
                mhy.Navigate(typeof(view), emp);
            else if (create.IsSelected)
                mhy.Navigate(typeof(Create), emp);
            else if (collection.IsSelected)
                mhy.Navigate(typeof(Collections), emp);
        }

        private async void signin_Click(object sender, RoutedEventArgs e)
        {
            string user = userid.Text;
            string password = pass.Text;
            if (user == "" || password =="")
                return;
             await chechUserdb(user, password);
        }

        private async Task chechUserdb(string user, string password)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string comm = "SELECT * FROM emp Where email='" + user + "' AND password='" + password + "';";
                SqliteCommand createTable = new SqliteCommand(comm, db);
                SqliteDataReader data = createTable.ExecuteReader(); ;
                if(data.Read())
                {
                    emp = new Employee { email = data.GetString(4), empName = data.GetString(1), password = data.GetString(5),
                        post = data.GetString(3), team = data.GetString(2), empid = data.GetString(0) };
                    login.Visibility = Visibility.Collapsed;
                    Task.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Check the username and password", "Invalid");
                    await messageDialog.ShowAsync();
                }
            }
        }
    }
}
