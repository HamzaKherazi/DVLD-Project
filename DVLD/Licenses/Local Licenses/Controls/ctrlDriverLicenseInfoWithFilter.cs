using DVLDBusinessLayer;
using Guna.UI2.WinForms;
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
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {

        public event Action<int> OnLicenseSelected;
        protected virtual void LicenseSelected (int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if(handler != null)
            {
                handler(LicenseID);
            }
        }

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        public bool FilterEnabled
        {
            get
            {
                return gbFilter.Enabled;
            }
            set
            {
                gbFilter.Enabled = value;
            }
        }
        private int _LicenseID;
        public int LicenseID
        {
            get
            {
                return ctrlDriverLicenseInfo1.LicenseID;
            }
        }
        public clsLicense SelectedLicenseInfo
        {
            get
            {
                return ctrlDriverLicenseInfo1.SelectedLicenseInfo;
            }
        }

        public void LoadLicenseInfo(int licenseID)
        {
            txtLicenseID.Text = licenseID.ToString();
            
            ctrlDriverLicenseInfo1.LoadInfo(licenseID);
            _LicenseID = ctrlDriverLicenseInfo1.LicenseID;
            if(OnLicenseSelected != null && FilterEnabled)
                OnLicenseSelected(_LicenseID);
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Prevent the character from being added to the TextBox
                e.Handled = true;
            }

            if(e.KeyChar == (char)13)
            {
                btnSearch.PerformClick();
            }
        }

        public void txtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                txtLicenseID.Focus();
                return;
            }

            LoadLicenseInfo(Convert.ToInt16(txtLicenseID.Text));

        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "Enter License ID!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, null);

            }
        }

    }
}
