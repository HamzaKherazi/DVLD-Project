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

namespace DVLD.Users
{
    public partial class ctrlUserCard : UserControl
    {

        clsUser _User;
        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public clsUser User
        {
            get
            {
                return _User;
            }
        }

        private void _ResetUserInfo()
        {
            ctrlPersonCard1.ResetPersonInfo();

            lblUserID.Text = "???";
            lblUserName.Text = "???";
            lblIsActive.Text = "???";
        }

        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.Find(UserID);

            if (_User == null)
            {
                _ResetUserInfo();
                MessageBox.Show($"User with ID: {UserID} not found!");
                return;
            }

            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);

            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName.ToString();
            lblIsActive.Text = _User.IsActive ? "Yes" : "No";
            
            
        }

    }
}
