using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp.Data
{
    public interface ITaskUtility
    {
        Task<ObservableCollection<comments>> Load(TaskDetails selected);
        Task Insert(string id1, string id2, string name, string text, string dt, string value);
    }
    public class TaskUtilityDataLayer : ITaskUtility
    {
        public bool flag { get; set; } = true;
        public async  Task<ObservableCollection<comments>> Load(TaskDetails selected)
        {
            string tableCommand = "";
            ObservableCollection<comments> com1 = new ObservableCollection<comments>();
            if (flag)
            {
                tableCommand = "SELECT * FROM comment WHERE id='" + selected.id + "' ;";
                com1 = await DataBase.LoadCommentsFromDatabase(tableCommand);
            }
            return com1;
        }
        public async Task<string> LoadFiles(TaskDetails selected)
        {
            string tableCommand = "";
            string value = "";
            if (flag)
            {
                tableCommand = "SELECT * FROM files WHERE taskid='" + selected.id + "';";
                value = await Task.Run(() => DataBase.FileReader(tableCommand));
            }
            return value;
        }
        public async Task Insert(string id1, string id2, string name, string text, string dt, string value)
        {
            if (flag)
            {
                if (value == "files")
                {
                    string tableCommand = "INSERT INTO files(taskid,empname,empid,name,date)" +
                    "VALUES('" + id1 + "','" + name + "','" + id2 + "','" + text + "','" + Convert.ToDateTime(dt) + "');";
                    bool result = await DataBase.ExecuteCommand(tableCommand);
                }
                else
                {
                    string tableCommand = "INSERT INTO comment(id,empid,empname,message,date)" +
                    "VALUES('" + id1 + "','" + id2 + "','" + name + "','" + text + "','" + dt + "');";
                    bool result = await DataBase.ExecuteCommand(tableCommand);
                }
            }
        }
        public async Task Delete(TaskDetails selected)
        {
            string tableCommand = "DELETE FROM files WHERE taskid='" + selected.id + "';";
            bool result = await DataBase.ExecuteCommand(tableCommand);
            tableCommand = "DELETE FROM comment WHERE id='" + selected.id + "';";
            result = await DataBase.ExecuteCommand(tableCommand);
        }
    }
   
}
