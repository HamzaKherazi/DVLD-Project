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

namespace DVLD.People
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int LicenseID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID);
            }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        public int PersonID
        {
            get
            {
                return ctrlPersonCard1.PersonID;
            }
        }

        public clsPerson Person
        {
            get
            {
                return ctrlPersonCard1.Person;
            }
        }

        public bool FilterEnabled
        {
            get
            {
                return gbFilter.Enabled; 
            }

            set
            {
                gbFilter.Enabled = value;
            }
          
        }

        private int _PersonID;

        public void LoadPersonInfo(int PersonID)
        {
            tbFilter.Text = PersonID.ToString();
            ctrlPersonCard1.LoadPersonInfo(PersonID);
            _PersonID = ctrlPersonCard1.PersonID;
            if (OnPersonSelected != null && FilterEnabled)
                OnPersonSelected(_PersonID);
        }

        public void LoadPersonInfo(string NationalNo)
        {
            ctrlPersonCard1.LoadPersonInfo(NationalNo);


        }



        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbFilter.Text = "";
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Person ID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    // Prevent the character from being added to the TextBox
                    e.Handled = true;
                }
            }

            if(e.KeyChar == (char)13)
            {
                btnSearchPerson.PerformClick();
            }
        }

        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbFilter.Text == "Person ID")
                {
                    ctrlPersonCard1.LoadPersonInfo(Convert.ToInt16(tbFilter.Text));
                }
                else
                {
                    ctrlPersonCard1.LoadPersonInfo(tbFilter.Text);

                }
            }
            catch(Exception ex)
            {
                //error message
            }
            
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
        }
    }
}
