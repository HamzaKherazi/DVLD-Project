using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Test_Appointments
{
    public partial class  frmScheduleTest: Form
    {
        private int _LocalDrivingLicenseApplicationID;
        private clsTestType.enTestType _TestType;
        private int _TestAppointmentID;

        private enum enMode { AddNew, Update };
        private enMode _Mode;
        public frmScheduleTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestType = TestType;
        }

        public frmScheduleTest(int TestAppointmentID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _TestAppointmentID = TestAppointmentID;
        }
        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (!clsTestAppointment.IsExists(_LocalDrivingLicenseApplicationID, (int)_TestType))
                    {
                        ctrlScheduleTest1.ScheduleTest(_LocalDrivingLicenseApplicationID, _TestType);
                        return;
                    }
                    else
                    {
                        ctrlScheduleTest1.ScheduleRetakeTest(_LocalDrivingLicenseApplicationID, _TestType);
                        return;
                    }

                case enMode.Update:
                    ctrlScheduleTest1.EditScheduleTest(_TestAppointmentID);
                    break;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduleTest_Load_1(object sender, EventArgs e)
        {

        }
    }
}
