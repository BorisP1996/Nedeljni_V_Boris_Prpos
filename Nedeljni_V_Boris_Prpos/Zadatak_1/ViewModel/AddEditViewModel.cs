using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Model;
using Zadatak_1.Service;
using Zadatak_1.View;

namespace Zadatak_1.ViewModel
{
    class AddEditViewModel : ViewModelBase
    {
        AddEdit addEdit;
        Entity context = new Entity();
        Tools tool = new Tools();

        public AddEditViewModel(AddEdit addEditOpen,string username)
        {
            addEdit = addEditOpen;
            Profile = new tblProfile();
            //Profile = tool.FindTblProfile(username);
            Username = username;
        }
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private tblProfile profileUser;

        public tblProfile Profile
        {
            get { return profileUser; }
            set
            {
                profileUser = value;
                OnPropertyChanged("Profile");
            }
        }

        private bool update;

        public bool Update
        {
            get { return update; }
            set { update = value; }
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
            addEdit.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
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

        private void SaveExecute()
        {
            try
            {
                int userId = (from r in context.tblUsers where r.Username==Username select r.UserID).FirstOrDefault();
                tblProfile profile = new tblProfile();
                profile.About = Profile.About;
                profile.Interests = Profile.Interests;
                profile.Age = Profile.Age;
                profile.UserID = userId;

                if (profile.Age < 10)
                {
                    MessageBox.Show("User must be at least 10 years old");
                }
                else
                {
                    List<tblProfile> profileList = context.tblProfiles.ToList();
                    foreach (tblProfile item in profileList)
                    {
                        if (item.UserID==userId)
                        {
                            context.tblProfiles.Remove(item);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    context.tblProfiles.Add(profile);
                    context.SaveChanges();
                    MessageBox.Show("Personal informations are saved.");
                    addEdit.Close();
                    Update = true;
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanSaveExecute()
        {
            if (String.IsNullOrEmpty(Profile.About) || String.IsNullOrEmpty(Profile.Interests) || String.IsNullOrEmpty(Profile.Age.ToString()) || Profile.Interests.Length>130 || Profile.About.Length>130)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
