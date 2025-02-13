using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsPerson
    {
        public int PersonID {  get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;
            }
        }
        public DateTime DateOfBirth { get; set; }
        public byte Gendor {  get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public clsCountry CountryInfo { get; set; }
        public string ImagePath { get; set; }

        public enum enMode {AddNew, Update}
        public enMode Mode { get; set; }



        public clsPerson()
        {
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now.AddYears(-18);
            this.Gendor = 0;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = null;

            this.Mode = enMode.AddNew;

        }

        public clsPerson(int personID, string nationalNo, string firstName, string secondName, string thirdName,
                                         string lastName, DateTime dateOfBirth, byte gendor, string address,
                                         string phone, string email, int nationalityCountryID, string imagePath)
        {
            this.PersonID = personID;
            this.NationalNo = nationalNo;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gendor = gendor;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.NationalityCountryID = nationalityCountryID;
            this.CountryInfo = clsCountry.Find(this.NationalityCountryID);
            this.ImagePath = imagePath;

            this.Mode = enMode.Update;
        }


        static public DataTable PeopleList()
        {
            return clsPersonData.PeopleList();
        }

        
        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);

            return this.PersonID != -1;
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
        }
        
        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:

                    if(_AddNewPerson())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePerson();
                    
            }

            return false;
            
        }

        public static clsPerson Find(int personID)
        {
            string nationalNo = "", firstName = "", secondName = "", thirdName = "", lastName = "", address = "", phone = "", email = "", imagePath = "";
            int nationalityCountryID = -1;
            DateTime dateOfBirth = DateTime.Now;
            byte gendor = 0;

            if (clsPersonData.GetPersonByID(personID, ref nationalNo, ref firstName, ref secondName, ref thirdName, ref lastName, ref dateOfBirth, ref gendor, ref address, ref phone, ref email, ref nationalityCountryID, ref imagePath))
            {
                return new clsPerson(personID, nationalNo, firstName, secondName, thirdName, lastName, dateOfBirth, gendor, address, phone, email, nationalityCountryID, imagePath);

            }
            else
            {
                return null;
            }
        }

        public static clsPerson Find(string NationalNo)
        {
            string firstName = "", secondName = "", thirdName = "", lastName = "", address = "", phone = "", email = "", imagePath = "";
            int personID = -1, nationalityCountryID = -1;
            DateTime dateOfBirth = DateTime.Now;
            byte gendor = 0;

            if (clsPersonData.GetPersonByNationalNo(NationalNo, ref personID, ref firstName, ref secondName, ref thirdName, ref lastName, ref dateOfBirth, ref gendor, ref address, ref phone, ref email, ref nationalityCountryID, ref imagePath))
            {
                return new clsPerson(personID, NationalNo, firstName, secondName, thirdName, lastName, dateOfBirth, gendor, address, phone, email, nationalityCountryID, imagePath);

            }
            else
            {
                return null;
            }
        }

        static public bool IsExist(string NationalNo)
        {
            return clsPersonData.IsExist(NationalNo);
        }

        static public bool IsExist(int PersonID)
        {
            return clsPersonData.IsExist(PersonID);
        }

        public static bool Delete(int PersonID)
        {
            return clsPersonData.Delete(PersonID);
        }


    }
}
