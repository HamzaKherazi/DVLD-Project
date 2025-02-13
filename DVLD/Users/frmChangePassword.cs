using DVLDBusinessLayer;
using Guna.UI2.WinForms;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmChangePassword : Form
    {

        private int _UserID;
        private clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _User = clsUser.Find(_UserID);

            if(_User == null)
            {
                MessageBox.Show("User Not found!");
                this.Close();

                return;
            }

            ctrlUserCard1.LoadUserInfo(_UserID);

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


        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbConfirmPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbConfirmPassword, "This field is required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbConfirmPassword, null);

            }

            if (tbNewPassword.Text.Trim() != tbConfirmPassword.Text.Trim())
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

        private void tbCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbCurrentPassword, "This field is required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbCurrentPassword, null);

            }

            if (tbCurrentPassword.Text != _User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(tbCurrentPassword, "Current Password is wrong!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbCurrentPassword, null);

            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                return;
            }

            _User.Password = tbNewPassword.Text;

            if(_User.Save())
            {
                MessageBox.Show("Password changed successfully!", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
            else
            {
                MessageBox.Show("Failed changing Password!", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
