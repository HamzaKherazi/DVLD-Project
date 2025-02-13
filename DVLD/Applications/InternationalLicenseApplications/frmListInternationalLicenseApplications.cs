using DVLD.Drivers;
using DVLD.International_Licenses;
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

namespace DVLD.Applications.InternationalLicenseApplications
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private DataTable _dtILApplications;

        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtILApplications = clsInternationalLicense.AllInternationalLicenses();

            dgvILApplications.DataSource = _dtILApplications;

            dgvILApplications.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                SelectionBackColor = Color.DimGray,
                SelectionForeColor = SystemColors.HighlightText,
                Font = new Font("Segoe UI Semibold", 12.75F),
                WrapMode = DataGridViewTriState.True,
                Alignment = DataGridViewContentAlignment.MiddleLeft,

            };

            if (dgvILApplications.Rows.Count > 0)
            {
                dgvILApplications.Columns[0].HeaderText = "Int.License ID";
                dgvILApplications.Columns[0].Width = 110;

                dgvILApplications.Columns[1].HeaderText = "Application ID";
                dgvILApplications.Columns[1].Width = 110;

                dgvILApplications.Columns[2].HeaderText = "Driver ID";
                dgvILApplications.Columns[2].Width = 110;

                dgvILApplications.Columns[3].HeaderText = "L.License ID";
                dgvILApplications.Columns[3].Width = 110;

                dgvILApplications.Columns[4].HeaderText = "Issue Date";
                dgvILApplications.Columns[4].Width = 170;

                dgvILApplications.Columns[5].HeaderText = " Expiration Date";
                dgvILApplications.Columns[5].Width = 170;

                dgvILApplications.Columns[6].HeaderText = "Is Active";
                dgvILApplications.Columns[6].Width = 110;

            }




            dgvILApplications.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvILApplications.GridColor = Color.Black;


            lblRecords.Text = _dtILApplications.Rows.Count.ToString();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(clsDriver.FindByDriverID((int)dgvILApplications.SelectedRows[0].Cells[2].Value).PersonID);
            frm.ShowDialog();

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseInfo frm = new frmInternationalLicenseInfo((int)dgvILApplications.SelectedRows[0].Cells[0].Value);
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmLicenseHistory frm = new frmLicenseHistory(clsDriver.FindByDriverID((int)dgvILApplications.SelectedRows[0].Cells[2].Value).PersonID);
            frm.ShowDialog();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmIssueInternationalLicense frm = new frmIssueInternationalLicense();
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Visible = cbFilter.Text != "None" && cbFilter.Text != "Is Active";
            cbIsActive.Visible = cbFilter.Text == "Is Active";
            if (cbIsActive.Visible)
            {
                cbIsActive.Text = "All";
            }

            if (tbFilter.Visible)
            {
                tbFilter.Text = "";
                tbFilter.Focus();
            }

            _dtILApplications.DefaultView.RowFilter = "";
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsActive.SelectedItem)
            {
                case "Yes":
                    _dtILApplications.DefaultView.RowFilter = "IsActive = 1";
                    lblRecords.Text = _dtILApplications.DefaultView.Count.ToString();
                    break;

                case "No":
                    _dtILApplications.DefaultView.RowFilter = "IsActive = 0";
                    lblRecords.Text = _dtILApplications.DefaultView.Count.ToString();

                    break;

                case "All":
                    _dtILApplications.DefaultView.RowFilter = "";
                    lblRecords.Text = _dtILApplications.DefaultView.Count.ToString();
                    break;
            }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter.Text == "" || cbFilter.Text == "None")
            {
                _dtILApplications.DefaultView.RowFilter = "";
                lblRecords.Text = _dtILApplications.Rows.Count.ToString();
            }

            else
            {
                string FilterColumn = "";
                switch (cbFilter.Text)
                {
                    case "International License ID":
                        FilterColumn = "InternationalLicenseID";
                        break;

                    case "Application ID":
                        FilterColumn = "ApplicationID";
                        break;

                    case "Driver ID":
                        FilterColumn = "DriverID";
                        break;

                    case "Local License ID":
                        FilterColumn = "IssuedUsingLocalLicenseID";
                        break;

                    case "Is Active":
                        FilterColumn = "IsActive";
                        break;

                }

                if (FilterColumn!= "IsActive" && FilterColumn != "")
                {
                    _dtILApplications.DefaultView.RowFilter = $"{FilterColumn}={tbFilter.Text}";

                }
                

                lblRecords.Text = _dtILApplications.DefaultView.Count.ToString();

            }
        }
    }
}
