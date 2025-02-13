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
    public partial class frmRenewLicense : Form
    {
        public frmRenewLicense()
        {
            InitializeComponent();
        }

        private int _NewLicenseID = -1;

        private void frmRenewLicense_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();

            lblApplicationID.Text = "???";
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblRenewedLicenseID.Text = "???";
            lblOldLicenseID.Text = "???";
            lblExpirationDate.Text = "???";
            lblCreatedByUser.Text = clsCurrentUser.CurrentUser.UserName;
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees.ToString();
            lblLicenseFees.Text = "$$$";
            lblTotalFees.Text = "$$$";
            
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;
            if(LicenseID == -1)
            {
                llShowLicenseHistory.Enabled = false;
                llShowNewLicenseInfo.Enabled = false;
                btnRenew.Enabled = false;
                return;
            }

            lblLicenseFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.ClassFees.ToString();
            lblOldLicenseID.Text = LicenseID.ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.DefaultValidityLength).ToString("dd/MMM/yyyy");
            lblTotalFees.Text = (Convert.ToDecimal(lblApplicationFees.Text)+ Convert.ToDecimal(lblLicenseFees.Text)).ToString();
            llShowLicenseHistory.Enabled = true;
            llShowNewLicenseInfo.Enabled = true;

            //Check if the license is not expired
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate>DateTime.Now)
            {
                MessageBox.Show("Selected License is not yet expired, it will be on: " + ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate.ToString("dd/mm/yyyy"), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;
            }

            //Check if the license is not active
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not active", "Not Active", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;
            }


            btnRenew.Enabled = true;
            
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            _NewLicenseID = -1;

            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text, clsCurrentUser.CurrentUser.UserID);

            if(NewLicense == null)
            {
                MessageBox.Show("Failed Renewing License!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _NewLicenseID = NewLicense.LicenseID;

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            lblRenewedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("License renewed successfully with ID: " + _NewLicenseID, "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnRenew.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowNewLicenseInfo.Enabled = true;

        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }
    }
}
