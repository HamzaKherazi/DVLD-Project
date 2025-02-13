using DVLD.Global_Classes;
using DVLD.Licenses;
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
    public partial class frmReplacementForDamagedOrLostLicense : Form
    {
        public frmReplacementForDamagedOrLostLicense()
        {
            InitializeComponent();
        }

        private clsLicense.enIssueReason _IssueReason = clsLicense.enIssueReason.ReplacementForDamaged;
        private int _NewLicenseID;

        private void frmReplacementForDamagedOrLostLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = clsCurrentUser.CurrentUser.UserName;
            rbDamagedLicense.Checked = true;
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;
            
            if(LicenseID == -1)
            {
                btnIssueReplacement.Enabled = false;
                return;
            }

            llShowLicenseHistory.Enabled = true;

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                btnIssueReplacement.Enabled = false;
                MessageBox.Show("Selected License is not active, Choose another one!", "Not active", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssueReplacement.Enabled = true;

        }

        private void chbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = (rbDamagedLicense.Checked) ? "Replacement For Damaged License" : "Replacement For Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = (rbDamagedLicense.Checked) ? clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).ApplicationFees.ToString() : clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense).ApplicationFees.ToString();
            _IssueReason = (rbDamagedLicense.Checked) ? clsLicense.enIssueReason.ReplacementForDamaged : clsLicense.enIssueReason.ReplacementForLost;
        }

        private void chbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = (rbDamagedLicense.Checked) ? "Replacement For Damaged License" : "Replacement For Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = (rbDamagedLicense.Checked) ? clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).ApplicationFees.ToString() : clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense).ApplicationFees.ToString();
            _IssueReason = (rbDamagedLicense.Checked) ? clsLicense.enIssueReason.ReplacementForDamaged : clsLicense.enIssueReason.ReplacementForLost;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to issue a replacement for the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IssueReplacement(_IssueReason, clsCurrentUser.CurrentUser.UserID);
            if(NewLicense == null)
            {
                MessageBox.Show("Failed to issue replacement for this license!","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
              
            _NewLicenseID = NewLicense.LicenseID;

            lblRLApplicationID.Text = NewLicense.ApplicationID.ToString();
            lblReplacedLicenseID.Text = _NewLicenseID.ToString();
            lblOldLicenseID.Text = ctrlDriverLicenseInfoWithFilter1.LicenseID.ToString();

            MessageBox.Show("License replaced successfully with ID: " + NewLicense.LicenseID, "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            llShowLicenseHistory.Enabled = true;
            llShowNewLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            gbReplacementFor.Enabled = false;
            btnIssueReplacement.Enabled = false;



        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }
    }
}
