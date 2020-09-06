using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zadatak_1.Model;
using Zadatak_1.Service;
using Zadatak_1.ViewModel;

namespace Zadatak_1.View
{
    /// <summary>
    /// Interaction logic for AddEdit.xaml
    /// </summary>
    public partial class AddEdit : Window
    {
        Tools tool = new Tools();

        public AddEdit(string usename)
        {
            InitializeComponent();
            this.DataContext = new AddEditViewModel(this, usename);
        }
        private void NumbersOnlyTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var vm = (ProfileViewModel)this.DataContext;

        //    Profile profile = (Profile)this.DataContext;

        //    profile.lblAbout.Content = tool.About(vm.Username);
        //    profile.lblInterests.Content = tool.Interests(vm.Username);
        //    profile.lblAge.Content = tool.Age(vm.Username);
        //}
    }
}
