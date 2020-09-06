using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak_1.View;
using Zadatak_1.Model;
using System.Windows.Input;

namespace Zadatak_1.ViewModel
{
    class ViewProfileViewModel : ViewModelBase
    {
        ViewProfile viewProfile;
        Entity context = new Entity();

        public ViewProfileViewModel(ViewProfile viewProfileOpen,int id)
        {
            viewProfile = viewProfileOpen;
            IDD = id;
            Profile = (from r in context.tblProfiles where r.UserID == IDD select r).FirstOrDefault();
        }

        private tblProfile profile;

        public tblProfile Profile
        {
            get { return profile; }
            set { profile = value;
                OnPropertyChanged("Profile");
            }
        }

        private int ID;

        public int IDD
        {
            get { return ID; }
            set { ID = value; }
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
            viewProfile.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }


    }
}
