using DVLD.Applications;
using DVLD.Drivers;
using DVLD.Licenses;
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

namespace DVLD.Detain_License
{
    public partial class frmListDetainedLicenses : Form
    {
        private DataTable _dtDetainedLicenses;
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _dtDetainedLicenses = clsDetainedLicense.DetainedLicensesList();
            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;
            dgvDetainedLicenses.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                SelectionBackColor = Color.DimGray,
                SelectionForeColor = SystemColors.HighlightText,
                Font = new Font("Segoe UI Semibold", 12.75F),
                WrapMode = DataGridViewTriState.True,
                Alignment = DataGridViewContentAlignment.MiddleLeft,

            };
            //dgvPeople.BorderStyle = BorderStyle.FixedSingle;

            dgvDetainedLicenses.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvDetainedLicenses.GridColor = Color.Black;

            if (dgvDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "Detain ID";
                dgvDetainedLicenses.Columns[0].Width = 100;

                dgvDetainedLicenses.Columns[1].HeaderText = "License ID";
                dgvDetainedLicenses.Columns[1].Width = 100;

                dgvDetainedLicenses.Columns[2].HeaderText = "Detain Date";
                dgvDetainedLicenses.Columns[2].Width = 150;


                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 100;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 100;


                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 150;

                dgvDetainedLicenses.Columns[6].HeaderText = "National No.";
                dgvDetainedLicenses.Columns[6].Width = 110;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 200;

                dgvDetainedLicenses.Columns[8].HeaderText = "Release App. ID";
                dgvDetainedLicenses.Columns[8].Width = 150;
            }


            lblRecords.Text = _dtDetainedLicenses.Rows.Count.ToString();
            cbFilter.Text = "None";
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseLicense frm = new frmReleaseLicense();
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Visible = cbFilter.Text != "None" && cbFilter.Text != "Is Released";
            cbIsReleased.Visible = cbFilter.Text == "Is Released";
            if (cbIsReleased.Visible)
            {
                cbIsReleased.Text = "All";
            }

            if (tbFilter.Visible)
            {
                tbFilter.Text = "";
                tbFilter.Focus();
            }

            _dtDetainedLicenses.DefaultView.RowFilter = "";

        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter.Text == "" || cbFilter.Text == "None")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = _dtDetainedLicenses.Rows.Count.ToString();
            }

            else
            {
                string FilterColumn = "";
                switch (cbFilter.Text)
                {
                    case "Detain ID":
                        FilterColumn = "DetainID";
                        break;

                    case "Is Released":
                        FilterColumn = "IsReleased";
                        break;

                    case "National No.":
                        FilterColumn = "NationalNo";
                        break;

                    case "Full Name":
                        FilterColumn = "FullName";
                        break;

                    case "Release Application ID":
                        FilterColumn = "ReleaseApplicationID";
                        break;

                }

                if (FilterColumn == "DetainID" || FilterColumn == "ReleaseApplicationID")
                {
                    _dtDetainedLicenses.DefaultView.RowFilter = $"{FilterColumn}={tbFilter.Text}";

                }
                else
                {
                    _dtDetainedLicenses.DefaultView.RowFilter = $"{FilterColumn} like '{tbFilter.Text}%'";
                }

                lblRecords.Text = _dtDetainedLicenses.DefaultView.Count.ToString();

            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Detain ID" || cbFilter.Text == "Release Application ID")
            {
                e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsReleased.SelectedItem)
            {
                case "Yes":
                    _dtDetainedLicenses.DefaultView.RowFilter = "IsReleased = 1";
                    lblRecords.Text = _dtDetainedLicenses.DefaultView.Count.ToString();
                    break;

                case "No":
                    _dtDetainedLicenses.DefaultView.RowFilter = "IsReleased = 0";
                    lblRecords.Text = _dtDetainedLicenses.DefaultView.Count.ToString();

                    break;

                case "All":
                    _dtDetainedLicenses.DefaultView.RowFilter = "";
                    lblRecords.Text = _dtDetainedLicenses.DefaultView.Count.ToString();
                    break;
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(dgvDetainedLicenses.SelectedRows[0].Cells[6].Value.ToString());
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);

        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo((int)dgvDetainedLicenses.SelectedRows[0].Cells[1].Value);
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null,null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(clsPerson.Find(dgvDetainedLicenses.SelectedRows[0].Cells[6].Value.ToString()).PersonID);
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseLicense frm = new frmReleaseLicense((int)dgvDetainedLicenses.SelectedRows[0].Cells[1].Value);
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled = !(bool)dgvDetainedLicenses.SelectedRows[0].Cells[3].Value;
        }
    }
}
