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
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void RefreshPeopleList()
        {
            dgvPeople.DataSource = clsPerson.PeopleList();
            lblRecords.Text = clsPerson.PeopleList().Rows.Count.ToString();

        }

        private DataTable _dtPeople;

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _dtPeople = clsPerson.PeopleList();
            dgvPeople.DataSource = _dtPeople;
            dgvPeople.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
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

            dgvPeople.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvPeople.GridColor = Color.Black;

            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 120;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;

                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;
            }


            lblRecords.Text = _dtPeople.Rows.Count.ToString();
            cbFilter.Text = "None";
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            RefreshPeopleList();

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmAddUpdatePerson frm = new frmAddUpdatePerson(Convert.ToInt16(dgvPeople.CurrentRow.Cells[0].Value));
            frm.ShowDialog();
            RefreshPeopleList();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete Person {dgvPeople.CurrentRow.Cells[0].Value}?", "Delete Person", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (clsPerson.Delete(Convert.ToInt16(dgvPeople.CurrentRow.Cells[0].Value)))
                {
                    RefreshPeopleList();
                    MessageBox.Show("Person deleted successfully!", "Delete Person", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                else
                {
                    MessageBox.Show("Related Data to this Person!", "Delete Person", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }


            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(Convert.ToInt16(dgvPeople.CurrentRow.Cells[0].Value));
            frm.ShowDialog();
            RefreshPeopleList();

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
                _dtPeople.DefaultView.RowFilter = "";
                lblRecords.Text = _dtPeople.Rows.Count.ToString();
            }

            else
            {
                string FilterColumn = "";
                switch (cbFilter.Text)
                {
                    case "Person ID":
                        FilterColumn = "PersonID";
                        break;

                    case "National No":
                        FilterColumn = "NationalNo";
                        break;

                    case "First Name":
                        FilterColumn = "FirstName";
                        break;

                    case "Second Name":
                        FilterColumn = "SecondName";
                        break;

                    case "Third Name":
                        FilterColumn = "ThirdName";
                        break;

                    case "Last Name":
                        FilterColumn = "LastName";
                        break;

                    case "Nationality":
                        FilterColumn = "CountryName";
                        break;

                    case "Phone":
                        FilterColumn = "Phone";
                        break;

                    case "Email":
                        FilterColumn = "Email";
                        break;

                    case "Gendor":
                        FilterColumn = "Gendor";
                        break;


                }

                if (FilterColumn == "PersonID")
                {
                    _dtPeople.DefaultView.RowFilter = $"{FilterColumn}={tbFilter.Text}";

                }
                else
                {
                    _dtPeople.DefaultView.RowFilter = $"{FilterColumn} like '{tbFilter.Text}%'";
                }

                lblRecords.Text = _dtPeople.DefaultView.Count.ToString();

            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilter.Text == "Person ID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    // Prevent the character from being added to the TextBox
                    e.Handled = true;
                }
            }
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            RefreshPeopleList();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented yet!");

        }

        private void phoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented yet!");

        }
    }
}
        

    

