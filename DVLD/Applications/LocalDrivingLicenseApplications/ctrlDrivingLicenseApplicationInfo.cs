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

namespace DVLD.Applications.LocalDrivingLicenseApplications
{
    public partial class ctrlLocalDrivingLicenseApplicationInfo : UserControl
    {

        private int _LocalDrivingLicenseApplicationID;
        private int _LicenseID;

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public int LocalDrivingLicenseApplicationID
        {
            get
            {
                return _LocalDrivingLicenseApplicationID;
            }
        }

        public ctrlLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            ctrlApplicationInfo1.ResetApplicationInfo();

            lblLocalDrivingLicenseAppID.Text = "???";
            lblLicenseClass.Text = "???";
            lblPassedTests.Text = "?/3";
            lblShowLicenseInfo.Enabled = false;

        }
        private void _LoadData()
        {
            _LicenseID = -1;
            lblShowLicenseInfo.Enabled = (_LicenseID != -1);

            ctrlApplicationInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID);
            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblPassedTests.Text = clsLocalDrivingLicenseApplication.GetPassedTests(_LocalDrivingLicenseApplicationID).ToString() + "/3";



        }

        public void LoadLocalDrivingLicenseApplicationInfo(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID(LocalDrivingLicenseApplicationID);
            
            if ( _LocalDrivingLicenseApplication == null )
            {
                _ResetLocalDrivingLicenseApplicationInfo();

                MessageBox.Show($"Local Driving Application with ID: {LocalDrivingLicenseApplicationID} Not Found");
                return;
            }

            _LoadData();



        }

        private void lblShowLicenseInfo_Click(object sender, EventArgs e)
        {

        }
    }
}
