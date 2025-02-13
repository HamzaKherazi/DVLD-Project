using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsInternationalLicense
    {
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public clsApplication ApplicationInfo { get; set; }
        public int DriverID { get; set; }
        public clsDriver DriverInfo { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public clsLicense LocalLicenseInfo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }

        public enum enMode { AddNew, Update }
        public enMode Mode { get; set; }


        public clsInternationalLicense()
        {
            this.InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = true;
            this.CreatedByUserID = -1;
            this.Mode = enMode.AddNew;
        }

        public clsInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.ApplicationInfo = clsApplication.FindBaseApplication(ApplicationID);
            this.DriverID = DriverID;
            this.DriverInfo = clsDriver.FindByDriverID(DriverID);
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.LocalLicenseInfo = clsLicense.FindByLicenseID(IssuedUsingLocalLicenseID);
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.Find(CreatedByUserID);
            this.Mode = enMode.Update;
        }


        static public DataTable AllInternationalLicenses()
        {
            return clsInternationalLicenseData.AllInternationalLicenses();
        }

        static public DataTable LicensesListForDriverID(int DriverID)
        {
            return clsInternationalLicenseData.LicensesListForDriverID(DriverID);
        }

        bool _AddNewInternationalLicence()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
            return this.InternationalLicenseID != -1;
        }

        bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicence())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateInternationalLicense();

            }

            return false;
        }

        static public clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID=-1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = false;

            if(clsInternationalLicenseData.GetInternationalLicenseInfoByInternationalLicenseID(InternationalLicenseID, ref ApplicationID, ref DriverID, ref  IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            }
            else
            {
                return null;
            }
           
        }

        static public int GetActiveInternationalLicense(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicense(DriverID);
        }

        public bool Delete(int InternationalLicense)
        {
            return clsInternationalLicenseData.Delete(InternationalLicense);
        }


    }
}
