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
    public sealed partial class view : Page
    {
        public Employee st;
        public string samp;
        public string[] str;
        public view()
        {
            this.InitializeComponent();
           
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
             st = e.Parameter as Employee;
            tasks.Items.Add("Assigned by me");
            tasks.Items.Add("Assigned to me");
            if(st.post=="manager")
            tasks.Items.Add("All Task");
        }
        private async void tasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str;
            string selected=tasks.SelectedItem.ToString();
            if(selected== "Assigned by me")
                str = "SELECT * FROM task WHERE assignedbyId='" + st.empid + "';";
            else if(selected== "Assigned to me")
                str = "SELECT * FROM task WHERE assignedtoId='" + st.empid + "';";
            else
                str="SELECT * FROM task WHERE team='"+st.team+"';";
            fillDb(str);
        }

        private void fillDb(string str)
        {
            lists.Items.Clear();
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand createTable = new SqliteCommand(str, db);
                SqliteDataReader reader = createTable.ExecuteReader();
               while(reader.Read())
                {
                    lists.Items.Add(reader.GetString(0) + " " + reader.GetString(1));
                }
            }
        }
        private async Task loadComments(string[] str)
        {
            comment.Items.Clear();
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string stri = "SELECT * FROM comment WHERE taskid='"+str[0]+"';";
                SqliteCommand createTable = new SqliteCommand(stri, db);
                SqliteDataReader reader = createTable.ExecuteReader();
                while(reader.Read())
                {
                    comment.Items.Add(reader.GetString(1));
                }
            }
        }

        private void fillDetailsinList(string[] stri)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string str="SELECT * FROM task WHERE taskid='"+stri[0]+"';";
                SqliteCommand createTable = new SqliteCommand(str, db);
                SqliteDataReader reader = createTable.ExecuteReader();
                if(reader.Read())
                {
                    taskid.Text = reader.GetString(0);
                    taskname.Text = reader.GetString(1);
                    taskdetails.Text = reader.GetString(7);
                    team.Text = reader.GetString(6);
                    assignby.Text = reader.GetString(3);
                    asignto.Text = reader.GetString(2);
                    if (st.empid == reader.GetString(5) || st.post == "manager")
                        delete.Visibility = Visibility.Visible;
                }
                
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            all.Visibility = Visibility.Visible;
            Details.Visibility = Visibility.Collapsed;
            delete.Visibility = Visibility.Collapsed;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            lists.Items.Remove(samp);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string str = "DELETE FROM comment WHERE taskid='" + taskid.Text + "'";
                SqliteCommand createTable = new SqliteCommand(str, db);
                createTable.ExecuteReader();
                str = "DELETE FROM task WHERE taskid='" + taskid.Text + "'";
                createTable = new SqliteCommand(str, db);
                createTable.ExecuteReader();
                all.Visibility = Visibility.Visible;
                Details.Visibility = Visibility.Collapsed;
                delete.Visibility = Visibility.Collapsed;
            }
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            if (text.Text == "")
                return;
            string mess = st.empName + ":" +text.Text;
            comment.Items.Add(mess);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string str = "INSERT INTO comment(taskid,comments)Values('"+taskid.Text+"','"+mess+"');";
                SqliteCommand createTable = new SqliteCommand(str, db);
                createTable.ExecuteReader();
                text.Text = "";
            }

        }
        private async void lists_ItemClick(object sender, ItemClickEventArgs e)
        {
            samp = e.ClickedItem.ToString();
            str = e.ClickedItem.ToString().Split(' ');
            fillDetailsinList(str);
            await loadComments(str);
            all.Visibility = Visibility.Collapsed;
            Details.Visibility = Visibility.Visible;
        }
    }
}
