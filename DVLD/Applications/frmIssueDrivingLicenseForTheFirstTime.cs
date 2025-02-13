using DVLD.Global_Classes;
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

namespace DVLD.Licenses
{
    public partial class frmIssueDrivingLicenseForTheFirstTime : Form
    {
        private int _LocalDrivingLicenseApplicationID;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;


        public frmIssueDrivingLicenseForTheFirstTime(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

        }

        private void frmIssueDrivingLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID(_LocalDrivingLicenseApplicationID);

            if(_LocalDrivingLicenseApplication == null )
            {
                MessageBox.Show($"No application with ID: {_LocalDrivingLicenseApplicationID}!", "License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if(!clsLocalDrivingLicenseApplication.HasPassedAllTests(_LocalDrivingLicenseApplicationID))
            {
                MessageBox.Show("The applicant shoul passed all tests!", "License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            int LicenseID = _LocalDrivingLicenseApplication.GetActiveLicense();
            if(LicenseID!=-1)
            {
                MessageBox.Show("The applicant has already an active license of this class!", "License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlLocalDrivingLicenseApplicationInfo1.LoadLocalDrivingLicenseApplicationInfo(_LocalDrivingLicenseApplicationID);


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool _AddNewDriver()
        {
            clsDriver Driver = new clsDriver();

            Driver.PersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
            Driver.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;
            Driver.CreatedDate = DateTime.Now;

            return Driver.Save();

        }

        private void btnIssue_Click(object sender, EventArgs e)
        {

            int LicenseID = _LocalDrivingLicenseApplication.IssueForTheFirstTime(txtNotes.Text, clsCurrentUser.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show("License issued successfully with License ID: " + LicenseID, "Issue License", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("License NOT issued successfully!", "Issue License", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }
    }
}
