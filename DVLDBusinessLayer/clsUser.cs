using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsUser
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public clsPerson PersonInfo { get; set; }

        public enum enMode { AddNew, Update};

        public enMode Mode { get; set; }

        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = null;
            this.Password = null;
            this.IsActive = false;

            this.Mode = enMode.AddNew;

        }

        public clsUser(int userID, int personID, string userName, string password, bool isActive)
        {
            this.UserID = userID;
            this.PersonID = personID;
            this.PersonInfo = clsPerson.Find(personID);
            this.UserName = userName;
            this.Password = password;
            this.IsActive = isActive;

            this.Mode = enMode.Update;
        }

        static public DataTable UsersList()
        {
            return clsUserData.UsersList();
        }

        static public clsUser Find(int UserID)
        {
            int personID = -1;
            string username = null, password = null;
            bool isActive = false;

            if (clsUserData.GetUserByUserID(UserID, ref personID, ref username, ref password, ref isActive))
            {
                return new clsUser(UserID, personID, username, password, isActive);
            }
            else
            {
                return null;
            }
        }

        static public clsUser Find(string UserName)
        {
            int personID = -1, userID = -1;
            string password = null;
            bool isActive = false;

            if (clsUserData.GetUserByUserName(UserName,ref userID, ref personID, ref password, ref isActive))
            {
                return new clsUser(userID, personID, UserName, password, isActive);
            }
            else
            {
                return null;
            }
        }

        static public bool IsExist(string UserName)
        {
            return clsUserData.IsExist(UserName);
        }
        static public bool IsExist(int PersonID)
        {
            return clsUserData.IsExist(PersonID);
        }
        static public bool IsExist(string UserName, string Password)
        {
            return clsUserData.IsExist(UserName, Password);
        }



        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.Password, this.IsActive);
            return this.UserID != -1;
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);
        }

        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewUser())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateUser();
            }

            return false;

        }

        static public bool Delete(int UserID)
        {
            return clsUserData.Delete(UserID);
        }


    }
}
