using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsPersonData
    {
        static public DataTable PeopleList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT    People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, 
                                    Gendor = 
                          			  case
                          			  when People.Gendor = 0 then 'Male'
                          			  when People.Gendor = 1 then 'Female'
                          			  end,
                          		  People.DateOfBirth, Countries.CountryName, People.Phone, People.Email
                          FROM      People INNER JOIN
                                        Countries ON People.NationalityCountryID = Countries.CountryID";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtPeople = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dtPeople.Load(reader);
                }

                reader.Close();

            }

            catch(Exception ex)
            {
                //error message;
            }

            finally
            {
                connection.Close();
            }

            return dtPeople;

        }

        
        static public int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName,
                                         string LastName, DateTime DateOfBirth, byte Gendor, string Address, 
                                         string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"INSERT INTO [dbo].[People]
                                   ([NationalNo]
                                   ,[FirstName]
                                   ,[SecondName]
                                   ,[ThirdName]
                                   ,[LastName]
                                   ,[DateOfBirth]
                                   ,[Gendor]
                                   ,[Address]
                                   ,[Phone]
                                   ,[Email]
                                   ,[NationalityCountryID]
                                   ,[ImagePath])
                             
                             VALUES(@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, 
                                    @Gendor, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);

                             select SCOPE_IDENTITY(); ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if(Email!=null)
            {
                command.Parameters.AddWithValue("@Email", Email);

            }
            else
            {
                command.Parameters.AddWithValue("@Email", DBNull.Value);

            }

            if (ImagePath != null)
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);

            }
            else
            {
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            }

            int NewPersonID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                NewPersonID = Convert.ToInt16(result);

            }

            catch (Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return NewPersonID;


        }
    
        static public bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName,
                                         string LastName, DateTime DateOfBirth, byte Gendor, string Address,
                                         string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE People
                               SET NationalNo = @NationalNo,
                                   FirstName = @FirstName,
                                   SecondName = @SecondName,
                                   ThirdName = @ThirdName,
                                   LastName = @LastName,
                                   DateOfBirth = @DateOfBirth,
                                   Gendor = @Gendor,
                                   Address = @Address,
                                   Phone = @Phone,
                                   Email = @Email,
                                   NationalityCountryID = @NationalityCountryID,
                                   ImagePath = @ImagePath

                             WHERE  PersonID = @PersonID ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (Email != null)
            {
                command.Parameters.AddWithValue("@Email", Email);

            }
            else
            {
                command.Parameters.AddWithValue("@Email", DBNull.Value);

            }

            if (ImagePath != null)
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);

            }
            else
            {
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            }

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }

            catch(Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;

        }

    
        static public bool GetPersonByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName,
                                         ref string LastName, ref DateTime DateOfBirth, ref byte Gendor, ref string Address,
                                         ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from People
                             where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    isfound = true;

                    NationalNo = reader["NationalNo"].ToString();
                    FirstName = reader["FirstName"].ToString();
                    SecondName = reader["SecondName"].ToString();
                    ThirdName = reader["ThirdName"].ToString();
                    LastName = reader["LastName"].ToString();
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    Gendor = Convert.ToByte(reader["Gendor"]);
                    Address = reader["Address"].ToString();
                    Phone = reader["Phone"].ToString();
                    NationalityCountryID = Convert.ToInt16(reader["NationalityCountryID"]);

                    if(reader["Email"] != DBNull.Value)
                    {
                        Email = reader["Email"].ToString();

                    }
                    else
                    {
                        Email = null;
                    }

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = reader["ImagePath"].ToString();

                    }
                    else
                    {
                        ImagePath = null;
                    }

                    reader.Close();

                }

            }

            catch(Exception ex)
            {
                //message error
            }

            finally
            {
                connection.Close();
            }

            return isfound;

        }


        static public bool GetPersonByNationalNo(string NationalNo,ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName,
                                         ref string LastName, ref DateTime DateOfBirth, ref byte Gendor, ref string Address,
                                         ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from People
                             where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    PersonID = Convert.ToInt16(reader["PersonID"]);
                    FirstName = reader["FirstName"].ToString();
                    SecondName = reader["SecondName"].ToString();
                    ThirdName = reader["ThirdName"].ToString();
                    LastName = reader["LastName"].ToString();
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    Gendor = Convert.ToByte(reader["Gendor"]);
                    Address = reader["Address"].ToString();
                    Phone = reader["Phone"].ToString();
                    NationalityCountryID = Convert.ToInt16(reader["NationalityCountryID"]);

                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = reader["Email"].ToString();

                    }
                    else
                    {
                        Email = null;
                    }

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = reader["ImagePath"].ToString();

                    }
                    else
                    {
                        ImagePath = null;
                    }

                    reader.Close();

                }

            }

            catch (Exception ex)
            {
                //message error
            }

            finally
            {
                connection.Close();
            }

            return isfound;

        }

        static public bool IsExist(string NationalNo)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select IsFound=1 from People
                             where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isfound = (result != null);

            }

            catch(SqlException ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return isfound;

        }

        static public bool IsExist(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select IsFound=1 from People
                             where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isfound = (result != null);

            }

            catch (SqlException ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return isfound;

        }


        static public bool Delete(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"delete from People
                            where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }

            catch(SqlException ex)
            {
                //error message 
            }

            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;


        }


    }
}
