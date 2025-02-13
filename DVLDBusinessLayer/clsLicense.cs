using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsLicense
    {
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public clsApplication ApplicationInfo { get; set; }
        public int DriverID { get; set; }
        public clsDriver DriverInfo { get; set; }
        public int LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public bool IsDetained { get; set; }
        public clsDetainedLicense DetainInfo { get; set; }
        
        public enum enIssueReason { FirstTime = 1, Renew = 2, ReplacementForDamaged = 3, ReplacementForLost = 4 }
        public enIssueReason IssueReason { get; set; }
        public string IssueReasonText
        {
            get
            {
                switch(this.IssueReason)
                {
                    case enIssueReason.FirstTime:
                        return "First Time";

                    case enIssueReason.Renew:
                        return "Renew";

                    case enIssueReason.ReplacementForDamaged:
                        return "Replacement For Damaged";

                    case enIssueReason.ReplacementForLost:
                        return "Replacement For Lost";
                }

                return "";
            }
        }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedUserInfo { get; set; }

        public enum enMode { AddNew, Update }
        public enMode Mode { get; set; }




        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = null;
            this.PaidFees = 0;
            this.IsActive = false;
            this.IssueReason = 0;
            this.CreatedByUserID = -1;

            this.Mode = enMode.AddNew;

        }
        public clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.ApplicationInfo = clsApplication.FindBaseApplication(ApplicationID);
            this.DriverID = DriverID;
            this.DriverInfo = clsDriver.FindByDriverID(DriverID);
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(this.LicenseClassID);
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IsDetained = clsLicenseData.IsDetained(this.LicenseID);
            this.DetainInfo = clsDetainedLicense.Find(this.LicenseID);

            
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedUserInfo = clsUser.Find(CreatedByUserID);

            this.Mode = enMode.Update;
        }




        static public DataTable LicensesList()
        {
            return clsLicenseData.LicensesList();
        }

        static public DataTable LicensesListForDriverID(int DriverID)
        {
            return clsLicenseData.LicensesListForDriverID(DriverID);
        }

        bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (short)this.IssueReason, this.CreatedByUserID);
            return this.LicenseID != -1;
        }

        bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (short)this.IssueReason, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateLicense();
            }

            return false;
        }

        static public clsLicense FindByLicenseID(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClassID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = null;
            decimal PaidFees = 0;
            bool IsActive = false;
            short IssueReason = 0;
            

            if(clsLicenseData.GetLicenseInfoByLicenseID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClassID, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            }
            else
            {
                return null; 

            }


        }

        static public clsLicense FindByApplicationID(int ApplicationID)
        {
            int LicenseID = -1, DriverID = -1, LicenseClassID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = null;
            decimal PaidFees = 0;
            bool IsActive = false;
            short IssueReason = 0;

            if (clsLicenseData.GetLicenseInfoByApplicationID(ApplicationID, ref LicenseID, ref DriverID, ref LicenseClassID, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            }
            else
            {
                return null;

            }


        }

        static public bool IsExist(int LicenseID)
        {
            return clsLicenseData.IsExist(LicenseID);
        }
        static public bool IsExist(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.IsExist(PersonID, LicenseClassID);
        }

        static public bool IsLicenseExistsByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.IsLicenseExistByPersonID(PersonID, LicenseClassID);
        }

        public bool IsLicenseExpired()
        {
            return this.ExpirationDate < DateTime.Now;
        }
        public bool DeactivateLicense()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }

        static public bool IsActiveLicenseExists(int PersonID, int LicenseClass)
        {
            return clsLicenseData.IsActiveLicenseExists(PersonID, LicenseClass);
        }

        public clsLicense RenewLicense(string Notes,int CreatedByUserID)
        {
            //First Create new Application

            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;

            if(!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = (Notes != "") ? Notes : null;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save())
            {
                return null;
            }

            DeactivateLicense();

            return NewLicense;

        }

        public clsLicense IssueReplacement(enIssueReason IssueReason, int CreatedByUserID)
        {
            //First Create new Application

            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (IssueReason == enIssueReason.ReplacementForDamaged)? (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense : (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = (IssueReason == enIssueReason.ReplacementForDamaged)? clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).ApplicationFees : clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save())
            {
                return null;
            }

            DeactivateLicense();

            return NewLicense;

        }

        public int Detain(decimal FineFees, int CreatedByUserID)
        {
            return clsDetainedLicenseData.DetainLicense(this.LicenseID, FineFees, CreatedByUserID);

        }

        public bool Release(int ReleasedByUserID, ref int ApplicationID)
        {
            //First we Create new Application

            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).ApplicationFees;
            Application.CreatedByUserID = ReleasedByUserID;

            if (!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }
            
            ApplicationID=Application.ApplicationID;

            return this.DetainInfo.ReleaseDetainedLicense(ReleasedByUserID, Application.ApplicationID);
        }

    }
}
