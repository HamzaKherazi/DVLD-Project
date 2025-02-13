using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsTestType
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }
        public enTestType ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fees { get; set; }



        public clsTestType(enTestType iD, string testTypeTitle, string testTypeDescription, decimal testTypeFees)
        {
            this.ID = iD;
            this.Title = testTypeTitle;
            this.Description = testTypeDescription;
            this.Fees = testTypeFees;
        }



        static public DataTable TestTypesList()
        {
            return clsTestTypeData.TestTypesList();
        }

        static public clsTestType Find(enTestType ID)
        {
            string testTypeTitle = "", testTypeDescription = "";
            decimal testTypeFees = 0;

            if (clsTestTypeData.GetTestTypeInfo(Convert.ToInt16(ID), ref testTypeTitle, ref testTypeDescription, ref testTypeFees))
            {
                return new clsTestType(ID, testTypeTitle, testTypeDescription, testTypeFees);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            return clsTestTypeData.UpdateTestType(Convert.ToInt16(this.ID), this.Title, this.Description, this.Fees);
        }
    }
}
