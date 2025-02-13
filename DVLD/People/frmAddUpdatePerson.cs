using DVLD.Properties;
using DVLDBusinessLayer;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmAddUpdatePerson : Form
    {
        private clsPerson _Person;
        private int _PersonID;
        private enum enMode { AddNew, Update };
        private enMode _Mode = enMode.AddNew;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _PersonID = PersonID;
        }
        
        

        private void tbNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbNationalNo, "This field is required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbNationalNo, null);

            }

            if (tbNationalNo.Text !=_Person.NationalNo && clsPerson.IsExist(tbNationalNo.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbNationalNo, "National No is used for another person!");
            }

            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbNationalNo, string.Empty);
            }


        }

        private void tbEmail_Validating(object sender, CancelEventArgs e)
        {
            if (tbEmail.Text != "" && !clsValidation.ValidateEmail(tbEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbEmail, "Invalid Email Address format!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbEmail, string.Empty);

            }
        }

        private void _FillCountriesInCombobox()
        {
            foreach (DataRow row in clsCountry.CountriesList().Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _ResetDefaultValues()
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);

            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            rbMale.Checked = true;

            cbCountry.SelectedItem = "Morocco";
            _Person = new clsPerson();

        }

        private void _LoadData()
        {
            lblTitle.Text = "Update Person";

            _Person = clsPerson.Find(_PersonID); 

            lblPersonID.Text = _Person.PersonID.ToString();
            tbFirstName.Text = _Person.FirstName;
            tbSecondName.Text = _Person.SecondName;
            tbThirdName.Text = _Person.ThirdName;
            tbLastName.Text = _Person.LastName;
            tbNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.Gendor == 0)
            {
                rbMale.Checked = true;
            }
            else
            {
                rbFemale.Checked = true;
            }

            tbPhone.Text = _Person.Phone;
            tbEmail.Text = _Person.Email;
            cbCountry.SelectedItem = _Person.CountryInfo.CountryName;
            if (_Person.Address != null)
            {
                tbAddress.Text = _Person.Address;


            }
            if (_Person.ImagePath != null)
            {
                pbImage.ImageLocation = _Person.ImagePath;
                lblRemove.Visible = true;
            }
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _FillCountriesInCombobox();

            if (_Mode == enMode.AddNew)
            {
                _ResetDefaultValues();

            }

            else
            {
                _LoadData();

            }
        }

        

        private void lblSetImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff";
            openFileDialog1.Title = "Select an Image";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if(openFileDialog1.FileName!="")
                {
                    pbImage.ImageLocation = openFileDialog1.FileName;
                    lblRemove.Visible = true;
                }
                                
            }

        }
        private bool _HandlePersonImage()
        {
            if (_Person.ImagePath != pbImage.ImageLocation)
            {
                if(_Person.ImagePath!=null)
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch(Exception ex)
                    {
                        
                    }
                }

                if(pbImage.ImageLocation!=null)
                {
                    string SourceFile = pbImage.ImageLocation.ToString();

                    if(clsUtil.CopyToImagesFolder(ref SourceFile))
                    {
                        pbImage.ImageLocation = SourceFile;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!_HandlePersonImage())
            {
                return;
            }

            _Person.NationalNo = tbNationalNo.Text;
            _Person.FirstName = tbFirstName.Text;
            _Person.SecondName = tbSecondName.Text;
            _Person.ThirdName = tbThirdName.Text;
            _Person.LastName = tbLastName.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;

            if (rbMale.Checked)
            {
                _Person.Gendor = 0;
            }
            else
            {
                _Person.Gendor = 1;

            }

            _Person.Phone = tbPhone.Text;
            _Person.Email = tbEmail.Text;
            _Person.NationalityCountryID = clsCountry.Find(cbCountry.Text).CountryID;
            _Person.Address = tbAddress.Text;

            if(pbImage.ImageLocation!=null)
            {
                _Person.ImagePath = pbImage.ImageLocation.ToString();
            }
            else
            {
                _Person.ImagePath = null;
            }

            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();

                lblTitle.Text = "Update Person";
                _Mode = enMode.Update;

                MessageBox.Show("Data saved successfully!", "Add New Person", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
               
                
            }
        }


        private void lblRemove_Click(object sender, EventArgs e)
        {
            
            pbImage.ImageLocation = null;

            if(rbMale.Checked)
            {
                pbImage.Image = Resources.Male_512;
            }
            else
            {
                pbImage.Image = Resources.Female_512;

            }

            lblRemove.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            if(pbImage.ImageLocation == null)
            {
                pbImage.Image = Properties.Resources.Male_512;

            }

        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            if (pbImage.ImageLocation == null)
            {
                pbImage.Image = Properties.Resources.Female_512;

            }

        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            Guna2TextBox tb = (Guna2TextBox)sender;
            if (string.IsNullOrEmpty(tb.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tb, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tb, null);

            }
        }

        
    }
}
