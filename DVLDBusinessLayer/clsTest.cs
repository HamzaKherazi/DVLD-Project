using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsTest
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public clsTestAppointment TestAppointmentInfo { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo;

        public enum enMode { AddNew, Update}
        public enMode Mode { get; set; }

        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = null;
            this.CreatedByUserID = -1;
            this.Mode = enMode.AddNew;
        }

        public clsTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.Find(CreatedByUserID);
            this.Mode = enMode.Update;
        }

        static public int GetTestTrials(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            return clsTestData.GetTestTrials(LocalDrivingLicenseApplicationID, (int)TestType);
        }
        
        static public DataTable TestsList()
        {
            return clsTestData.GetAllTests();
        }

        private bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return this.TestID != -1;
        }

        public bool Save()
        {
            if(this.Mode == enMode.AddNew)
            {
                if(_AddNewTest())
                {
                    this.Mode = enMode.Update;
                    return true;
                }
                
            }
            return false;
        }

        static public clsTest Find(int TestID)
        {
            int TestAppointmentID = -1, CreatedByUserID = -1;
            bool TestResult = false;
            string Notes = null;

            if(clsTestData.GetTestInfo(TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            else
            {
                return null;
            }

                    

        }



    }
}
