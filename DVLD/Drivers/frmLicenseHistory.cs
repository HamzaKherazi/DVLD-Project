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
    public partial class frmLicenseHistory : Form
    {
        private int _PersonID = -1;
        public frmLicenseHistory()
        {
            InitializeComponent();
        }

        public frmLicenseHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {
            if(_PersonID != -1)
            {
                ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
                ctrlDriverLicenses1.LoadLicenseHistory(_PersonID);
                ctrlPersonCardWithFilter1.FilterEnabled = false;


            }
            else
            {
                ctrlPersonCardWithFilter1.FilterEnabled = true;

            }

        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            int PersonID = obj;
            if(PersonID == -1)
            {
                ctrlDriverLicenses1.Clear();
            }
            ctrlDriverLicenses1.LoadLicenseHistory(PersonID);
        }
    }
}
