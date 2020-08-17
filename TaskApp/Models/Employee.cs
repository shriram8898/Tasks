using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace TaskApp.Models
{
    public class Employee
    {
        public BitmapImage Img { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string designation { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string role { get; set; }
    }
}
