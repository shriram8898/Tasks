using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp.Data
{
    public interface IEmployee
    {
        Task<Employee> Get(string us, string pas);
        Task Write(string id1, string name1, string email1, string role1, string position1);
        Task<bool> InsertToMembers(string name, string v1, string v2, string v3, string v4);
    }
    public class EmployeeDataLayer:IEmployee
    {
        public bool flag { get; set; } = true;
        public async Task<Employee> Get(string us, string pas)
        {
            Employee emp = new Employee();
            if (flag)
            {
                string tableCommand = "SELECT * FROM employee WHERE username='" + us + "' AND password='" + pas + "';";
                return await Task.Run(() => DataBase.VerifyDatabase(tableCommand));
            }
            return emp;
        }
        public async Task Write(string id1, string name1, string email1, string role1, string position1)
        {
            if (flag)
            {
                string tableCommand = "INSERT INTO employee(empid,empname,designation,role,username,password)" +
                "VALUES('" + id1 + "','" + name1 + "','" + position1 + "','" + role1 + "','" + email1 + "','password');";
                bool result = await DataBase.ExecuteCommand(tableCommand);
            }
        }
        public async Task<ObservableCollection<members>> LoadingMembers(string name, string v)
        {
            ObservableCollection<members> mem = new ObservableCollection<members>();
            string tableCommand;
            if (flag)
            {
                if (v == "designation")
                {
                    tableCommand = "SELECT * FROM members WHERE name='" + name + "' AND designation='team leader';";
                    mem = await DataBase.LoadTeams(tableCommand);
                }
                else
                {
                    tableCommand = "SELECT * FROM members WHERE name='" + name + "';";
                    mem = await DataBase.LoadTeams(tableCommand);
                }
            }
            return mem;
        }
        public async Task<string> GetDetails(string v)
        {
            string value = "";
            if (flag)
            {
                string tableCommand = "SELECT role,designation FROM employee WHERE empid='" + v + "';";
                value = await DataBase.EmpDetails(tableCommand);
            }
            return value;
        }
        public async Task<bool> InsertToMembers(string name, string v1, string v2, string v3, string v4)
        {
            bool result = false;
            string tableCommand;
            if (flag)
            {
                tableCommand = "INSERT INTO members(name,empid,empname,role,designation)" +
                "VALUES('" + name + "','" + v1 + "','" + v2 + "','" + v3 + "','" + v4 + "');";
                result = await DataBase.ExecuteCommand(tableCommand);
            }
            return result;
        }
        public async Task<ObservableCollection<Employee>> LoadEmployees()
        {
            ObservableCollection<Employee> emp = new ObservableCollection<Employee>();
            if (flag)
            {
                string tableCommand = "SELECT * FROM employee;";
                emp = await DataBase.EmployeeDetails(tableCommand);
            }
            return emp;
        }

        public async Task<ObservableCollection<string>> Loading(Employee emp)
        {
            ObservableCollection<string> value;
            string tableCommand = "SELECT * FROM teams WHERE empid='" + emp.id + "';";
            value=await DataBase.LoadTeam1(tableCommand);
            return value;
        }

        public async Task<ObservableCollection<members>> LoadTeams(string item)
        {
            string tableCommand = "SELECT * FROM members WHERE name='" + item + "';";
            return await DataBase.LoadTeams(tableCommand);
        }

        public async Task<ObservableCollection<members>> LoadMembers1(Employee emp)
        {
            string tableCommand = "SELECT * FROM members WHERE empid='" + emp.id + "';";
             return await DataBase.LoadTeams(tableCommand);
        }
    }
}
