using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Model;
using Zadatak_1.Service;

namespace Zadatak_1.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        MainWindow main;
        Entity context = new Entity();
        Tools tool = new Tools();

        public MainWindowViewModel(MainWindow mainOpen)
        {
            main = mainOpen;
        }

        #region Properties
        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        private string fullname;

        public string FullName
        {
            get { return fullname; }
            set
            {
                fullname = value;
                OnPropertyChanged("FullName");
            }
        }

        private bool loginb;

        public bool LoginB
        {
            get { return loginb; }
            set { loginb = value;
                OnPropertyChanged("LoginB");
            }
        }

        private bool registerb;

        public bool RegisterB
        {
            get { return registerb; }
            set { registerb = value;
                OnPropertyChanged("RegisterB");
            }
        }

        #endregion

        #region Commands
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
            main.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }

        private ICommand login;
        public ICommand Login
        {
            get
            {
                if (login == null)
                {
                    login = new RelayCommand(param => LoginExecute(), param => CanLoginExecute());
                }
                return login;
            }
        }
        
        private void LoginExecute()
        {
            try
            {
                if (RegisterB==true)
                {
                    if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
                    {
                        MessageBox.Show("Please enter your credentials");
                    }
                    else
                    {
                        tblUser newUser = new tblUser();
                        newUser.Username = Username;
                        newUser.Pasword = Password;

                        if (tool.UsernameExist(Username)==true)
                        {
                            context.tblUsers.Add(newUser);
                            MessageBox.Show("You have succsesfully registered.");
                            context.SaveChanges();
                        }
                        else
                        {
                            MessageBox.Show("Username already exist.Please try with another one");
                        }
                    }
                }
                else if (LoginB==true)
                {
                    if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
                    {
                        MessageBox.Show("Please enter your credentials");
                    }
                    //if username exists
                    else if (tool.UsernameExist(Username) == false && tool.CredentialsMatch(Username,Password)==true)
                    {
                        MessageBox.Show("Welcome!");
                    }
                    else
                    {
                        MessageBox.Show("Invalid parametres.");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private bool CanLoginExecute()
        {
            if (RegisterB==false && LoginB==false)
            {
                return false;
            }
            else if (RegisterB==true && LoginB==true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
