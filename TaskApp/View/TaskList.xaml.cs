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
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
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
    public sealed partial class TaskList : Page
    {
        public PassData pd = new PassData();
        public ObservableCollection<TaskDetails> tds = new ObservableCollection<TaskDetails>();
        public ObservableCollection<string> temp = new ObservableCollection<string>();
        public TaskDetails selected;
        public int count = 0;
        public TaskDataLayer tdl = new TaskDataLayer();
        public EmployeeDataLayer edl = new EmployeeDataLayer();
        public ObservableCollection<TaskDetails> search = new ObservableCollection<TaskDetails>();
        public TaskList()
        {
            this.InitializeComponent();           

        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            pd = e.Parameter as PassData;
            temp = await LoadTeamMembers();
            MenuFlyoutItemSettings(temp);
            tds = await Task.Run(() => tdl.Get("All", pd.emp));
            LoadSpecificTask();

        }
        private async void Add_task_Click(object sender, RoutedEventArgs e)
        {
            initializeData();
            await contentDialog1.ShowAsync();
        }
        
        private void SelectedTask_ItemClick(object sender, ItemClickEventArgs e)
        {
            pd.Selected = e.ClickedItem as TaskDetails;
            pd.tds = tds;
            this.Frame.Navigate(typeof(TaskDetailedView), pd);
        }

        private async Task<ObservableCollection<string>> LoadTeamMembers()
        {
            ObservableCollection<members> teams = new ObservableCollection<members>();
            ObservableCollection<string> select1 = new ObservableCollection<string>();
            if (pd.emp.designation == "manager")
            {
                select1.Add(pd.emp.name + " " + pd.emp.id);
                var items = await edl.Loading(pd.emp);
                foreach (string item in items)
                {
                    var teams1 = await edl.LoadTeams(item);
                    foreach (var it in teams1)
                        select1.Add(it.empname + " " + it.empid);

                    return select1;

                }
                select1.Add(pd.emp.name + " " + pd.emp.id);
            }
            else
            {
                teams = await edl.LoadMembers1(pd.emp);
                foreach (var item in teams)
                {
                    var assign = await edl.LoadTeams(item.name);
                    foreach (var vari in assign)
                    {
                        string value = vari.empname + " " + vari.empid;
                        select1.Add(value);
                    }
                }
            }
            return select1;
        }

        //CONTENDIALOG BOX INTIALIZATION AND CORRESPONDING EVENT HANDLING
        private async void initializeData()
        {
            priority.Items.Clear();
            collective.Items.Clear();
            priority.Items.Add("None");
            priority.Items.Add("Low");
            priority.Items.Add("Medium");
            priority.Items.Add("High");
            collective.Items.Add("sample");
            collective.Items.Add("Login");
            collective.Items.Add("Other");
            taskName.Text = "";
            taskDetails.Text = "";
            priority.SelectedIndex = 0;
            Assignto.SelectedIndex = -1;
            collective.SelectedIndex = 2;
            temp = await LoadTeamMembers();
            ObservableCollection<Employee> temp1 = new ObservableCollection<Employee>();
            foreach (var items in temp)
            {
                string[] item = items.Split(' ');
                string pic = "Assets/" + item[1] + ".jpg";
                temp1.Add(new Employee { name = item[0], Img = new BitmapImage(new Uri(this.BaseUri, pic)), id = item[1] });
            }

            Assignto.ItemsSource = temp1;
        }


        private async void add_Click(object sender, RoutedEventArgs e)
        {
            string name = taskName.Text;
            string details = taskDetails.Text;
            string prior = priority.SelectedItem.ToString();
            var emplo = (Employee)Assignto.SelectedItem;
            string asign = emplo.name + " " + emplo.id;
            string coll = collective.SelectedItem.ToString();
            string[] values = startDate.Date.ToString().Split('+');
            var startdate = values[0];
            string[] values1 = endDate.Date.ToString().Split('+');
            var enddate = values1[0];
            if (name == "" || details == "" || asign == "" || coll == ""||startdate==""||enddate=="")
                return;
            else
            {
                await writeinDb(name, details, prior, asign, coll, startdate, enddate);
                pd.Selected = selected;
                pd.tds = tds;
                this.Frame.Navigate(typeof(TaskDetailedView), pd);
                contentDialog1.Hide();
                taskName.Text = "";
                taskDetails.Text = "";
                priority.SelectedIndex = 0;
                Assignto.SelectedIndex = -1;
                collective.SelectedIndex = 2;

            }
        }
        private async Task writeinDb(string name, string details, string prior, string asign, string coll, string startdate, string enddate)
        {
            string n = await DataBase.findIddb("T-");
            string[] assigned = asign.Split(" ");
            string status = "Open";
            var dt = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            selected = new TaskDetails
            {
                id = n,
                Assign_by = pd.emp.name,
                Assign_by_id = pd.emp.id,
                Assign_to = assigned[0],
                Assign_to_id = assigned[1],
                collective = coll,
                details = details,
                name = name,
                updated = DateTime.Now,
                createdDate = DateTime.Now,
                priority = prior,
                status = status,
                startdate = Convert.ToDateTime(startdate),
                enddate = Convert.ToDateTime(enddate)
            };
            tds.Add(selected);
            bool results = await tdl.Write(n, name, details, assigned[0], assigned[1], pd.emp.name, pd.emp.id, dt, prior, status, coll, "", "", startdate, enddate);

        }
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            contentDialog1.Hide();
        }
        private async void filter_Click(object sender, RoutedEventArgs e)
        {
            count = 0;
            Notask.Visibility = Visibility.Collapsed;
            stark1.Visibility = Visibility.Visible;
            MenuFlyoutItem selectedItemFlyout = sender as MenuFlyoutItem;
            string value = selectedItemFlyout.Text.ToString();
            tds.Clear();
            if (value == "All" || value == "Assigned by me")
            {
                tds = await Task.Run(() => tdl.Get(value, pd.emp));
            }
            else
            {
                string[] support = value.Split(' ');
                tds = await Task.Run(() => tdl.Get(support[1], pd.emp));
            }

            if (!(tds.Count > 0))
            {
                stark1.Visibility = Visibility.Collapsed;
                Notask.Visibility = Visibility.Visible;
            }
            LoadSpecificTask();
        }

        private void MenuFlyoutItemSettings(ObservableCollection<string> temp)
        {
            MenuFlyout flyout = new MenuFlyout();
            foreach (var item in temp)
            {
                MenuFlyoutItem mi = new MenuFlyoutItem();
                string[] values = item.Split(' ');
                mi.Text = item;
                mi.Click += filter_Click;
                flyout.Items.Add(mi);
            }
            foreach (var item in flyout.Items)
            {
                filter.Items.Insert(filter.Items.Count, item);
            }
        }

        private void SearchBox_QueryChanged(object sender, TextChangedEventArgs e)
        {
            if (search != null)
            {
                var values = search.Where(a => a.name.ToUpper().Contains(searchitem.Text.ToUpper()));
                //tds = tds.Where(x => x.name.ToUpper().Contains(values));
                var groups = from c in values
                             group c by c.collective;
                this.cvs.Source = groups;
            }
        }

        private void TextBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            LoadSpecificTask();
        }

        private void LoadSpecificTask()
        {
            int iterator;
            for (iterator = count; iterator < count + 10; iterator++)
                if (tds.Count > iterator)
                    search.Add(tds[iterator]);
                else
                    break;
            count += iterator;
            var groups = from c in search
                         group c by c.collective;
            this.cvs.Source = groups;
            tasks.SelectedIndex = -1;
        }

        private void scroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight && tds.Count > count)
                LoadSpecificTask();
        }
    }
}
