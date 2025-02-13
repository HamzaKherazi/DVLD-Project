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

namespace DVLD
{
    public partial class ctrlPersonCard : UserControl
    {
        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        private int _PersonID = -1;
        public int PersonID
        {
            get
            {
                return _PersonID;
            }
        }


        private clsPerson _Person;
        public clsPerson Person
        {
            get
            {
                return _Person;
            }
        }

        private void _LoadPersonImage()
        {
            if (_Person.ImagePath != null)
            {
                if (File.Exists(_Person.ImagePath))
                {
                    pbImage.ImageLocation = _Person.ImagePath;

                }
                else
                {
                    MessageBox.Show("Could not find this image!");
                }
            }
            else
            {
                if (_Person.Gendor == 0)
                {
                    pbImage.Image = Resources.Male_512;
                }
                else
                {
                    pbImage.Image = Resources.Female_512;

                }
            }
        }

        private void _FillPersonInfo()
        {
            _PersonID = _Person.PersonID;

            lblPersonID.Text = _Person.PersonID.ToString();
            lblName.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
            lblNationalNo.Text = _Person.NationalNo.ToString();
            if (_Person.Gendor == 0)
            {
                lblGendor.Text = "Male";
                pbGendor.Image = Resources.Man_32;
            }
            else
            {
                lblGendor.Text = "Female";
                pbGendor.Image = Resources.Woman_32;

            }

            if (_Person.Email != null)
            {
                lblEmail.Text = _Person.Email;
            }
            else
            {
                lblEmail.Text = "";
            }

            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToString("dd/MM/yyyy");
            lblPhone.Text = _Person.Phone;
            lblCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;

            _LoadPersonImage();
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if(_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show($"Person with ID: {PersonID} not found!");
                return;
            }

            _FillPersonInfo();
            
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show($"Person with National No: {NationalNo} not found!");
                return;
            }

            _FillPersonInfo();

        }

        public void ResetPersonInfo()
        {
            lblPersonID.Text = "???";
            lblName.Text = "???";
            lblNationalNo.Text = "???";
            lblGendor.Text= "???";
            lblEmail.Text= "???";
            lblAddress.Text= "???";
            lblDateOfBirth.Text= "???";
            lblPhone.Text= "???";
            lblCountry.Text= "???";
            pbGendor.Image = Resources.Man_32;
            pbImage.Image = Resources.Male_512;
        }

        private void lblEditPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_Person.PersonID);
            frm.ShowDialog();
            LoadPersonInfo(_Person.PersonID);
        }
    }
}
