using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class TeamDetails : Page
    {
        public ObservableCollection<Team> team = new ObservableCollection<Team>();
        public ObservableCollection<members> mem = new ObservableCollection<members>();
        public ObservableCollection<Employee> mem1 = new ObservableCollection<Employee>();
        public Team t = new Team();
        public PassData pd;
        public EmployeeDataLayer edl = new EmployeeDataLayer();
        public TeamsDataLayer Tdl = new TeamsDataLayer();
        public TeamDetails()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            pd = e.Parameter as PassData;
            if (!(pd.emp.designation == "manager"))
            {
                add_team.Visibility = Visibility.Collapsed;
                add1.Visibility = Visibility.Collapsed;

            }
            LoadTeams();
            LoadMembersItems();
        }

        private async void add_team_Click(object sender, RoutedEventArgs e)
        {
            await Addteam.ShowAsync();
        }
        private async void LoadTeams()
        {
            team.Clear();
            string tableCommand;
            ObservableCollection<Team> teams = await Tdl.Load();
            if (teams.Count > 0)
            {
                t = teams[0];
                foreach (var item in teams)
                {
                    ObservableCollection<members> head = await edl.LoadingMembers(item.name, "designation");
                    if (head.Count > 0)
                        item.head = head[0].empname;
                    head = await edl.LoadingMembers(item.name, "");
                    item.count = head.Count.ToString();
                    team.Add(item);
                }
            }

        }

        private void teams_ItemClick(object sender, ItemClickEventArgs e)
        {
            t = (Team)e.ClickedItem;
            LoadMembersItems();
        }
        private async void LoadMembersItems()
        {
            mem.Clear();
            mem1.Clear();
            mem = await edl.LoadingMembers(t.name, "");
            foreach (var item in mem)
            {
                Employee ed = new Employee();
                ed.name = item.empname;
                ed.role = item.role;
                ed.designation = item.designation;
                string pic = "Assets/" + item.empid + ".jpg";
                ed.Img = new BitmapImage(new Uri(this.BaseUri, pic));
                mem1.Add(ed);
            }
            member.ItemsSource = null;
            member.ItemsSource = mem1;
        }

        private async void add_Click(object sender, RoutedEventArgs e)
        {
            string tableCommand;
            string name = teamName.Text;
            string type = teamType.Text;
            if (name == "" || type == "")
                return;
            await Tdl.Insertteams(name, type, pd.emp.id, pd.emp.name);
            team.Add(new Team { name = name, type = type, manager = pd.emp.name });
            Addteam.Hide();
            this.Frame.Navigate(typeof(TeamDetails), pd);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Addteam.Hide();
        }

        private async void add2_Click(object sender, RoutedEventArgs e)
        {
            var item = (Employee)employee.SelectedItem;
            var items = item.name + " " + item.id;
            string[] name = items.Split(' ');
            string role1 = await edl.GetDetails(name[1]);
            string[] role = role1.Split('-');
            bool result = await edl.InsertToMembers(t.name, name[1], name[0], role[0], role[1]);
            Employee ed = new Employee();
            ed.name = name[0];
            ed.role = role[0];
            ed.designation = role[1];
            string pic = "Assets/" + name[1] + ".jpg";
            ed.Img = new BitmapImage(new Uri(this.BaseUri, pic));
            mem1.Add(ed);
            this.Frame.Navigate(typeof(TeamDetails), pd);
            add_member.Hide();
        }

        private void close1_Click(object sender, RoutedEventArgs e)
        {
            add_member.Hide();
        }

        private async void add1_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> temp = await LoadTeamMembers();
            ObservableCollection<string> temp2 = new ObservableCollection<string>();
            foreach (var item in mem)
            {
                temp2.Add(item.empname + " " + item.empid);
            }
            temp2 = new ObservableCollection<string>(temp.Except(temp2));
            ObservableCollection<Employee> temp1 = new ObservableCollection<Employee>();
            foreach (var items in temp2)
            {
                string[] item = items.Split(' ');
                string pic = "Assets/" + item[1] + ".jpg";
                temp1.Add(new Employee { name = item[0], Img = new BitmapImage(new Uri(this.BaseUri, pic)), id = item[1] });
            }
            employee.ItemsSource = temp1;
            await add_member.ShowAsync();
        }
        private async Task<ObservableCollection<string>> LoadTeamMembers()
        {
            string tableCommand;
            ObservableCollection<Employee> assign;
            ObservableCollection<string> select1 = new ObservableCollection<string>();
            assign = await edl.LoadEmployees();
            foreach (var vari in assign)
                select1.Add(vari.name + " " + vari.id);
            return select1;
        }
    }
}
