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
    public partial class frmEditTestType : Form
    {
        clsTestType.enTestType _TestTypeID;
        private clsTestType _TestType;

        public frmEditTestType(clsTestType.enTestType testTypeID)
        {
            InitializeComponent();
            _TestTypeID = testTypeID;
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestType.Find(_TestTypeID);

            lblID.Text =((int)_TestType.ID).ToString();
            tbTitle.Text = _TestType.Title;
            tbDescription.Text = _TestType.Description;
            tbFees.Text = _TestType.Fees.ToString();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                return;
            }

            _TestType.Title = tbTitle.Text;
            _TestType.Description = tbDescription.Text;
            _TestType.Fees = Convert.ToDecimal(tbFees.Text);

            if (_TestType.Save())
            {
                MessageBox.Show("Data saved successfully!", "Update Test Type", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed saving data!", "Update Test Type", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void tbDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(tbDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbDescription, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbDescription, null);

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
