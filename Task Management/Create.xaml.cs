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
    public sealed partial class Create : Page
    {
        public Employee emp;
        public string[] sele;
        public Create()
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
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string comm = "SELECT * FROM emp WHERE team='"+emp.team+"';";
                SqliteCommand createTable = new SqliteCommand(comm, db);
                SqliteDataReader readre = createTable.ExecuteReader();
                while(readre.Read())
                {
                    asignto.Items.Add(readre.GetString(1) + " " + readre.GetString(0));
                }
            }
        }

        private async void creates_Click(object sender, RoutedEventArgs e)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "taskdb.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string ano="Select * FROM task WHERE taskid='"+ taskid.Text + "';";
                SqliteCommand createTable = new SqliteCommand(ano, db);
                SqliteDataReader reader = createTable.ExecuteReader();
                if(!reader.Read())
                {
                    string comm = "INSERT INTO task(taskid,taskname,assignedto,assignedby,assignedtoId,assignedbyId,team,taskdetails)" +
                   "Values('" + taskid.Text + "','" + taskname.Text + "','" + sele[0] + "','" + emp.empName + "','" + sele[1] + "','" + emp.empid + "','" + emp.team + "','" + taskdetails.Text + "');";
                    createTable = new SqliteCommand(comm, db);
                    createTable.ExecuteReader();
                    taskid.Text = "";
                    taskdetails.Text = "";
                    taskname.Text = "";
                    asignto.SelectedItem = "";
                    asignto.SelectedIndex = -1;
                }
                else
                {
                    MessageDialog mes = new MessageDialog("", "Task ID already Exist!!!");
                    await mes.ShowAsync();
                }
            }
        }

        private void asignto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(asignto.SelectedIndex!=-1)
            {
                var vara = asignto.SelectedItem.ToString();
                sele = vara.Split(' ');
            }
        }
    }
}
