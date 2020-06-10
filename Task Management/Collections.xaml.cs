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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Task_Management
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Collections : Page
    {
        public Employee emp;
        public string e1;
        public List<string> selectedItems1 = new List<string>();
        public Collections()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            emp = e.Parameter as Employee;
            initializeAll();
        }
        private void initializeAll()
        {
            Collection.Items.Clear();
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string comm = "SELECT * FROM colltask ;";
                SqliteCommand createTable = new SqliteCommand(comm, db);
                SqliteDataReader readre = createTable.ExecuteReader();
                while (readre.Read())
                {
                    Collection.Items.Add(readre.GetString(0) + " " + readre.GetString(2));
                }
            }
        }
        private void Collection_ItemClick(object sender, ItemClickEventArgs e)
        {
            e1 = e.ClickedItem.ToString();
            string[] item1 = e.ClickedItem.ToString().Split(' ');
            string item = item1[0];
            task.Items.Clear();
            loadAllfromDb(item);
            Collect.Visibility = Visibility.Collapsed;
            All.Visibility = Visibility.Visible;
        }

        private void loadAllfromDb(string item)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string comm = "SELECT * FROM colltask Where id='" + item + "' ;";
                SqliteCommand createTable = new SqliteCommand(comm, db);
                SqliteDataReader readre = createTable.ExecuteReader();
                while (readre.Read())
                {
                    string[] stark = readre.GetString(1).Split(',');
                    foreach(string str in stark)
                        task.Items.Add(str);
                }
            }
        }
        private void new_Click(object sender, RoutedEventArgs e)
        {
            
            view.Visibility = Visibility.Collapsed;
            Create.Visibility = Visibility.Visible;
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            All.Visibility = Visibility.Collapsed;
            Create.Visibility = Visibility.Collapsed;
            view.Visibility = Visibility.Visible;
            Collect.Visibility = Visibility.Visible;
            
        }
        private async void WriteInDb(List<string> selectedItems)
        {
            string stark="";
            foreach (var item in selectedItems)
            {
                stark+= item + ",";
            }
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string ano = "Select * FROM colltask WHERE id='" + collId.Text + "';";
                SqliteCommand createTable = new SqliteCommand(ano, db);
                SqliteDataReader reader = createTable.ExecuteReader();
                if (!reader.Read())
                {
                    
                        string comm = "INSERT INTO colltask(taskid,id,name)" +
                            "Values('" + stark + "','" + collId.Text + "','" + collname.Text + "')";
                        createTable = new SqliteCommand(comm, db);
                        createTable.ExecuteReader();
                    
                }
                else
                {
                    MessageDialog mes = new MessageDialog("", "Collection ID already Exist!!!");
                    await mes.ShowAsync();
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            selectedItems1.Clear();
            foreach (string item in allTasks.SelectedItems)
            {
                selectedItems1.Add(item);
            }
            WriteInDb(selectedItems1);
            Collection.Items.Add(collId.Text + " " + collname.Text);
            Create.Visibility = Visibility.Collapsed;
            view.Visibility = Visibility.Visible;
            Collect.Visibility = Visibility.Visible;
        }

        private async void se_Click(object sender, RoutedEventArgs e)
        {
            allTasks.Items.Clear();
            await systemEnable();
            selection.Visibility = Visibility.Visible;
        }

        private async Task systemEnable()
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string ano = "Select * FROM task WHERE team='" + emp.team + "';";
                SqliteCommand createTable = new SqliteCommand(ano, db);
                SqliteDataReader reader = createTable.ExecuteReader();
                while(reader.Read())
                {
                    allTasks.Items.Add(reader.GetString(0));
                }
            }
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            string[] sta = e1.Split(' ');
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string ano = "DELETE FROM colltask WHERE id='" + sta[0] + "';";
                SqliteCommand createTable = new SqliteCommand(ano, db);
                SqliteDataReader reader = createTable.ExecuteReader();
                
                    Collection.Items.Remove(e1);
                    All.Visibility = Visibility.Collapsed;
                    Create.Visibility = Visibility.Collapsed;
                    view.Visibility = Visibility.Visible;
                    Collect.Visibility = Visibility.Visible;
                
            }
        }
    }
}
