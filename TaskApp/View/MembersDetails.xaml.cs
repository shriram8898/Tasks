using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TaskApp.Data;
using TaskApp.Models;
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
    public sealed partial class MembersDetails : Page
    {
        public PassData pd;
        public ObservableCollection<Employee> emp1 = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> emp2 = new ObservableCollection<Employee>();
        public EmployeeDataLayer edl = new EmployeeDataLayer();
        public MembersDetails()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            pd = e.Parameter as PassData;
            emp1 = await edl.LoadEmployees();
            foreach (var item in emp1)
            {
                Employee ed = new Employee();
                ed.id = item.id; ed.name = item.name; ed.designation = item.designation; ed.role = item.role; ed.username = item.username;
                string pic = "Assets/" + ed.id + ".jpg";
                ed.Img = new BitmapImage(new Uri(this.BaseUri, pic));
                emp2.Add(ed);
            }
            if (!(pd.emp.designation == "manager"))
                Addmember.Visibility = Visibility.Collapsed;
        }

        private async void Addmembers_Click(object sender, RoutedEventArgs e)
        {
            InitializeData();
            await Addmembers.ShowAsync();
        }
        private async void InitializeData()
        {
            Role.Items.Clear();
            Position.Items.Clear();
            Role.Items.Add("developer");
            Role.Items.Add("quality analysis");
            Position.Items.Add("manager");
            Position.Items.Add("team leader");
            Position.Items.Add("member");
        }

        private async void add1_Click(object sender, RoutedEventArgs e)
        {
            string name1 = empname.Text;
            string id1 = empid.Text;
            string role1 = Role.SelectedItem.ToString();
            string email1 = Email.Text;
            string position1 = Position.SelectedItem.ToString();
            if (id1 == "" || name1 == "" || email1 == "" || role1 == "" || position1 == "")
                return;
            await edl.Write(id1, name1, email1, role1, position1);
            string pic = "Assets/" + id1 + ".jpg";
            emp2.Add(new Employee { id = id1, name = name1, username = email1, role = role1, designation = position1, Img = new BitmapImage(new Uri(this.BaseUri, pic)) });
            Addmembers.Hide();
        }

        private void close1_Click(object sender, RoutedEventArgs e)
        {
            Addmembers.Hide();
        }

    }
}
