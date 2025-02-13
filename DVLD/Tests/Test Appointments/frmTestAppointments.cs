using DVLD.Properties;
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

namespace DVLD.Tests.Test_Appointments
{
    public partial class frmTestAppointments : Form
    {
        private int _LocalDrivingLicenseApplicationID;
        private clsTestAppointment _TestAppointment;
        private clsTestType.enTestType _TestType;


        public frmTestAppointments(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestType = TestType;
        }


        private DataTable _dtTestAppointments;

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    this.Text = "Vision Test Appointments";
                    lblTitle.Text = "Vision Test Appointments";
                    pbTest.Image = Resources.Vision_512;
                    break;

                case clsTestType.enTestType.WrittenTest:
                    this.Text = "Written Test Appointments";
                    lblTitle.Text = "Written Test Appointments";
                    pbTest.Image = Resources.Written_Test_512;
                    break;

                case clsTestType.enTestType.StreetTest:
                    this.Text = "Street Test Appointments";
                    lblTitle.Text = "Street Test Appointments";
                    pbTest.Image = Resources.driving_test_512;
                    break;
            }
        }
        private void frmTestAppointments_Load(object sender, EventArgs e)
        {
            ctrlLocalDrivingLicenseApplicationInfo1.LoadLocalDrivingLicenseApplicationInfo(_LocalDrivingLicenseApplicationID);

            _LoadTestTypeImageAndTitle();

            _dtTestAppointments = clsTestAppointment.GetTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, (int)_TestType);

            dgvAppointments.DataSource = _dtTestAppointments;

            dgvAppointments.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                SelectionBackColor = Color.DimGray,
                SelectionForeColor = SystemColors.HighlightText,
                Font = new Font("Segoe UI Semibold", 12.75F),
                WrapMode = DataGridViewTriState.True,
                Alignment = DataGridViewContentAlignment.MiddleLeft,

            };

            if (dgvAppointments.Rows.Count > 0)
            {
                dgvAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvAppointments.Columns[0].Width = 110;

                dgvAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvAppointments.Columns[1].Width = 140;

                dgvAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvAppointments.Columns[2].Width = 110;

                dgvAppointments.Columns[3].HeaderText = "Is Locked";
                dgvAppointments.Columns[3].Width = 110;

            }




            dgvAppointments.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvAppointments.GridColor = Color.Black;


            lblRecords.Text = _dtTestAppointments.Rows.Count.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            _TestAppointment = clsTestAppointment.GetLastTestAppointment(_LocalDrivingLicenseApplicationID, (int)_TestType);
            
            if(_TestAppointment != null)
            {
                if (!_TestAppointment.IsLocked)
                {
                    MessageBox.Show("Person already has an active appointment for this test, you can not add a new appointment!", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (clsTest.Find(_TestAppointment.TestID).TestResult == true)
                {
                    MessageBox.Show("This Person already passed this test before, you can only retake failed test!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            

            frmScheduleTest Frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType);
            Frm.ShowDialog();
            frmTestAppointments_Load(null, null);


        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleTest frm = new frmScheduleTest((int)dgvAppointments.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmTestAppointments_Load(null, null);

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvAppointments.CurrentRow.Cells[0].Value;
            if(clsTestAppointment.Find(TestAppointmentID).IsLocked)
            {
                MessageBox.Show("Test already taken! Make a new test appointment!", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmTakeTest frm = new frmTakeTest(TestAppointmentID);
            frm.ShowDialog();
            frmTestAppointments_Load(null, null);

        }
    }
}
