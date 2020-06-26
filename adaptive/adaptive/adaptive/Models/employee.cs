using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace adaptive.Models
{
   public class employee:INotifyPropertyChanged
    {
        public int sno { get; set; }
        public String id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
      //  public string gender { get; set; }
        public float salary { get; set; }
        public string teamname { get; set; }
        public long phoneno {get; set;}
        public  string shift { get; set; }
        public string dob { get; set; }
        public bool status { get; set; }
      
        private string gender;
        public string Gendervalue
        {
            get
            {
                return gender;
            }
            set
            {

                gender = value;
                onPropertyChanged("Gendervalue");

            }
           
        }
        private string imagesource;
        public string Imagesource
        {
            get
            {
                return imagesource;
            }
            set
            {
                if(Gendervalue=="Male")
                {
                    imagesource = "Assets/Malecircle.png";
                }
                if(Gendervalue=="Female")
                {
                    imagesource = "Assets/Femalecircle.png";
                }
               
               // onPropertyChanged("Gendervalue");
                onPropertyChanged("Imagesource");

            }

        }

        private void onPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
          
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    class employeemanager
    {
        public static ObservableCollection<employee> getemployeedetails()
        {
            ObservableCollection<employee> employee = new ObservableCollection<employee>();
            employee.Add(new employee() { sno = 1, id = "Vtouch101", firstname = "Hemavathi", lastname = "A", Gendervalue = "Female", salary = 12000, teamname = "vtouch-cliq", phoneno = 8015726383, shift = "gerneral", dob = "25.12.1999",status=false });
            employee.Add(new employee() { sno = 2, id = "Vtouch102", firstname = "Banumathi", lastname = "Anandan", Gendervalue = "Female", salary = 15000, teamname = "vtouch-desktop", phoneno = 3333022389, shift = "gerneral", dob = "02.10.1987", status = false});
            employee.Add(new employee() { sno = 3, id = "Vtouch103", firstname = "Lingusamy", lastname = "Arumugam", Gendervalue = "Male", salary = 35000, teamname = "Support", phoneno = 3312783302, shift = "gerneral", dob = "02.01.1996", status = false });
            employee.Add(new employee() { sno = 4, id = "Vtouch104", firstname = "Anandan", lastname = "Lingusamy", Gendervalue = "Male", salary = 45000, teamname = "CRM Mail", phoneno = 9012783302, shift = "gerneral", dob = "13.11.1963",status=false });
            employee.Add(new employee() { sno = 5, id = "Vtouch105", firstname = "Dhanabal", lastname = "S", Gendervalue = "Male", salary = 55000, teamname = "DSP", phoneno = 82302939202, shift = "gerneral", dob = "02.01.1999", status = false });

          for (int i = 0; i < employee.Count; i++)
            {
                if (employee[i].Gendervalue == "Male")
                    employee[i].Imagesource = "Assets/Malecircle.png";
                if (employee[i].Gendervalue == "Female")
                    employee[i].Imagesource = "Assets/Femalecircle.png";
            }

            return employee;
        }
    }
}
