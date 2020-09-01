using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak_1.Model;
using Zadatak_1.Service;
using Zadatak_1.View;

namespace Zadatak_1.ViewModel
{
    class MainUserViewModel : ViewModelBase
    {
        MainUserView mainUserView;
        Entity context = new Entity();
        Tools tool = new Tools();

        public MainUserViewModel(MainUserView mainUserOpen,string username)
        {
            mainUserView = mainUserOpen;
            Username = username;
            UserList = tool.GetUsers();
            UserFeedList = tool.GetFeeds();
            UserRequestList = tool.GetRequests();
            User = new tblUser();
            UserFeed = new vwUser_Feed();
            UserRequest = new vwUser_Request_Receiving();
        }

        #region Properties
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value;
                OnPropertyChanged("Username");
            }
        }
        private tblUser user;

        public tblUser User
        {
            get { return user; }
            set { user = value;
                OnPropertyChanged("User");
            }
        }
        private List<tblUser> userList;

        public List<tblUser> UserList
        {
            get { return userList; }
            set { userList = value;
                OnPropertyChanged("UserList");
            }
        }

        private vwUser_Feed userFeed;

        public vwUser_Feed UserFeed
        {
            get { return userFeed; }
            set { userFeed = value;
                OnPropertyChanged("UserFeed");
            }
        }
        private List<vwUser_Feed> userFeedList;

        public List<vwUser_Feed> UserFeedList
        {
            get { return userFeedList; }
            set { userFeedList = value;
                OnPropertyChanged("UserFeedList");
            }
        }

        private vwUser_Request_Receiving userRequest;

        public vwUser_Request_Receiving UserRequest
        {
            get { return userRequest; }
            set { userRequest = value;
                OnPropertyChanged("UserRequest");
            }
        }

        private List<vwUser_Request_Receiving> userRequestList;

        public List<vwUser_Request_Receiving> UserRequestList
        {
            get { return userRequestList; }
            set { userRequestList = value;
                OnPropertyChanged("UserRequestList");
            }
        }


        #endregion
    }
}
