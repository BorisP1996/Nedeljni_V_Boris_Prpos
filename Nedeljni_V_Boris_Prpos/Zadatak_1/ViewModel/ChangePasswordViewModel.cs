using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Model;
using Zadatak_1.View;

namespace Zadatak_1.ViewModel
{
    class ChangePasswordViewModel : ViewModelBase
    {
        ChangePassword changeView;
        Entity context = new Entity();

        public ChangePasswordViewModel(string username,ChangePassword passOpen)
        {
            changeView = passOpen;
            Username = username;
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value;
                OnPropertyChanged("Username");
            }
        }

        private string old;

        public string Old
        {
            get { return old; }
            set { old = value;
                OnPropertyChanged("Old");
            }
        }

        private string neww;

        public string New
        {
            get { return neww; }
            set { neww = value;
                OnPropertyChanged("New");
            }
        }

        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save==null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }

        private bool CanSaveExecute()
        {
            return true;
        }

        private void SaveExecute()
        {
            try
            {
                string oldPassword = (from r in context.tblUsers where r.Username == Username select r.Pasword).FirstOrDefault();

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult resultMessageBox = MessageBox.Show("Are you sure you want to change password?", "Be carefull!", btnMessageBox, icnMessageBox);

                if (resultMessageBox == MessageBoxResult.Yes)
                {
                    if (oldPassword == Old && New.Length > 0)
                    {
                        tblUser userToFind = (from r in context.tblUsers where r.Username == Username select r).FirstOrDefault();
                        userToFind.Pasword = New;
                        context.SaveChanges();
                        MessageBox.Show("Password is changed");
                        New = "";
                        Old = "";

                    }
                    else if (oldPassword!=Old)
                    {
                        MessageBox.Show("You did not input correct old password.");
                    }
                    else if (New.Length<1)
                    {
                        MessageBox.Show("New password can not be empty");
                    }
                    else
                    {
                        MessageBox.Show("Wrong input");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand close;
        public ICommand Close
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }
        private void CloseExecute()
        {
            changeView.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }
    }
}
