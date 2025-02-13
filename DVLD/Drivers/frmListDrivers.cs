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

namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private DataTable _dtDrivers;
        private void RefreshPeopleList()
        {
            dgvDrivers.DataSource = clsDriver.DriversList();
            lblRecords.Text = clsPerson.PeopleList().Rows.Count.ToString();

        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _dtDrivers = clsDriver.DriversList();
            dgvDrivers.DataSource = _dtDrivers;
            dgvDrivers.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
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

            dgvDrivers.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvDrivers.GridColor = Color.Black;

            if (dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 120;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 120;

                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 120;


                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 290;

                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 150;


                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 150;
            }


            lblRecords.Text = _dtDrivers.Rows.Count.ToString();
            cbFilter.Text = "None";
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Visible = (cbFilter.Text != "None");
            if (tbFilter.Visible)
            {
                tbFilter.Text = "";
                tbFilter.Focus();
            }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter.Text == "" || cbFilter.Text == "None")
            {
                _dtDrivers.DefaultView.RowFilter = "";
                lblRecords.Text = _dtDrivers.Rows.Count.ToString();
            }

            else
            {
                string FilterColumn = "";
                switch (cbFilter.Text)
                {
                    case "Driver ID":
                        FilterColumn = "DriverID";
                        break;

                    case "Person ID":
                        FilterColumn = "PersonID";
                        break;

                    case "National No":
                        FilterColumn = "NationalNo";
                        break;

                    case "Full Name":
                        FilterColumn = "FullName";
                        break;

                }

                if (FilterColumn == "PersonID" || FilterColumn == "DriverID")
                {
                    _dtDrivers.DefaultView.RowFilter = $"{FilterColumn}={tbFilter.Text}";

                }
                else
                {
                    _dtDrivers.DefaultView.RowFilter = $"{FilterColumn} like '{tbFilter.Text}%'";
                }

                lblRecords.Text = _dtDrivers.DefaultView.Count.ToString();

            }

        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Person ID" || cbFilter.Text == "DriverID")
            {
                e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails((int)dgvDrivers.SelectedRows[0].Cells[1].Value);
            frm.ShowDialog();
            frmListDrivers_Load(null, null);

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory((int)dgvDrivers.SelectedRows[0].Cells[1].Value);
            frm.ShowDialog();
            frmListDrivers_Load(null, null);

        }
    }
}
