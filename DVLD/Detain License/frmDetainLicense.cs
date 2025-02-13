using DVLD.Drivers;
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

namespace DVLD.Detain_License
{
    public partial class frmDetainLicense : Form
    {

        int _LicenseID;

        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblCreatedBy.Text = clsCurrentUser.CurrentUser.UserName;
        }
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            
            if(_LicenseID == -1)
            {
                return;
            }

            lblLicenseID.Text = _LicenseID.ToString();
            llShowLicenseHistory.Enabled = true;
            llShowLicenseInfo.Enabled = true;

            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected license is already detained, choose another one!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnDetain.Enabled = true;
            txtFineFees.Enabled = true;
            txtFineFees.Focus();

             

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFineFees, null);

            }
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                return;

            }

            if(MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            int DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(Convert.ToDecimal(txtFineFees.Text), clsCurrentUser.CurrentUser.UserID);

            if( DetainID == -1)
            {
                MessageBox.Show("Detain failed!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblDetainID.Text = DetainID.ToString();
            btnDetain.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            txtFineFees.Enabled = false;
            MessageBox.Show("License is detained successfully with ID:" + DetainID, "License Detained", MessageBoxButtons.OK);

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        
    }
}
