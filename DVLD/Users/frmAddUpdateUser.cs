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

namespace DVLD
{
    public partial class frmAddUpdateUser : Form
    {
        clsUser _User;
        private int _UserID;

        enum enMode { AddNew, Update}
        enMode _Mode = enMode.AddNew;

        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _UserID = UserID;
        }

        private void _ResetDefaultValues()
        {
            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();

                tpLoginInfo.Enabled = false;
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;

            }


        }
        private void _LoadData()
        {
            _User = clsUser.Find(_UserID);

            ctrlPersonCardWithFilter1.FilterEnabled = false;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);

            lblUserID.Text = _User.UserID.ToString();
            tbUserName.Text = _User.UserName.ToString();
            tbPassword.Text = _User.Password.ToString();
            tbConfirmPassword.Text = _User.Password.ToString();
            chbIsActive.Checked = _User.IsActive;
            btnNext.Enabled = false;
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if(_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a person!", "Add New User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(clsUser.IsExist(ctrlPersonCardWithFilter1.PersonID))
            {
                MessageBox.Show("Selected person already has a user. Choose another one!", "Add New User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tpLoginInfo.Enabled = true;

            tcUserInfo.SelectTab(tpLoginInfo);
            btnSave.Enabled = true;
            
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void ConfirmPassword(object sender, CancelEventArgs e)
        {
            if(tbPassword.Text != tbConfirmPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(tbConfirmPassword, "Password confirmation does not match password!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbConfirmPassword, null);

            }
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            ValidateEmptyTextBox(sender, e);
            ConfirmPassword(sender, e);


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                return;

            }

            _User.UserName = tbUserName.Text;
            _User.Password = tbPassword.Text;
            _User.IsActive = chbIsActive.Checked;
            _User.PersonID = ctrlPersonCardWithFilter1.Person.PersonID;

            if(_User.Save())
            {
                MessageBox.Show("Data saved successfully!", "Add/Update User", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblUserID.Text = _User.UserID.ToString();

                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                this.Text = "Update User";
            }
            else
            {
                MessageBox.Show("Failed saving data!", "Add/Update User", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void tbUserName_Validating(object sender, CancelEventArgs e)
        {
            if (tbUserName.Text != _User.UserName && clsUser.IsExist(tbUserName.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbUserName, "This username already exists!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbUserName, null);

            }
            ValidateEmptyTextBox(sender, e);
            
        }
    }
}
