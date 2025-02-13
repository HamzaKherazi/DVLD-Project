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

namespace DVLD.International_Licenses
{
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }


        private int _InternationalLicenseID;
        private clsInternationalLicense _InternationalLicense;

        public int InternationalLicenseID
        {
            get
            {
                return _InternationalLicenseID;
            }
        }

        public clsInternationalLicense SelectedLicenseInfo
        {
            get
            {
                return _InternationalLicense;
            }
        }

        private void _LoadPersonImage()
        {
            string ImagePath = _InternationalLicense.DriverInfo.PersonInfo.ImagePath;
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
                if (_InternationalLicense.DriverInfo.PersonInfo.Gendor == 0)
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
            lblName.Text = "???";
            lblIntLicenseID.Text = "???";
            lblLicenseID.Text = "???";
            lblNationalNo.Text = "???";
            lblGendor.Text = "???";
            pbGendor.Image = Resources.Man_32;
            lblIssueDate.Text = "???";
            lblApplicationID.Text = "???";
            lblIsActive.Text = "???";
            lblDateOfBirth.Text = "???";
            lblDriverID.Text = "???";
            lblExpirationDate.Text = "???";

            pbImage.Image = Resources.Male_512;

        }
        public void LoadInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            _InternationalLicense = clsInternationalLicense.Find(_InternationalLicenseID);

            if (_InternationalLicense == null)
            {
                _ResetInfo();
                MessageBox.Show("License not found!", "License Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }

            lblName.Text = _InternationalLicense.DriverInfo.PersonInfo.FullName;
            lblIntLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = _InternationalLicense.DriverInfo.PersonInfo.NationalNo;

            lblGendor.Text = (_InternationalLicense.DriverInfo.PersonInfo.Gendor == 0) ? "Male" : "Female";
            pbGendor.Image = (_InternationalLicense.DriverInfo.PersonInfo.Gendor == 0) ? Resources.Man_32 : Resources.Woman_32;
            
            lblIssueDate.Text = _InternationalLicense.IssueDate.ToString("dd/mm/yyyy");
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = _InternationalLicense.DriverInfo.PersonInfo.DateOfBirth.ToString("dd/mm/yyyy");
            lblDriverID.Text = _InternationalLicense.DriverID.ToString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToString("dd/mm/yyyy");

            _LoadPersonImage();


        }

    }
}
