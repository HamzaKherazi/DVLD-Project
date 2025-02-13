using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsTestAppointment
    {
        public int TestAppointmentID { get; set; }
        public clsTestType.enTestType TestTypeID { get; set; }
        public clsTestType TestTypeInfo;
        public int LocalDrivingLicenseApplicationID { get; set; }
        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplicationInfo;
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserIDInfo;
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }
        public clsApplication RetakeTestApplicationInfo;

        public int TestID
        {
            get
            {
                return clsTestAppointment._GetTestID(this.TestAppointmentID);
            }
        }
 
        public enum enMode {AddNew, Update}
        public enMode Mode { get; set; }

        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeTestApplicationID = -1;

            this.Mode = enMode.AddNew;
        }

        public clsTestAppointment(int TestAppointmentID, clsTestType.enTestType TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.TestTypeInfo = clsTestType.Find((clsTestType.enTestType)TestTypeID);
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID(LocalDrivingLicenseApplicationID);
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            this.RetakeTestApplicationInfo = clsApplication.FindBaseApplication(RetakeTestApplicationID);

            this.Mode = enMode.Update;
        }


        static public DataTable TestAppointmentsList()
        {
            return clsTestAppointmentData.TestAppointmentsList();
        }

        static public DataTable TestAppointmentsListForLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return clsTestAppointmentData.GetTestAppointmentsForLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
        }

        private bool _AddNewTestAppointment()
        {
            int TestAppointmentID = clsTestAppointmentData.AddNewAppointment((int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);

            return TestAppointmentID != -1;

        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, this.AppointmentDate, this.IsLocked);
        }

        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:

                    if(_AddNewTestAppointment())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTestAppointment();
            }

            return false;
        }

        static public clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = -1, LocalDrivingLicenseApplicationID = -1, CreatedByUserID = -1, RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = 0;
            bool IsLocked = false;

            if (clsTestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            }
            else
            {
                return null;
            }

        }
        static public clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            int TestAppointmentID=-1, CreatedByUserID = -1, RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = 0;
            bool IsLocked = false;

            if (clsTestAppointmentData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, TestTypeID, ref TestAppointmentID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            }
            else
            {
                return null;
            }

        }
        static public DataTable GetTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestType)
        {
            return clsTestAppointmentData.GetTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, TestType);
        }
        public bool Delete(int TestAppointmentID)
        {
            return clsTestAppointmentData.Delete(TestAppointmentID);
        }

        static public bool IsTestAppointmentLockedForLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return clsTestAppointmentData.IsTestAppointmentLockedForLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
        }

        static public bool IsExists(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestAppointmentData.IsExists(LocalDrivingLicenseApplicationID, TestTypeID);
        }
        
        static private int _GetTestID(int TestAppointmentID)
        {
            return clsTestAppointmentData.GetTestID(TestAppointmentID); 
        }


    }
}
