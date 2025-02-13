using DVLD.Drivers;
using DVLD.Global_Classes;
using DVLD.International_Licenses;
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

namespace DVLD.Applications
{
    public partial class frmIssueInternationalLicense : Form
    {
        private int _InternationalLicenseID = -1;
        public frmIssueInternationalLicense()
        {
            InitializeComponent();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;

            if (LicenseID == -1 )
            {
                return;
            }

            lblLocalLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID.ToString();
            llShowLicenseHistory.Enabled = true;
            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassID != 3)
            {
                MessageBox.Show("License should be of the 3rd Class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("The License is not active!", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate<DateTime.Now)
            {
                MessageBox.Show("The License is expired!", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(clsInternationalLicense.GetActiveInternationalLicense(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID)!=-1)
            {
                MessageBox.Show("This person has already an active international license!", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssue.Enabled = true;


        }

        private void frmIssueInternationalLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalDrivingLicense).ApplicationFees.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToString("dd / MMM / yyyy");
            lblCreatedBy.Text = clsCurrentUser.CurrentUser.UserName;


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private clsInternationalLicense _IssueInternationalLicense()
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).ApplicationFees;
            Application.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

            if(!Application.Save())
            {
                return null;
            }

            clsInternationalLicense InternationalLicense = new clsInternationalLicense();
            InternationalLicense.ApplicationID = Application.ApplicationID;
            InternationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            InternationalLicense.IsActive = true;
            InternationalLicense.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

            if(!InternationalLicense.Save())
            {
                return null;
            }

            return InternationalLicense;

        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)  == DialogResult.No)
            {
                return;
            }

            clsInternationalLicense InternationalLicense = _IssueInternationalLicense();

            if (InternationalLicense == null)
            {
                MessageBox.Show("Failed to issue the international license!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblILApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            lblILicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            llShowNewLicenseInfo.Enabled = true;
            btnIssue.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

            MessageBox.Show("International License issue successfully with ID:"+ InternationalLicense.InternationalLicenseID, "Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;

        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInternationalLicenseInfo frm = new frmInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();

        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(_InternationalLicenseID);
            frm.ShowDialog();

        }
    }
}
