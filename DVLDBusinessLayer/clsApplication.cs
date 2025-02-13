 using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static DVLDBusinessLayer.clsApplication;

namespace DVLDBusinessLayer
{
    public class clsApplication
    {
        public enum enApplicationType {NewDrivingLicence = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicense = 5, NewInternationalDrivingLicense = 6, RetakeTest = 7}

        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public clsPerson PersonInfo { get; set; }
        public string ApplicantFullName
        {
            get
            {
                return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate {  get; set; }
        public int ApplicationTypeID { get; set; }
        public clsApplicationType ApplicationTypeInfo { get; set; }
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 }
        public enApplicationStatus ApplicationStatus { get; set; }
        public string StatusText
        {
            get
            {
                switch(ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";

                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }

        public enum enMode { AddNew, Update }
        public enMode Mode { get; set; }


        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;

            this.Mode = enMode.AddNew;

        }

        public clsApplication(int applicationID,  int applicantPersonID, DateTime applicationDate, int applicationTypeID, 
                              enApplicationStatus applicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            this.ApplicationID = applicationID;
            this.ApplicantPersonID = applicantPersonID;
            this.PersonInfo = clsPerson.Find(applicantPersonID);
            this.ApplicationDate = applicationDate;
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTypeInfo = clsApplicationType.Find(applicationTypeID);
            this.ApplicationStatus = applicationStatus;
            this.LastStatusDate = lastStatusDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
            this.CreatedByUserInfo = clsUser.Find(createdByUserID);

            this.Mode = enMode.Update;
        }


        static public DataTable ApplicationsList()
        {
            return clsApplicationData.ApplicationsList();
        }

        static public clsApplication FindBaseApplication(int ApplicationID)
        {
            int applicantPersonID = -1, applicationTypeID = -1, createdByUserID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            int applicationStatus = 0;
            decimal paidFees = 0;

            if (clsApplicationData.GetApplicationInfoByApplicationID(ApplicationID, ref applicantPersonID, ref applicationDate,
                         ref applicationTypeID, ref applicationStatus, ref lastStatusDate, ref paidFees, ref createdByUserID))
            {
                return new clsApplication(ApplicationID, applicantPersonID, applicationDate, applicationTypeID, (enApplicationStatus)applicationStatus, lastStatusDate, paidFees, createdByUserID);
            }

            else
            {
                return null;
            }

        }
        
        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID, Convert.ToInt16(this.ApplicationStatus),
                                                           this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

            return this.ApplicationID != -1;

        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID, Convert.ToInt16(this.ApplicationStatus),
                                                           this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, (short)enApplicationStatus.Cancelled);

        }

        public bool SetComplete()
        {
                return clsApplicationData.UpdateStatus(this.ApplicationID, (short)enApplicationStatus.Completed);
        }

        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:

                    if(_AddNewApplication())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateApplication();
            }

            return false;

        }

        static public bool Delete(int ApplicationID)
        {
            return clsApplicationData.Delete(ApplicationID);
        }

        static public bool IsApplicationExist(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExists(ApplicationID);
        }

        static public bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }

        public bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);

        }

        static public int GetActiveApplicationID(int PersonID, clsApplication.enApplicationType ApplicationType)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID, (int)ApplicationType);
        }

        static public int GetActiveApplicationIDForLicenseClass(int PersonID, clsApplication.enApplicationType ApplicationType, int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationType, LicenseClassID);
        }

        public int GetActiveApplicationID(clsApplication.enApplicationType ApplicationType)
        {
            return GetActiveApplicationID(this.ApplicantPersonID, ApplicationType);
        }

    }
}
