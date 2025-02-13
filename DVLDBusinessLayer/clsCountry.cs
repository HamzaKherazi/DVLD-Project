using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public clsCountry(int countryID, string countryName)
        { 
            this.CountryID = countryID;
            this.CountryName = countryName;
        }

        public static DataTable CountriesList()
        {
            return clsCountryData.CoutriesList();
        }

        static public clsCountry Find(string countryName)
        {
            int CountryID = -1;

            if (clsCountryData.Find(countryName, ref CountryID))
            {
                return new clsCountry(CountryID, countryName);
            }
            else
            {
                return null;
            }

        }
        static public clsCountry Find(int countryID)
        {
            string countryName = "";

            if (clsCountryData.Find(countryID, ref countryName))
            {
                return new clsCountry(countryID, countryName);
            }
            else
            {
                return null;
            }

        }

    }
}
