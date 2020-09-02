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
    class MainUserViewModel : ViewModelBase
    {
        MainUserView mainUserView;
        Entity context = new Entity();
        Tools tool = new Tools();

        public MainUserViewModel(MainUserView mainUserOpen,string username)
        {
            mainUserView = mainUserOpen;
            Username = username;
            UserList = tool.GetUsers(Username);
            UserFeedList = tool.GetFeeds();
            UserRequestList = tool.GetRequests(Username);
            User = new tblUser();
            UserFeed = new vwUser_Feed();
            UserRequest = new vwUser_Request_Sending();
        }

        #region Properties

        private string post;

        public string Post
        {
            get { return post; }
            set { post = value;
                OnPropertyChanged("Post");
            }
        }

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

        private vwUser_Request_Sending userRequest;

        public vwUser_Request_Sending UserRequest
        {
            get { return userRequest; }
            set { userRequest = value;
                OnPropertyChanged("UserRequest");
            }
        }

        private List<vwUser_Request_Sending> userRequestList;

        public List<vwUser_Request_Sending> UserRequestList
        {
            get { return userRequestList; }
            set { userRequestList = value;
                OnPropertyChanged("UserRequestList");
            }
        }


        #endregion

        #region Commands

        private ICommand like;
        public ICommand Like
        {
            get
            {
                if (like==null)
                {
                    like = new RelayCommand(param => LikeExecute(), param => CanLikeExecute());
                }
                return like;
            }
        }

        private void LikeExecute()
        {
            try
            {              
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Information;

                MessageBoxResult resultMessageBox = MessageBox.Show("Are you sure you want to like this post?", "Like approval", btnMessageBox, icnMessageBox);

                if (resultMessageBox == MessageBoxResult.Yes)
                {
                    int FeedID = UserFeed.FeedID;
                    tblUser userToFind = (from r in context.tblUsers where r.Username == Username select r).FirstOrDefault();

                    if (tool.IfAlreadyLiked(FeedID, userToFind.UserID) == true && tool.AreTheyFriends(userToFind.UserID, UserFeed.UserID) ==true)
                    {
                       
                        tblFeed feedToFind = (from r in context.tblFeeds where r.FeedID == FeedID select r).FirstOrDefault();
                        feedToFind.LikeNumbers++;
                        context.SaveChanges();
                        UserFeedList = tool.GetFeeds();
                        MessageBox.Show("You liked this post.");

                        //inserting this user in like list for this post:                  
                        tblLikeList newList = new tblLikeList();
                        newList.Feed_ID = FeedID;
                        newList.UserLikedID = userToFind.UserID;
                        context.tblLikeLists.Add(newList);
                        context.SaveChanges();
                    }
                    else if (tool.AreTheyFriends(userToFind.UserID,UserFeed.UserID) == false)
                    {
                        MessageBox.Show("You are can not like this feed because you are not friend with the publisher.");
                    }
                    else if (tool.IfAlreadyLiked(FeedID, userToFind.UserID) == false)
                    {
                        MessageBox.Show("You have already liked this post.");
                    }
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanLikeExecute()
        {
            //int FeedID = UserFeed.FeedID;
            //tblUser userToFind = (from r in context.tblUsers where r.Username == Username select r).FirstOrDefault();

            if (UserFeed==null )/*&& tool.IfAlreadyLiked(FeedID,userToFind.UserID)==false)*/
            {
                return false;
            }
            else
            {
                return true;
            }
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

            if (UserFeed == null )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private ICommand sendRequest;
        public ICommand SendRequest
        {
            get
            {
                if (sendRequest==null)
                {
                    sendRequest = new RelayCommand(param => SendRequestExecute(), param => CanSendRequestExecute());
                }
                return sendRequest;
            }
        }

        private bool CanSendRequestExecute()
        {
            if (User == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SendRequestExecute()
        {
            try
            {
                int idReceiving = User.UserID;
                tblUser userSending = (from r in context.tblUsers where r.Username == Username select r).FirstOrDefault();

                if (tool.AreTheyFriends(idReceiving, userSending.UserID) == true)
                {
                    MessageBox.Show("You are already friends");
                }
                else
                {
                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                    MessageBoxResult resultMessageBox = MessageBox.Show("Are you sure you want to send friend request?", "Request approval", btnMessageBox, icnMessageBox);

                    if (resultMessageBox == MessageBoxResult.Yes)
                    {
                        if (tool.AreTheyFriends(userSending.UserID,idReceiving)==false && tool.StatusPending(userSending.UserID,idReceiving)==false)
                        {
                            tblRequest newRequest = new tblRequest();
                            newRequest.UserID_Receiving = idReceiving;
                            newRequest.UserID_Sending = userSending.UserID;
                            newRequest.Approved = false;
                            context.tblRequests.Add(newRequest);
                            context.SaveChanges();
                            MessageBox.Show("Friend request is sent.");
                        }
                        else if (tool.StatusPending(userSending.UserID, idReceiving) == true)
                        {
                            MessageBox.Show("Request is already sent.");
                        }
                        else
                        {
                            MessageBox.Show("You are already friend with this user.");
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand accept;
        public ICommand Accept
        {
            get
            {
                if (accept==null)
                {
                    accept = new RelayCommand(param => AcceptExecute(), param => CanAcceptExecute());
                }
                return accept;
            }
        }

        private bool CanAcceptExecute()
        {
            return true;
        }

        private void AcceptExecute()
        {
            try
            {
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult resultMessageBox = MessageBox.Show("Are you sure you want to accept friend request?", "Accept approval", btnMessageBox, icnMessageBox);

                if (resultMessageBox == MessageBoxResult.Yes)
                {
                    int userSending = UserRequest.UserID_Sending;
                    int userToFindID = (from r in context.tblUsers where r.Username == Username select r.UserID).FirstOrDefault();

                    tblRequest requestToFind = (from r in context.tblRequests where r.UserID_Receiving == userToFindID && r.UserID_Sending == userSending select r).FirstOrDefault();
                    requestToFind.Approved = true;
                    context.SaveChanges();
                    MessageBox.Show("You are now friends!");
                    UserRequestList = tool.GetRequests(Username);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private ICommand reject;
        public ICommand Reject
        {
            get
            {
                if (reject==null)
                {
                    reject = new RelayCommand(param => RejectExecute(), param => CanRejectExecute());
                }
                return reject;
            }
        }

        private void RejectExecute()
        {
            try
            {
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult resultMessageBox = MessageBox.Show("Are you sure you want to reject friend request?", "Accept approval", btnMessageBox, icnMessageBox);

                if (resultMessageBox == MessageBoxResult.Yes)
                {
                    int userSending = UserRequest.UserID_Sending;
                    int userToFindID = (from r in context.tblUsers where r.Username == Username select r.UserID).FirstOrDefault();

                    tblRequest requestToFind = (from r in context.tblRequests where r.UserID_Receiving == userToFindID && r.UserID_Sending == userSending select r).FirstOrDefault();
                    context.tblRequests.Remove(requestToFind);
                    context.SaveChanges();
                    MessageBox.Show("Friend request is rejected!");
                    UserRequestList = tool.GetRequests(Username);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanRejectExecute()
        {
            return true;
        }

        private ICommand publish;
        public ICommand Publish
        {
            get
            {
                if (publish==null)
                {
                    publish = new RelayCommand(param => PublishExecute(), param => CanPublishExecute());
                }
                return publish;
            }
        }

        private void PublishExecute()
        {
            try
            {
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult resultMessageBox = MessageBox.Show("Are you sure you want to publish this post?", "Publish approval", btnMessageBox, icnMessageBox);

                if (resultMessageBox == MessageBoxResult.Yes)
                {
                    
                    int userToFindID = (from r in context.tblUsers where r.Username == Username select r.UserID).FirstOrDefault();

                    tblFeed newFeed = new tblFeed();
                    newFeed.FeedContent = Post;
                    newFeed.LikeNumbers = 0;
                    newFeed.PublishDate = DateTime.Now;
                    newFeed.UserID = userToFindID;
                    context.tblFeeds.Add(newFeed);
                    context.SaveChanges();
                    MessageBox.Show("Your post is public.");
                    UserFeedList = tool.GetFeeds();
                    Post = "";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool CanPublishExecute()
        {
            if (String.IsNullOrEmpty(Post) || Post.Length>100)
            {
                return false;
            }
            else
            {
                return true;
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
            mainUserView.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }

        private ICommand change;
        public ICommand Change
        {
            get
            {
                if (change==null)
                {
                    change = new RelayCommand(param => ChangeExecute(), param => CanChangeExecute());
                }
                return change;
            }
        }

        private void ChangeExecute()
        {
            ChangePassword change = new ChangePassword(Username);
            change.ShowDialog();
        }

        private bool CanChangeExecute()
        {
            return true;
        }

        #endregion
    }
}
