using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zadatak_1.Model;

namespace Zadatak_1.Service
{
    class Tools
    {
        public bool UsernameExist(string usernameInput)
        {
            try
            {
                using (Entity context = new Entity())
                {
                    List<tblUser> userList = context.tblUsers.ToList();
                    List<string> usernameList = new List<string>();

                    foreach (tblUser item in userList)
                    {
                        usernameList.Add(item.Username);
                    }

                    if (!usernameList.Contains(usernameInput))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return false;

            }
        }

        public bool CredentialsMatch(string inputUsername, string inputPassword)
        {
            try
            {
                using (Entity context = new Entity())
                {
                    List<tblUser> userList = context.tblUsers.ToList();

                    foreach (tblUser item in userList)
                    {
                        if (item.Username == inputUsername && item.Pasword == inputPassword)
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public List<vwUser_Feed> GetFeeds()
        {
            try
            {
                using (Entity context = new Entity())
                {
                    List<vwUser_Feed> list = context.vwUser_Feed.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        public List<vwUser_Request_Sending> GetRequests(string Username)
        {
            try
            {
                using (Entity context = new Entity())
                {
                    tblUser userToFind = (from r in context.tblUsers where r.Username == Username select r).FirstOrDefault();
                    List<vwUser_Request_Sending> list = context.vwUser_Request_Sending.ToList();
                    List<vwUser_Request_Sending> myRequests = new List<vwUser_Request_Sending>();

                    foreach (vwUser_Request_Sending item in list)
                    {
                        if (item.UserID_Receiving == userToFind.UserID && item.Approved==false) 
                        {
                            myRequests.Add(item);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    return myRequests;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        public List<tblUser> GetUsers(string Username)
        {
            try
            {
                using (Entity context = new Entity())
                {
                    List<tblUser> list = context.tblUsers.ToList();
                    List<tblUser> allExceptMe = new List<tblUser>();

                    foreach (tblUser item in list)
                    {
                        if (item.Username!=Username)
                        {
                            allExceptMe.Add(item);
                        }
                    }

                    return allExceptMe;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        public bool IfAlreadyLiked(int FeedIDinput, int UserIDinput)
        {
            try
            {
                using (Entity context = new Entity())
                {
                    List<tblLikeList> list = context.tblLikeLists.ToList();

                    foreach (tblLikeList item in list)
                    {
                        if (item.Feed_ID==FeedIDinput && item.UserLikedID==UserIDinput)
                        {
                            return false;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        public bool AreTheyFriends(int UserOne, int UserTwo)
        {
            try
            {
                using (Entity context = new Entity())
                {
                    List<tblRequest> list = context.tblRequests.ToList();

                    foreach (tblRequest item in list)
                    {
                        if (item.UserID_Sending==UserOne && item.UserID_Receiving==UserTwo && item.Approved==true)
                        {
                            return true;
                        }
                        else if (item.UserID_Sending == UserTwo && item.UserID_Receiving == UserOne && item.Approved == true)
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public bool StatusPending(int UserOne, int UserTwo)
        {
            try
            {
                using (Entity context = new Entity())
                {
                    List<tblRequest> list = context.tblRequests.ToList();

                    foreach (tblRequest item in list)
                    {
                        if (item.UserID_Sending == UserOne && item.UserID_Receiving == UserTwo && item.Approved == false)
                        {
                            return true;
                        }
                        else if (item.UserID_Sending == UserTwo && item.UserID_Receiving == UserOne && item.Approved == false)
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return false;
            }
        }
    }
}
