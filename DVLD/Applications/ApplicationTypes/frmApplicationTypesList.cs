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
    public partial class frmApplicationTypesList : Form
    {
        public frmApplicationTypesList()
        {
            InitializeComponent();
        }

        private DataTable _dtApplicationTypes;

        private void frmApplicationTypesList_Load(object sender, EventArgs e)
        {
            _dtApplicationTypes = clsApplicationType.ApplicationTypesList();

            dgvApplicationTypes.DataSource = _dtApplicationTypes;

            dgvApplicationTypes.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.DimGray,
                ForeColor = Color.White,
                SelectionBackColor = Color.DimGray,
                SelectionForeColor = SystemColors.HighlightText,
                Font = new Font("Segoe UI Semibold", 12.75F),
                WrapMode = DataGridViewTriState.True,
                Alignment = DataGridViewContentAlignment.MiddleLeft,

            };

            if (dgvApplicationTypes.Rows.Count > 0)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "ID";
                dgvApplicationTypes.Columns[0].Width = 110;

                dgvApplicationTypes.Columns[1].HeaderText = "Title";
                dgvApplicationTypes.Columns[1].Width = 500;

                dgvApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvApplicationTypes.Columns[2].Width = 110;

            }




            dgvApplicationTypes.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvApplicationTypes.GridColor = Color.Black;


            lblRecords.Text = _dtApplicationTypes.Rows.Count.ToString();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditApplicationType frm = new frmEditApplicationType(Convert.ToInt16(dgvApplicationTypes.CurrentRow.Cells[0].Value));
            frm.ShowDialog();
            frmApplicationTypesList_Load(null,null);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
