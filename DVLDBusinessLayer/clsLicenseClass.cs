using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsLicenseClass
    {

        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public int MinimumAllowedAge { get; set; }
        public int DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }


        public clsLicenseClass(int licenseClassID, string className, string classDescription, int minimumAllowedAge, int defaultValidityLength, decimal classFees)
        {
            this.LicenseClassID = licenseClassID;
            this.ClassName = className;
            this.ClassDescription = classDescription;
            this.MinimumAllowedAge = minimumAllowedAge;
            this.DefaultValidityLength = defaultValidityLength;
            this.ClassFees = classFees;
        }



        static public DataTable LicenseClassesList()
        {
            return clsLicenseClassData.LicenseClassesList();
        }

        static public clsLicenseClass Find(int  licenseClassID)
        {
            string className = "", classDescription = "";
            int minimumAllowedAge = 18, defaultValidityLength = 1;
            decimal classFees = 0;


            if (clsLicenseClassData.GetLicenseClassInfoByID(licenseClassID, ref className, ref classDescription, ref minimumAllowedAge, ref defaultValidityLength, ref classFees))
            {
                return new clsLicenseClass(licenseClassID, className, classDescription, minimumAllowedAge, defaultValidityLength, classFees);
            }
            else
            {
                return null;
            }
        }

        static public clsLicenseClass Find(string className)
        {
            string classDescription = "";
            int licenseClassID=-1, minimumAllowedAge = 18, defaultValidityLength = 1;
            decimal classFees = 0;


            if (clsLicenseClassData.GetLicenseClassInfoByClassName(className, ref licenseClassID, ref classDescription, ref minimumAllowedAge, ref defaultValidityLength, ref classFees))
            {
                return new clsLicenseClass(licenseClassID, className, classDescription, minimumAllowedAge, defaultValidityLength, classFees);
            }
            else
            {
                return null;
            }
        }

        


    }
}
