using DVLD.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        private int _LicenseID;
        private clsLicense _License;

        public int LicenseID
        {
            get
            {
                return _LicenseID;
            }
        }

        public clsLicense SelectedLicenseInfo
        {
            get
            {
                return _License;
            }
        }

        private void _LoadPersonImage()
        {
            string ImagePath = _License.DriverInfo.PersonInfo.ImagePath;
            if (ImagePath != null)
            {
                if (File.Exists(ImagePath))
                {
                    pbImage.ImageLocation = ImagePath;

                }
                else
                {
                    MessageBox.Show("Could not find this image!");
                }
            }
            else
            {
                if (_License.ApplicationInfo.PersonInfo.Gendor == 0)
                {
                    pbImage.Image = Resources.Male_512;
                }
                else
                {
                    pbImage.Image = Resources.Female_512;

                }
            }
        }

        private void _ResetInfo()
        {
            lblClass.Text = "???";
            lblName.Text = "???";
            lblLicenseID.Text = "???";
            lblNationalNo.Text = "???";

            lblGendor.Text = "???";
            pbGendor.Image = Resources.Man_32;


            lblIssueDate.Text = "???";
            lblIssueReason.Text = "???";
            lblNotes.Text = "???";
            lblIsActive.Text = "???";
            lblDateOfBirth.Text = "???";
            lblDriverID.Text = "???";
            lblExpirationDate.Text = "???";
            lblIsDetained.Text = "???";

            pbImage.Image = Resources.Male_512;

        }
        public void LoadInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = clsLicense.FindByLicenseID(LicenseID);

            if ( _License == null )
            {
                _ResetInfo();
                MessageBox.Show("License not found!", "License Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            lblClass.Text = _License.LicenseClassInfo.ClassName;
            lblName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblNationalNo.Text = _License.DriverInfo.PersonInfo.NationalNo;

            lblGendor.Text = (_License.DriverInfo.PersonInfo.Gendor == 0) ? "Male" : "Female";
            pbGendor.Image = (_License.DriverInfo.PersonInfo.Gendor == 0) ? Resources.Man_32 : Resources.Woman_32;


            lblIssueDate.Text = _License.IssueDate.ToString("dd/mm/yyyy");
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = (_License.Notes != null) ? _License.Notes.ToString() : "No Notes";
            lblIsActive.Text = _License.IsActive? "Yes" : "No";
            lblDateOfBirth.Text = _License.DriverInfo.PersonInfo.DateOfBirth.ToString("dd/mm/yyyy");
            lblDriverID.Text = _License.DriverID.ToString();
            lblExpirationDate.Text = _License.ExpirationDate.ToString("dd/mm/yyyy");
            lblIsDetained.Text = _License.IsDetained? "Yes" : "No";

            _LoadPersonImage();


        }
       
    }
}
