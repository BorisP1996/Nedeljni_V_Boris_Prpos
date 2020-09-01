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
    }
}
