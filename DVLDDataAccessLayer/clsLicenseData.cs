using System;                            
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsLicenseData
    {
        static public DataTable LicensesList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Licenses";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtLicenses = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dtLicenses.Load(reader);
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                //error message
            }
            finally
            {
                connection.Close();
            }

            return dtLicenses;
        }

        static public DataTable LicensesListForDriverID(int DriverID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT    Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName, Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive
                             FROM      Licenses INNER JOIN
                                           Applications ON Licenses.ApplicationID = Applications.ApplicationID INNER JOIN
                                           LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                             
                             			  where DriverID = @DriverID";
                             
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

            DataTable dtLicenses = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtLicenses.Load(reader);
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                //error message
            }
            finally
            {
                connection.Close();
            }

            return dtLicenses;
        }


        static public int AddNewLicense(int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive, short IssueReason, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"INSERT INTO [dbo].[Licenses]
                                         ([ApplicationID]
                                         ,[DriverID]
                                         ,[LicenseClass]
                                         ,[IssueDate]
                                         ,[ExpirationDate]
                                         ,[Notes]
                                         ,[PaidFees]
                                         ,[IsActive]
                                         ,[IssueReason]
                                         ,[CreatedByUserID])
                                   VALUES
                                         (@ApplicationID
                                         ,@DriverID
                                         ,@LicenseClass
                                         ,@IssueDate
                                         ,@ExpirationDate
                                         ,@Notes
                                         ,@PaidFees
                                         ,@IsActive
                                         ,@IssueReason
                                         ,@CreatedByUserID);
                           select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            if(Notes!=null)
            {
                command.Parameters.AddWithValue("@Notes", Notes);
            }
            else
            {
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            }
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int LicenseID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if(result != null)
                {
                    LicenseID = Convert.ToInt16(result);
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return LicenseID;

        }

        static public bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive, short IssueReason, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE [dbo].[Licenses]
                             SET [ApplicationID] = @ApplicationID,
                                [DriverID] = @DriverID,
                                [LicenseClass] = @LicenseClass,
                                [IssueDate] = @IssueDate ,
                                [ExpirationDate] = @ExpirationDate,
                                [Notes] = @Notes,
                                [PaidFees] = @PaidFees,
                                [IsActive] = @IsActive,
                                [IssueReason] = @IssueReason,
                                [CreatedByUserID] = @CreatedByUserID,
                           WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            if (Notes != null)
            {
                command.Parameters.AddWithValue("@Notes", Notes);
            }
            else
            {
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            }
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;

        }

        static public bool GetLicenseInfoByLicenseID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClassID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees, ref bool IsActive, ref short IssueReason, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Licenses
                             where LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    isfound = true;
                    ApplicationID = Convert.ToInt16(reader["ApplicationID"]);
                    DriverID = Convert.ToInt16(reader["DriverID"]);
                    LicenseClassID = Convert.ToInt16(reader["LicenseClass"]);
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    if(reader["Notes"]!=DBNull.Value)
                    {
                        Notes = (string)reader["Notes"];
                    }
                    else
                    {
                        Notes = null;
                    }
                    PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = Convert.ToInt16(reader["IssueReason"]);
                    CreatedByUserID = Convert.ToInt16(reader["CreatedByUserID"]);
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                //error message
            }
            finally
            {
                connection.Close();
            }

            return isfound ;
        }

        static public bool GetLicenseInfoByApplicationID(int ApplicationID, ref int LicenseID, ref int DriverID, ref int LicenseClassID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref decimal PaidFees, ref bool IsActive, ref short IssueReason, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Licenses
                             where ApplicationID=@ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;
                    LicenseID = Convert.ToInt16(reader["LicenseID"]);
                    DriverID = Convert.ToInt16(reader["DriverID"]);
                    LicenseClassID = Convert.ToInt16(reader["LicenseClassID"]);
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    if (reader["Notes"] != DBNull.Value)
                    {
                        Notes = (string)reader["Notes"];
                    }
                    else
                    {
                        Notes = null;
                    }
                    PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = (short)reader["IssueReason"];
                    CreatedByUserID = Convert.ToInt16(reader["CreatedByUserID"]);
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                //error message
            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }

        static public bool IsExist(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select isfound=1 from Licenses
                             where LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if(result!=null)
                {
                    isfound = true;
                }

            }
            catch (Exception ex)
            {
                //error message
            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }
        static public bool IsExist(int PersonID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT isfound=1   
                             FROM      Licenses INNER JOIN
                                           Applications ON Licenses.ApplicationID = Applications.ApplicationID
                             
                             			  where Applications.ApplicantPersonID = @PersonID and LicenseClass = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    isfound = true;
                }

            }
            catch (Exception ex)
            {
                //error message
            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }

        static public bool IsDetained(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select isfound=1 from DetainedLicenses
                             where LicenseID = @LicenseID and IsReleased=0";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    isfound = true;
                }

            }
            catch (Exception ex)
            {
                //error message
            }
            finally
            {
                connection.Close();
            }

            return isfound;
        }

        static public bool Delete(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"Delete * from Licenses
                             where LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch
            {
                //
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0 ;
        }

        static public bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT    isfound =1
                             FROM      Drivers INNER JOIN
                                           Licenses ON Drivers.DriverID = Licenses.DriverID INNER JOIN
                                           People ON Drivers.PersonID = People.PersonID
                             
                             where People.PersonID = @PersonID and LicenseClass=@LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    isfound = true;
                }

            }

            catch (Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return isfound;
        }

        static public bool DeactivateLicense(int LicenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE [dbo].[Licenses] 
                               SET [IsActive] = 0
                                
                           WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;

        }

        static public bool IsActiveLicenseExists(int PersonID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT    isfound =1
                             FROM      Drivers INNER JOIN
                                           Licenses ON Drivers.DriverID = Licenses.DriverID INNER JOIN
                                           People ON Drivers.PersonID = People.PersonID
                             
                             where People.PersonID = @PersonID and LicenseClass=@LicenseClassID and Licenses.IsActive=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    isfound = true;
                }

            }

            catch (Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return isfound;
        }


    }

}
