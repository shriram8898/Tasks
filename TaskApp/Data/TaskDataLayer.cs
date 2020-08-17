using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp.Data
{
    public interface ITask
    {
        Task<ObservableCollection<TaskDetails>> Get(string value, Employee emp);
        Task<bool> Write(string n, string name1, string details, string v1, string v2, string name2, string id, string dt,
            string prior, string status, string coll, string taskid, string taskname, string startdate, string enddate);
        Task Delete(TaskDetails selected);
    }
    public class TaskDataLayer:ITask
    {
        public bool flag { get; set; } = true;

        //Reading Task List Based on  employee visual preference either from database or API
        public async Task<ObservableCollection<TaskDetails>> Get(string value, Employee emp)
        {
            ObservableCollection<TaskDetails> ts = new ObservableCollection<TaskDetails>();
            if (flag)
            {
                string tableCommand = "";
                if (value == "All")
                {
                    tableCommand = "SELECT * FROM taskdetails WHERE taskid='0';";
                }
                else if (value == "Assigned by me")
                {
                    tableCommand = "SELECT * FROM taskdetails WHERE assignedbyId='" + emp.id + "' AND taskid='0';";
                }
                else
                {
                    tableCommand = "SELECT * FROM taskdetails WHERE assignedtoId='" + value + "' AND taskid='0';";
                }
                ts = await Task.Run(() => DataBase.ReadDataDb(tableCommand));
            }
            return ts;
        }
        // Writing Task details either to DataBase or API       
        public async Task<bool> Write(string n, string name1, string details, string v1, string v2, string name2, string id,
             string dt, string prior, string status, string coll, string taskid, string taskname, string startdate, string enddate)
        {
            string tableCommand = ""; bool result = false;
            if (flag)
            {
                if (taskid == "")
                {
                    tableCommand = "INSERT INTO taskdetails(id,name,details,assignedto,assignedtoid,assignedby,assignedbyid,Updated,Created,Priority,Status,Collective,taskid,startdate,enddate)" +
                   "VALUES('" + n + "','" + name1 + "','" + details + "','" + v1 + "','" + v2 + "','" + name2 + "','" + id + "','" + dt + "','" + dt + "','" + prior + "','" + status + "','" + coll + "','0','" + startdate + "','" + enddate + "');";
                    result = await Task.Run(() => DataBase.ExecuteCommand(tableCommand));
                }
                else
                {
                    tableCommand = "INSERT INTO taskdetails(id,name,details,assignedto,assignedtoid,assignedby,assignedbyid,Updated,Created,Priority,Status,collective,taskid,taskname,startdate,enddate)" +
                    "VALUES('" + n + "','" + name1 + "','" + details + "','" + v1 + "','" + v2 + "','" + name2 + "','" + id + "','" + dt + "','" + dt + "','" + prior + "','" + status + "','" + coll + "','" + taskid + "','" + taskname + "','" + startdate + "','" + enddate + "');";
                    result = await DataBase.ExecuteCommand(tableCommand);
                }

            }
            return result;
        }
        //Reading  subatsk Based on current task from DATABASE or API
        public async Task<ObservableCollection<TaskDetails>> LoadSub(TaskDetails task)
        {
            string tableCommand = "";
            ObservableCollection<TaskDetails> ts = new ObservableCollection<TaskDetails>();
            if (flag)
            {
                tableCommand = "SELECT * FROM taskdetails WHERE taskid='" + task.id + "';";
                ts = await Task.Run(() => DataBase.ReadDataDb(tableCommand));
            }
            return ts;
        }
        //Delete of a specific Task Details from DATABASE or API
        public async Task Delete(TaskDetails selected)
        {
            if (flag)
            {
                string tableCommand = "DELETE FROM taskdetails WHERE id='" + selected.id + "' OR taskid='" + selected.id + "';";
                bool result = await DataBase.ExecuteCommand(tableCommand);
                await DataBase.DeleteSubTask();
            }
        }

        //Update the Task Details
        public async Task Update1(TaskDetails selected)
        {
            if (flag)
            {
                string tableCommand = "UPDATE taskdetails SET status='Close' WHERE id='" + selected.id + "'";
                bool result = await DataBase.ExecuteCommand(tableCommand);
            }
        }

        //Update the Task Details
        public async Task Update(string prior, TaskDetails selected, string value)
        {
            if (flag)
            {
                if (value == "priority")
                {
                    string tableCommand = "UPDATE taskdetails SET Priority='" + prior + "' WHERE id='" + selected.id + "'";
                    bool result = await DataBase.ExecuteCommand(tableCommand);
                }
                else if (value == "status")
                {

                    string tableCommand = "UPDATE taskdetails SET Status='" + prior + "' WHERE id='" + selected.id + "'";
                    bool result = await DataBase.ExecuteCommand(tableCommand);
                }
                else if (value == "collective")
                {
                    string tableCommand = "UPDATE taskdetails SET Collective='" + prior + "' WHERE id='" + selected.id + "'";
                    bool result = await DataBase.ExecuteCommand(tableCommand);
                }
                else if (value == "enddate")
                {
                    string tableCommand = "UPDATE taskdetails SET enddate='" + prior + "' WHERE id='" + selected.id + "'";
                    bool result = await DataBase.ExecuteCommand(tableCommand);
                }
                else if (value == "startdate")
                {
                    string tableCommand = "UPDATE taskdetails SET startdate='" + prior + "' WHERE id='" + selected.id + "'";
                    bool result = await DataBase.ExecuteCommand(tableCommand);
                }
            }

        }
        //Update the Task Details
        public async Task Update2(string dt, string name, string details, string prior, string status, string coll, string id, string startdate, string enddate)
        {
            if (flag)
            {
                string tableCommand = "UPDATE taskdetails SET Updated='" + dt + "',name='" + name + "',details='" + details + "',Priority='" + prior + "',Status='" + status + "',Collective='" + coll + "',startdate='" + startdate + "',enddate='" + enddate + "' WHERE id='" + id + "'";
                bool result = await DataBase.ExecuteCommand(tableCommand);
            }
        }
    }
}
