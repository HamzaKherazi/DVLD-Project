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
    public partial class frmEditApplicationType : Form
    {

        private int _ApplicationTypeID;
        private clsApplicationType _ApplicationType;


        public frmEditApplicationType(int AppliactionTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = AppliactionTypeID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            lblID.Text = _ApplicationType.ApplicationTypeID.ToString();

            tbTitle.Text = _ApplicationType.ApplicationTypeTitle;

            tbFees.Text = _ApplicationType.ApplicationFees.ToString();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _ApplicationType.ApplicationTypeTitle = tbTitle.Text.Trim();
            _ApplicationType.ApplicationFees = Convert.ToDecimal(tbFees.Text);

            if(_ApplicationType.Save())
            {
                MessageBox.Show("Data saved successfully!", "Update Application Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed saving data!", "Update Application Type", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void tbTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbTitle, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbTitle, null);

            }
        }

        private void tbFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbFees, "This field is required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbFees, null);

            }

            

        }

        private void tbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                // Prevent the character from being added to the TextBox
                e.Handled = true;
            }
        }
    }
}
