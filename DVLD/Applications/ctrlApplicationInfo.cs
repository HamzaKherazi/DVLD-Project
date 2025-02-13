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

namespace DVLD.Applications
{
    public partial class ctrlApplicationInfo : UserControl
    {

        private int _ApplicationID;
        private clsApplication _Application;

        public ctrlApplicationInfo()
        {
            InitializeComponent();
        }

        public void ResetApplicationInfo()
        {
            lblID.Text = "???";
            lblStatus.Text = "???";
            lblFees.Text = "???";
            lblType.Text = "???";
            lblApplicant.Text = "???";
            lblDate.Text = "???";
            lblStatusDate.Text = "???";
            lblCreatedBy.Text = "???";
            
            
        }

        private void _LoadData()
        {
            lblID.Text = _Application.ApplicationID.ToString();
            lblStatus.Text = _Application.StatusText;
            lblFees.Text = _Application.PaidFees.ToString();
            lblType.Text = _Application.ApplicationTypeInfo.ApplicationTypeTitle;
            lblApplicant.Text = _Application.ApplicantFullName;
            lblDate.Text = _Application.ApplicationDate.ToString();
            lblStatusDate.Text = _Application.LastStatusDate.ToString();
            lblCreatedBy.Text = _Application.CreatedByUserInfo.UserName;

        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            _ApplicationID = ApplicationID;

            _Application = clsApplication.FindBaseApplication(ApplicationID);

            ResetApplicationInfo();

            if (_Application == null)
            {
                MessageBox.Show($"Application with ID: {ApplicationID} Not Found");
                return;
            }

            _LoadData();
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(_Application.ApplicantPersonID);
            frm.ShowDialog();

        }
    }
}
