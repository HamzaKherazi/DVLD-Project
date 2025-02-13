using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DVLDBusinessLayer
{
    public class clsApplicationType
    {   
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees { get; set; }

        public clsApplicationType(int applicationTypeID, string applicationTypeTitle, decimal applicationFees)
        {
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTypeTitle = applicationTypeTitle;
            this.ApplicationFees = applicationFees;
        }

        static public DataTable ApplicationTypesList()
        {
            return clsApplicationTypeData.ApplicationTypesList();
        }

        static public clsApplicationType Find(int applicationTypeID)
        {
            string applicationTypeTitle = "";
            decimal applicationFees = 0;

            if(clsApplicationTypeData.GetApplicationTypeInfo(applicationTypeID, ref  applicationTypeTitle, ref applicationFees))
            {
                return new clsApplicationType(applicationTypeID,  applicationTypeTitle, applicationFees);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees);
        }


    }
}
