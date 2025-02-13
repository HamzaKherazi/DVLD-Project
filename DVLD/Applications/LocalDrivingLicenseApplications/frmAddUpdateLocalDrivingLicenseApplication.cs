using DVLD.Global_Classes;
using DVLDBusinessLayer;
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

namespace DVLD
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {

        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID;

        enum enMode { AddNew, Update }
        enMode _Mode = enMode.AddNew;


        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;

        }

        public frmAddUpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dtLicenceClasses = clsLicenseClass.LicenseClassesList();

            foreach (DataRow row in dtLicenceClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
            cbLicenseClass.SelectedIndex = 2;
        }

        private void _ResetDefaultValues()
        {
            _FillLicenseClassesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";

                lblApplicationDate.Text = DateTime.Now.ToString();
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicence).ApplicationFees.ToString();
                lblCreatedByUser.Text = clsCurrentUser.CurrentUser.UserName;

                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();

                tpApplicationInfo.Enabled = false;
            }


            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
              
            }


        }
        private void _LoadData()
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID(_LocalDrivingLicenseApplicationID);

            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show($"No Application with ID: {_LocalDrivingLicenseApplicationID}");
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);

            lblLocalDrivingAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = _LocalDrivingLicenseApplication.ApplicationDate.ToString();
            cbLicenseClass.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblApplicationFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedByUser.Text = clsUser.Find(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;

            btnNext.Enabled = false;
        }


        private void frmLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a person!", "New Local Driving License Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tpApplicationInfo.Enabled = true;

            tcLocalDrivingLicenseApplicationInfo.SelectTab(tpApplicationInfo);
            btnSave.Enabled = true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                return;

            }

            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(ctrlPersonCardWithFilter1.Person.PersonID, clsApplication.enApplicationType.NewDrivingLicence, LicenseClassID);

            if(ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another license class. The selected person already have an active application for the selected class!", "Local Driving License Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(clsLicense.IsLicenseExistsByPersonID(ctrlPersonCardWithFilter1.Person.PersonID, LicenseClassID))
            {
                MessageBox.Show("Person alreasy have a license with the same applied driving license", "Local Driving License Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.Person.PersonID;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = (int) clsApplication.enApplicationType.NewDrivingLicence;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToDecimal(lblApplicationFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;

            if (_LocalDrivingLicenseApplication.Save())
            {
                lblLocalDrivingAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();

                _Mode = enMode.Update;
                
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                
                MessageBox.Show("Data saved successfully!", "Local Driving License Application", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Failed saving data!", "Local Driving License Application", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
