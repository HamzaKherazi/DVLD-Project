using DVLD.Global_Classes;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                return;
            }

            clsUser User = null;

            if(clsUser.IsExist(tbUsername.Text, tbPassword.Text))
            {
                User = clsUser.Find(tbUsername.Text);

                if (!User.IsActive)
                {
                    MessageBox.Show("Your account is not active! Contact Admin!", "User Not Active", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsCurrentUser.CurrentUser = User;

                if(chbRememberMe.Checked)
                {
                    clsUtil.SaveLogedInUserInTextFile(tbUsername.Text, tbPassword.Text);
                }

                tbUsername.Text = "";
                tbPassword.Text = "";
                chbRememberMe.Checked = false;
                this.Hide();
                frmMain frm = new frmMain(this);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Invalid Username/Password!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string Username="", Password="";

            if(clsUtil.GetUserCredentials(ref Username, ref Password))
            {
                tbUsername.Text = Username;
                tbPassword.Text= Password;
                chbRememberMe.Checked = true;
            }
        }

        
    }
}
