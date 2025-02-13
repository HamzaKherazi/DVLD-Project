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

namespace DVLD.Tests
{
    public partial class ctrlTakeTest : UserControl
    {

        private int _TestAppointmentID;
        private clsTestAppointment _TestAppointment;


        public ctrlTakeTest()
        {
            InitializeComponent();
        }

        public void LoadTestInfo(int TestAppointmentID)
        {
            _TestAppointmentID = TestAppointmentID;
            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);

            switch ((clsTestType.enTestType)_TestAppointment.TestTypeID)
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
            gbTest.Text = _TestAppointment.TestTypeInfo.Title;

            lblLocalDrivingLicenseAppID.Text = _TestAppointment.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _TestAppointment.LocalDrivingLicenseApplicationInfo.LicenseClassInfo.ClassName;
            lblName.Text = _TestAppointment.LocalDrivingLicenseApplicationInfo.ApplicantFullName;
            lblTrials.Text = clsTest.GetTestTrials(_TestAppointment.LocalDrivingLicenseApplicationID,(clsTestType.enTestType)_TestAppointment.TestTypeID).ToString();
            lblDate.Text = _TestAppointment.AppointmentDate.ToString();
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            if(_TestAppointment.TestID!=-1)
            {
                lblTestID.Text = _TestAppointment.TestID.ToString();

            }
            else
            {
                lblTestID.Text = "Not Taken Yet";
            }


        }

        private clsTest _Test;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to save? After that you can not change the Pass/Fail results after you save!", "Test", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _Test = new clsTest();
                _Test.TestAppointmentID = _TestAppointmentID;
                _Test.TestResult = rbPass.Checked;
                if(txtNotes.Text !="")
                {
                    _Test.Notes = txtNotes.Text;
                }
                else
                {
                    _Test.Notes = null;
                }
                _Test.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

                if(_Test.Save())
                {
                    MessageBox.Show("Data saved successfully!", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Data not saved successfully!", "Test", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        
    }
}
