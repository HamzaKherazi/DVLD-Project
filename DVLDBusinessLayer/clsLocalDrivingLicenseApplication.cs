using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLDBusinessLayer.clsApplication;

namespace DVLDBusinessLayer
{
    public class clsLocalDrivingLicenseApplication: clsApplication
    {
        public int LocalDrivingLicenseApplicationID { get; set; }

        public int LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }

        public string PersonFullName
        {
            get
            {
                return base.PersonInfo.FullName;
            }
        }

        public enum enMode { AddNew, Update }
        public enMode Mode { get; set; }


        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;

            this.Mode = enMode.AddNew;
        }
        public clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
                  DateTime ApplicationDate, int ApplicationTypeID, enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
                  decimal PaidFees, int CreatedByUserID, int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);

            this.Mode = enMode.Update;
        }

        static public DataTable LocalDrivingLicenseApplicationsList()
        {
            return clsLocalDrivingLicenseApplicationData.LocalDrivingLicenseApplicationsList();
        }
         
        static public clsLocalDrivingLicenseApplication FindByLocalDrivingLicenseAppID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            if(clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID))
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID, Application.ApplicantPersonID,
                               Application.ApplicationDate, Application.ApplicationTypeID, Application.ApplicationStatus, Application.LastStatusDate,
                                Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
                    
            }

            else
            {
                return null;
            }

        }
        static public clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID(ApplicationID, ref LocalDrivingLicenseApplicationID, ref LicenseClassID))
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID, Application.ApplicantPersonID,
                               Application.ApplicationDate, Application.ApplicationTypeID, Application.ApplicationStatus, Application.LastStatusDate,
                                Application.PaidFees, Application.CreatedByUserID, LicenseClassID);

            }

            else
            {
                return null;
            }

        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(this.ApplicationID, this.LicenseClassID);
            
            return this.LocalDrivingLicenseApplicationID != -1;


        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        }

        public bool Save()
        {
            base.Mode = (clsApplication.enMode) Mode;

            if(!base.Save())
            {
                return false;
            }

            switch (this.Mode)
            {
                case enMode.AddNew:

                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();
            }

            return false;

        }

        static public bool Delete(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.Delete(LocalDrivingLicenseApplicationID);
        }





        //Add other methods


        static public short GetPassedTests(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.GetPassedTests(LocalDrivingLicenseApplicationID);
        }

        static public bool HasPassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.GetPassedTests(LocalDrivingLicenseApplicationID) == 3;
        }

        public int IssueForTheFirstTime(string Notes, int CreatedUserID)
        {
            clsLicense License;
            License = new clsLicense();

            License.ApplicationID = this.ApplicationID;

            if (!clsDriver.IsExist(this.ApplicantPersonID))
            {
                clsDriver Driver = new clsDriver();

                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedUserID;
                Driver.CreatedDate = DateTime.Now;

                if(!Driver.Save())
                {
                    return -1;
                }
            }

            License.DriverID = clsDriver.FindByPersonID(this.ApplicantPersonID).DriverID;
            License.LicenseClassID = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes = (Notes != "") ? Notes : null;
            License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedUserID;

            this.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            this.SetComplete();

            int LicenseID = -1;

            if(License.Save())
            {
                LicenseID = License.LicenseID;
            }

            return LicenseID;
        }

        public int GetActiveLicense()
        {
            return clsLocalDrivingLicenseApplicationData.GetActiveLicense(this.ApplicantPersonID, this.LicenseClassID);
        }


    }
}
