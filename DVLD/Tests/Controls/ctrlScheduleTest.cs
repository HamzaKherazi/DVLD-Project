using DVLD.Global_Classes;
using DVLD.Properties;
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
using static DVLDBusinessLayer.clsTestType;

namespace DVLD.Tests.Test_Appointments
{
    public partial class ctrlScheduleTest : UserControl
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _TestTypeID;

        public enTestType TestTypeID
        {
            get
            {
                return (enTestType)_TestTypeID;
            }
            set
            {
                _TestTypeID = (int)value;
                _SetTestImage(value);
                switch(value)
                {
                    case enTestType.VisionTest:
                        gbTest.Text = "Vision Test";
                        break;

                    case enTestType.WrittenTest:
                        gbTest.Text = "Written Test";
                        break;

                    case enTestType.StreetTest:
                        gbTest.Text = "Street Test";
                        break;
                }
            }
        }

        private clsTestAppointment _TestAppointment;
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }
        public bool DateTimePickerEnabled
        {
            get
            {
                return dtpDate.Enabled;
            }
            set
            {
                dtpDate.Enabled = value;
            }
        }


        private void _SetTestImage(clsTestType.enTestType TestType)
        {
            _TestTypeID = (int)TestType;
            switch (TestType)
            {
                case enTestType.VisionTest:
                    pbTest.Image = Resources.Vision_512;
                    break;

                case enTestType.WrittenTest:
                    pbTest.Image = Resources.Written_Test_512;
                    break;

                case enTestType.StreetTest:
                    pbTest.Image = Resources.driving_test_512;
                    break;

            }
        }

        public void ScheduleTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            _SetTestImage(TestType);

            gbTest.Text = clsTestType.Find(TestType).Title;

            lblTitle.Text = "Schedule Test";

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID(LocalDrivingLicenseApplicationID);
            
            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.ApplicantFullName;
            lblTrials.Text = clsTest.GetTestTrials(LocalDrivingLicenseApplicationID, TestType).ToString();
            dtpDate.Value = DateTime.Now;
            dtpDate.MinDate = DateTime.Now;
            lblFees.Text = clsTestType.Find(TestType).Fees.ToString();
            lblTotalFees.Text = (Convert.ToDecimal(lblFees.Text) + Convert.ToDecimal(lblRAppFees.Text)).ToString();

            _TestAppointment = new clsTestAppointment();

        }
        public void EditScheduleTest(int TestAppointmentID)
        {
            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);

            _TestTypeID = (int)_TestAppointment.TestTypeInfo.ID;
            _SetTestImage((enTestType)_TestAppointment.TestTypeID);
            gbTest.Text = _TestAppointment.TestTypeInfo.Title;

            lblTitle.Text = "Scheduled Test";

            _LocalDrivingLicenseApplication = _TestAppointment.LocalDrivingLicenseApplicationInfo;

            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.ApplicantFullName;
            lblTrials.Text = clsTest.GetTestTrials(_LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID, (clsTestType.enTestType)_TestAppointment.TestTypeID).ToString();
            dtpDate.Value = _TestAppointment.AppointmentDate;
            dtpDate.MinDate = DateTime.Now;
            lblFees.Text = _TestAppointment.TestTypeInfo.Fees.ToString();
            lblTotalFees.Text = (Convert.ToDecimal(lblFees.Text) + Convert.ToDecimal(lblRAppFees.Text)).ToString();

            if (_TestAppointment.IsLocked)
            {
                dtpDate.Enabled = false;
                lblSubtitle.Visible = true;
                btnSave.Enabled = false;
            }
        }
        public void ScheduleRetakeTest(int _LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            _TestTypeID = (int)TestType;

            ScheduleTest(_LocalDrivingLicenseApplicationID, TestType);

            lblTitle.Text = "Schedule Retake Test";
            lblTitle.Left = pbTest.Left + (pbTest.Width - lblTitle.Width) / 2;

            gbRetakeTest.Enabled = true;
            decimal RetakeAppFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees;
            lblRAppFees.Text = RetakeAppFees.ToString();
            lblTotalFees.Text = (RetakeAppFees + Convert.ToDecimal(lblFees.Text)).ToString();
            lblRTestAppID.Text = "N/A";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int ApplicationID = -1;

            if (gbRetakeTest.Enabled)
            {
                clsApplication Application = new clsApplication();
                Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = dtpDate.Value;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.New;
                Application.LastStatusDate = dtpDate.Value;
                Application.PaidFees = Convert.ToDecimal(lblRAppFees.Text);
                Application.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

                if(Application.Save())
                {
                    ApplicationID = Application.ApplicationID;
                }
                
            }

            _TestAppointment.TestTypeID = (enTestType)_TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpDate.Value;
            _TestAppointment.PaidFees = Convert.ToDecimal(lblTotalFees.Text);
            _TestAppointment.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;
            _TestAppointment.IsLocked = false;
            _TestAppointment.RetakeTestApplicationID = ApplicationID;


            if (_TestAppointment.Save())
            {
                lblRTestAppID.Text = ApplicationID.ToString();
                MessageBox.Show("Data Saved Successfully!", "Schedule Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            else
            {
                MessageBox.Show("Data is not Saved Successfully!", "Schedule Test", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        

        
    }
}
