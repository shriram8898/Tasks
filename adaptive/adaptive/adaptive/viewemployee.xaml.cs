using adaptive.Models;
using Gremlin.Net.Process.Traversal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace adaptive
{
    public sealed partial class viewemployee : UserControl
    {
        public Models.employee emp { get { return this.DataContext as Models.employee; } }

        employee Emp;
        public viewemployee()
        {
            this.InitializeComponent();
            Emp = new employee { Gendervalue = "null" };
            combo.Items.Add("Male");
            combo.Items.Add("Female");
            
          
            this.DataContextChanged += (s, e) => Bindings.Update();
            
        }

    }
}
