using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp.Data
{
    public interface ITeams
    {
        Task Insertteams(string name1, string type, string id, string name2);
        Task<ObservableCollection<Team>> Load();
    }
    public class TeamsDataLayer:ITeams
    {
        public bool flag { get; set; } = true;

        public async Task Insertteams(string name1, string type, string id, string name2)
        {
            string tableCommand;
            if (flag)
            {
                tableCommand = "INSERT INTO teams(name,type,empid,empname)" +
               "VALUES('" + name1 + "','" + type + "','" + id + "','" + name2 + "');";
                var result = await DataBase.ExecuteCommand(tableCommand);
            }
        }
        public async Task<ObservableCollection<Team>> Load()
        {
            ObservableCollection<Team> t = new ObservableCollection<Team>();
            if (flag)
            {
                string tableCommand = "SELECT * FROM teams;";
                t = await DataBase.LoadTeam(tableCommand);
            }
            return t;
        }

    }
}
