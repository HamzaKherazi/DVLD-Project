using DVLD.Applications.LocalDrivingLicenseApplications;
using DVLD.Licenses;
using DVLD.Tests.Test_Appointments;
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

namespace DVLD
{
    public partial class frmLocalDrivingLicenseApplicationsList : Form
    {
        public frmLocalDrivingLicenseApplicationsList()
        {
            InitializeComponent();
        }

        private DataTable _dtLDLApplications;

        private void frmLocalDrivingLicenseApplicationsList_Load(object sender, EventArgs e)
        {
            _dtLDLApplications = clsLocalDrivingLicenseApplication.LocalDrivingLicenseApplicationsList();

            dgvLDLApplications.DataSource = _dtLDLApplications;

            dgvLDLApplications.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                SelectionBackColor = Color.DimGray,
                SelectionForeColor = SystemColors.HighlightText,
                Font = new Font("Segoe UI Semibold", 12.75F),
                WrapMode = DataGridViewTriState.True,
                Alignment = DataGridViewContentAlignment.MiddleLeft,

            };

            if (dgvLDLApplications.Rows.Count > 0)
            {
                dgvLDLApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLDLApplications.Columns[0].Width = 110;

                dgvLDLApplications.Columns[1].HeaderText = "Driving Class";
                dgvLDLApplications.Columns[1].Width = 320;

                dgvLDLApplications.Columns[2].HeaderText = "National No.";
                dgvLDLApplications.Columns[2].Width = 110;

                dgvLDLApplications.Columns[3].HeaderText = "Full Name";
                dgvLDLApplications.Columns[3].Width = 400;

                dgvLDLApplications.Columns[4].HeaderText = "Application Date";
                dgvLDLApplications.Columns[4].Width = 170;

                dgvLDLApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLDLApplications.Columns[5].Width = 100;

                dgvLDLApplications.Columns[6].HeaderText = "Status";
                dgvLDLApplications.Columns[6].Width = 150;

            }




            dgvLDLApplications.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvLDLApplications.GridColor = Color.Black;


            lblRecords.Text = _dtLDLApplications.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication((int)dgvLDLApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilter.Text == "None")
            {
                tbFilter.Visible= false;
                cbStatus.Visible= false;

                _dtLDLApplications.DefaultView.RowFilter = "";
                lblRecords.Text = dgvLDLApplications.Rows.Count.ToString();


                return;
            }

            tbFilter.Visible = (cbFilter.Text != "Status");
            cbStatus.Visible = (cbFilter.Text == "Status");

            if (tbFilter.Visible)
            {
                tbFilter.Text = "";
                tbFilter.Focus();
            }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            string filter = "";

            if(tbFilter.Text =="")
            {
                _dtLDLApplications.DefaultView.RowFilter = "";
                lblRecords.Text = dgvLDLApplications.Rows.Count.ToString();
                return;
            }

            switch(cbFilter.Text)
            {
                case "L.D.L.AppID":
                    filter = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    filter = "NationalNo";
                    break;

                case "Full Name":
                    filter = "FullName";
                    break;

            }

            if(cbFilter.Text != "L.D.L.AppID")
            {
                _dtLDLApplications.DefaultView.RowFilter = $"{filter} like '{tbFilter.Text}%'";

            }
            else
            {
                _dtLDLApplications.DefaultView.RowFilter = $"{filter} = {tbFilter.Text}";

            }

            lblRecords.Text = dgvLDLApplications.Rows.Count.ToString();


        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilter.Text == "L.D.L.AppID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    // Prevent the character from being added to the TextBox
                    e.Handled = true;
                }
            }

        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if(cbStatus.Text == "All")
            {
                _dtLDLApplications.DefaultView.RowFilter = "";
            }
            else

            {
                _dtLDLApplications.DefaultView.RowFilter = $"Status = '{cbStatus.Text}'";
            }

            lblRecords.Text = dgvLDLApplications.Rows.Count.ToString();

        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestAppointments frm = new frmTestAppointments((int)dgvLDLApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.VisionTest);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void cmsLocalDrivingLicenseApplication_Opening(object sender, CancelEventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID((int)dgvLDLApplications.CurrentRow.Cells[0].Value);

            editToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            cancelApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            deleteApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus != clsApplication.enApplicationStatus.Completed);
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = clsLocalDrivingLicenseApplication.HasPassedAllTests(LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID) & (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            scheduleTestsToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            
            int ApplicationID = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID((int)dgvLDLApplications.CurrentRow.Cells[0].Value).ApplicationID;
            
            showLicenseToolStripMenuItem.Enabled = clsLicense.FindByApplicationID(ApplicationID)!=null;

            if (scheduleTestsToolStripMenuItem.Enabled)
            {
                switch (clsLocalDrivingLicenseApplication.GetPassedTests(LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID))
                {
                    case 0:
                        scheduleTestsToolStripMenuItem.Enabled = true;
                        scheduleVisionTestToolStripMenuItem.Enabled = true;
                        scheduleWrittenTestToolStripMenuItem.Enabled = false;
                        scheduleStreetTestToolStripMenuItem.Enabled = false;
                        break;

                    case 1:
                        scheduleTestsToolStripMenuItem.Enabled = true;
                        scheduleVisionTestToolStripMenuItem.Enabled = false;
                        scheduleWrittenTestToolStripMenuItem.Enabled = true;
                        scheduleStreetTestToolStripMenuItem.Enabled = false;
                        break;

                    case 2:
                        scheduleTestsToolStripMenuItem.Enabled = true;
                        scheduleVisionTestToolStripMenuItem.Enabled = false;
                        scheduleWrittenTestToolStripMenuItem.Enabled = false;
                        scheduleStreetTestToolStripMenuItem.Enabled = true;
                        break;

                    case 3:
                        scheduleTestsToolStripMenuItem.Enabled = false;
                        //scheduleVisionTestToolStripMenuItem.Enabled = false;
                        //scheduleWrittenTestToolStripMenuItem.Enabled = false;
                        //scheduleStreetTestToolStripMenuItem.Enabled = false;
                        break;
                }

            }

        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo((int)dgvLDLApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplicationsList_Load(null, null);
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this application?", "Delete Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(clsLocalDrivingLicenseApplication.Delete((int)dgvLDLApplications.CurrentRow.Cells[0].Value))
                {
                    frmLocalDrivingLicenseApplicationsList_Load(null, null);
                    MessageBox.Show("Application deleted successfully!", "Delete Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
                }
                else
                {
                    MessageBox.Show("Could not delete the application! Related Data to it!", "Delete Application", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to cancel this application?", "Cancel application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID((int)dgvLDLApplications.CurrentRow.Cells[0].Value);
                
                if(LocalDrivingLicenseApplication != null)
                {
                    if (LocalDrivingLicenseApplication.Cancel())
                    {
                        frmLocalDrivingLicenseApplicationsList_Load(null, null);
                        MessageBox.Show("Application cancelled successfully!", "Cancel application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Could not cancel this application!", "Cancel application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Could not find this application!", "Cancel application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                
            }
            
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestAppointments frm = new frmTestAppointments((int)dgvLDLApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.WrittenTest);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplicationsList_Load(null, null);

        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestAppointments frm = new frmTestAppointments((int)dgvLDLApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.StreetTest);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplicationsList_Load(null, null);

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationID = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseAppID((int)dgvLDLApplications.CurrentRow.Cells[0].Value).ApplicationID;
            int LicenseID = clsLicense.FindByApplicationID(ApplicationID).LicenseID;
            frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDrivingLicenseForTheFirstTime frm = new frmIssueDrivingLicenseForTheFirstTime((int)dgvLDLApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplicationsList_Load(null, null);

        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
            frmLocalDrivingLicenseApplicationsList_Load(null, null);
        }
    }
}
