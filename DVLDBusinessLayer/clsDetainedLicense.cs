using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsDetainedLicense
    {
        public int DetainID {  get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public clsUser ReleasedByUserInfo { get; set; }
        public int ReleaseApplicationID { get; set; }
        public enum enMode { AddNew, Update}
        public enMode Mode { get; set; }

        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.Now;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;

            this.Mode = enMode.AddNew;
        }

        public clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate,int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.Find(CreatedByUserID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleasedByUserInfo = clsUser.Find(ReleasedByUserID);
            this.ReleaseApplicationID = ReleaseApplicationID;

            this.Mode = enMode.Update;
        }

        static public DataTable DetainedLicensesList()
        {
            return clsDetainedLicenseData.DetainedLicensesList();
        }

        static public clsDetainedLicense Find(int LicenseID)
        {
            int DetainID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.Now;
            decimal FineFees = 0;
            bool IsReleased = false;

            if (clsDetainedLicenseData.GetDetainedLicenseInfo(LicenseID, ref DetainID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            }
            else
            {
                return null;
            }
        }

        public bool ReleaseDetainedLicense(int ReleaseByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseLicense(this.DetainID, ReleaseByUserID, ReleaseApplicationID);
        }


    }
}
