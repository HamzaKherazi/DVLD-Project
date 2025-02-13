using DVLD.International_Licenses;
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

namespace DVLD.Drivers
{
    public partial class ctrlDriverLicenses : UserControl
    {
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        private int _DriverID;
        private clsDriver _Driver;
        private DataTable _dtLocalLicenses;
        private DataTable _dtInternationalLicenses;

        public int DriverID
        {
            get
            {
                return _DriverID;
            }
        }

        public clsDriver DriverInfo
        {
            get
            {
                return _Driver;
            }
        }

        void _LoadLocalLicenses()
        {
            _dtLocalLicenses = clsLicense.LicensesListForDriverID(DriverID);
            dgvLocalLicenses.DataSource = _dtLocalLicenses;
            dgvLocalLicenses.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                SelectionBackColor = Color.DimGray,
                SelectionForeColor = SystemColors.HighlightText,
                Font = new Font("Segoe UI Semibold", 12.75F),
                WrapMode = DataGridViewTriState.True,
                Alignment = DataGridViewContentAlignment.MiddleLeft,

            };

            dgvLocalLicenses.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvLocalLicenses.GridColor = Color.Black;

            if (dgvLocalLicenses.Rows.Count > 0)
            {
                dgvLocalLicenses.Columns[0].HeaderText = "Lic. ID";
                dgvLocalLicenses.Columns[0].Width = 120;

                dgvLocalLicenses.Columns[1].HeaderText = "App. ID";
                dgvLocalLicenses.Columns[1].Width = 120;

                dgvLocalLicenses.Columns[2].HeaderText = "Class Name";
                dgvLocalLicenses.Columns[2].Width = 250;


                dgvLocalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicenses.Columns[3].Width = 150;

                dgvLocalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicenses.Columns[4].Width = 150;


                dgvLocalLicenses.Columns[5].HeaderText = "Is Active";
                dgvLocalLicenses.Columns[5].Width = 120;
            }

            lblLocalRecords.Text = _dtLocalLicenses.Rows.Count.ToString();
        }
        void _LoadInternationalLicenses()
        {
            _dtInternationalLicenses = clsInternationalLicense.LicensesListForDriverID(DriverID);
            dgvInternationalLicenses.DataSource = _dtInternationalLicenses;
            dgvInternationalLicenses.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                SelectionBackColor = Color.DimGray,
                SelectionForeColor = SystemColors.HighlightText,
                Font = new Font("Segoe UI Semibold", 12.75F),
                WrapMode = DataGridViewTriState.True,
                Alignment = DataGridViewContentAlignment.MiddleLeft,

            };

            dgvInternationalLicenses.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvInternationalLicenses.GridColor = Color.Black;

            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 150;

                dgvInternationalLicenses.Columns[1].HeaderText = "App. ID";
                dgvInternationalLicenses.Columns[1].Width = 150;

                dgvInternationalLicenses.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[2].Width = 150;


                dgvInternationalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[3].Width = 200;

                dgvInternationalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[4].Width = 200;


                dgvInternationalLicenses.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[5].Width = 150;
            }


            lblInternationalRecords.Text = _dtInternationalLicenses.Rows.Count.ToString();
        }

        public void LoadLicenseHistoryByDriverID(int DriverID)
        {
            _Driver = clsDriver.FindByDriverID(DriverID);
            
            if(_Driver == null)
            {
                MessageBox.Show("There is no driver with ID: " + _DriverID, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _DriverID = DriverID;
            _LoadLocalLicenses();
            _LoadInternationalLicenses();

        }
        public void LoadLicenseHistory(int PersonID)
        {
            _Driver = clsDriver.FindByPersonID(PersonID);

            if (_Driver == null)
            {
                MessageBox.Show("There is no driver with ID: " + _DriverID, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _DriverID = _Driver.DriverID;
            _LoadLocalLicenses();
            _LoadInternationalLicenses();

        }


        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo((int)dgvLocalLicenses.SelectedRows[0].Cells[0].Value);
            frm.ShowDialog();
        }

        public void Clear()
        {
            _dtLocalLicenses.Clear();
            _dtInternationalLicenses.Clear();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseInfo frm = new frmInternationalLicenseInfo((int)dgvInternationalLicenses.SelectedRows[0].Cells[0].Value);
            frm.ShowDialog();
        }
    }
}
