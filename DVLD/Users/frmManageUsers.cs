using DVLD.Users;
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
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();
        }

        private DataTable _dtUsers;

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            _dtUsers = clsUser.UsersList();
            dgvUsers.DataSource = _dtUsers;
            dgvUsers.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                SelectionBackColor = Color.DimGray,
                SelectionForeColor = SystemColors.HighlightText,
                Font = new Font("Segoe UI Semibold", 12.75F),
                WrapMode = DataGridViewTriState.True,
                Alignment = DataGridViewContentAlignment.MiddleLeft,

            };

            if(dgvUsers.Rows.Count>0)
            {
                dgvUsers.Columns[0].Width = 110;
                dgvUsers.Columns[1].Width = 110;
                dgvUsers.Columns[2].Width = 400;
                dgvUsers.Columns[3].Width = 110;
                dgvUsers.Columns[4].Width = 110;
            }
            



            dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvUsers.GridColor = Color.Black;


            lblRecords.Text = _dtUsers.Rows.Count.ToString();
            cbFilter.Text = "None";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Visible = cbFilter.Text != "None" && cbFilter.Text != "Is Active";
            cbIsActive.Visible = cbFilter.Text == "Is Active";
            if(cbIsActive.Visible)
            {
                cbIsActive.Text = "All";
            }

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
                _dtUsers.DefaultView.RowFilter = "";
                lblRecords.Text = dgvUsers.Rows.Count.ToString();
            }

            else
            {
                string FilterColumn = "";
                switch (cbFilter.Text)
                {
                    case "User ID":
                        FilterColumn = "UserID";
                        break;

                    case "UserName":
                        FilterColumn = "UserName";
                        break;

                    case "Person ID":
                        FilterColumn = "PersonID";
                        break;

                    case "Full Name":
                        FilterColumn = "FullName";
                        break;

                }

                if (FilterColumn == "UserID" || FilterColumn == "PersonID")
                {
                    _dtUsers.DefaultView.RowFilter = $"{FilterColumn}={tbFilter.Text}";

                }

                else
                {
                    _dtUsers.DefaultView.RowFilter = $"{FilterColumn} like '{tbFilter.Text}%'";
                }

                lblRecords.Text = _dtUsers.DefaultView.Count.ToString();

            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Person ID" || cbFilter.Text == "User ID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    // Prevent the character from being added to the TextBox
                    e.Handled = true;
                }
            }
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            frmManageUsers_Load(null, null);
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbIsActive.SelectedItem)
            {
                case "Yes":
                    _dtUsers.DefaultView.RowFilter = "IsActive = 1";
                    lblRecords.Text = _dtUsers.DefaultView.Count.ToString();
                    break;

                case "No":
                    _dtUsers.DefaultView.RowFilter = "IsActive = 0";
                    lblRecords.Text = _dtUsers.DefaultView.Count.ToString();

                    break;

                case "All":
                    _dtUsers.DefaultView.RowFilter = "";
                    lblRecords.Text = _dtUsers.DefaultView.Count.ToString();
                    break;
            }
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
            frmManageUsers_Load(null,null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser(Convert.ToInt16(dgvUsers.CurrentRow.Cells[0].Value));
            frm.ShowDialog();
            frmManageUsers_Load(null,null);
            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this user?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(clsUser.Delete(Convert.ToInt16(dgvUsers.CurrentRow.Cells[0].Value)))
                {
                    frmManageUsers_Load(null, null);
                    MessageBox.Show("User deleted successfully!", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Related Data to this User!", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }

        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserDetails frm = new frmUserDetails(Convert.ToInt16(dgvUsers.CurrentRow.Cells[0].Value));
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(Convert.ToInt16(dgvUsers.CurrentRow.Cells[0].Value));
            frm.ShowDialog();
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
