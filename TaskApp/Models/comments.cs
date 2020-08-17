using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TaskApp.Models
{
    public class comments
    {
        public string id { get; set; }
        public string message { get; set; }
        public string empid { get; set; }
        public string empname { get; set; }
        public DateTime dt { get; set; }
        public StorageFile storagefile { get; set; }
        public bool IsFile { get; set; } = false;
    }
}
