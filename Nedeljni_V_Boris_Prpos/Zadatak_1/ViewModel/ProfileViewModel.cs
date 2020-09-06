using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak_1.Service;
using Zadatak_1.View;
using Zadatak_1.Model;
using System.Windows.Input;
using System.Windows;

namespace Zadatak_1.ViewModel
{
    class ProfileViewModel : ViewModelBase  
    {
        Profile profile;
        Tools tool = new Tools();
        Entity context = new Entity();

        public ProfileViewModel(Profile profileOpen,string username)
        {
            profile = profileOpen;
            Username = username;
            UserFeedList = tool.GetMyFeeds(Username);
            UserFeed = new vwUser_Feed();
            Profile = tool.FindTblProfile(Username);
        }

        private string username;

        public string Username
        {
            get { return username; }
            set { username = value;
                OnPropertyChanged("Username");
            }
        }

        private vwUser_Feed userFeed;

        public vwUser_Feed UserFeed
        {
            get { return userFeed; }
            set
            {
                userFeed = value;
                OnPropertyChanged("UserFeed");
            }
        }
        private List<vwUser_Feed> userFeedList;

        public List<vwUser_Feed> UserFeedList
        {
            get { return userFeedList; }
            set
            {
                userFeedList = value;
                OnPropertyChanged("UserFeedList");
            }
        }

        private tblProfile profileUser;

        public tblProfile Profile
        {
            get { return profileUser; }
            set { profileUser = value;
                OnPropertyChanged("Profile");
            }
        }

        private ICommand addEdit;
        public ICommand AddEdit
        {
            get
            {
                if (addEdit==null)
                {
                    addEdit = new RelayCommand(param => AddEditExecute(), param => CanAddEditExecute());
                }
                return addEdit;
            }
        }

        private void AddEditExecute()
        {
            try
            {
                AddEdit addEdit = new AddEdit(Username);
                addEdit.ShowDialog();
                if ((addEdit.DataContext as AddEditViewModel).Update==true)
                {
                    Profile = tool.FindTblProfile(Username);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanAddEditExecute()
        {
            return true;
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
            profile.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }
        private ICommand likeList;
        public ICommand LikeList
        {
            get
            {
                if (likeList == null)
                {
                    likeList = new RelayCommand(param => LikeListExecute(), param => CanLikeListExecute());
                }
                return likeList;
            }
        }

        private void LikeListExecute()
        {
            try
            {
                List<tblLikeList> list = context.tblLikeLists.ToList();
                List<tblUser> users = context.tblUsers.ToList();

                if (list.Count == 0)
                {
                    MessageBox.Show("This feed does not have any likes");
                }
                else
                {
                    //will be used for storing user id's that liked
                    List<int> newListUserID = new List<int>();
                    //get all likes for this feed
                    foreach (tblLikeList item in list)
                    {
                        //ako se poklapa feedID onda uzmi userID=>pokupi sve usere koji su lajkovali
                        if (item.Feed_ID == UserFeed.FeedID)
                        {
                            newListUserID.Add(item.UserLikedID.GetValueOrDefault());
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //lista u kojoj ce biti username svih korisnika koji su lajkovali
                    List<string> usernames = new List<string>();

                    foreach (tblUser item in users)
                    {
                        //ako se poklapa sa id-em usera koji su lajkovali onda uzmi username i stavi ga u listu
                        if (newListUserID.Contains(item.UserID))
                        {
                            usernames.Add(item.Username);
                        }
                    }
                    //show message
                    var message = string.Join(Environment.NewLine, usernames);
                    MessageBox.Show(message);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanLikeListExecute()
        {

            if (UserFeed == null)
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
