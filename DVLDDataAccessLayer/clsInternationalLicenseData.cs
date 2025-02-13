using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDDataAccessLayer
{
    public class clsInternationalLicenseData
    {

        static public DataTable AllInternationalLicenses()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive
                             from InternationalLicenses
                             order by IsActive, ExpirationDate DESC";

            SqlCommand command = new SqlCommand(query, connection);

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


        static public DataTable LicensesListForDriverID(int DriverID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select InternationalLicenseID, ApplicationID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive
                             from InternationalLicenses
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

        static public int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)   
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"Update InternationalLicenses   
                               SET IsActive=0
                                WHERE DriverID=@DriverID;


                             INSERT INTO [dbo].[InternationalLicenses]
                                          ([ApplicationID]
                                          ,[DriverID]
                                          ,[IssuedUsingLocalLicenseID]
                                          ,[IssueDate]
                                          ,[ExpirationDate]
                                          ,[IsActive]
                                          ,[CreatedByUserID])
                                   VALUES
                                         (@ApplicationID
                                         ,@DriverID
                                         ,@IssuedUsingLocalLicenseID
                                         ,@IssueDate
                                         ,@ExpirationDate
                                         ,@IsActive
                                         ,@CreatedByUserID);
            
                           select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            
            int InternationalLicenseID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    InternationalLicenseID = Convert.ToInt16(result);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return InternationalLicenseID;
        }
        static public bool UpdateLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE [dbo].[InternationalLicenses]
                             SET [ApplicationID] = @ApplicationID,
                                [DriverID] = @DriverID,
                                [IssuedUsingLocalLicenseID] = @IssuedUsingLocalLicenseID,
                                [IssueDate] = @IssueDate ,
                                [ExpirationDate] = @ExpirationDate,
                                [IsActive] = @IsActive,
                                [CreatedByUserID] = @CreatedByUserID
                           WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
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

        static public bool GetInternationalLicenseInfoByInternationalLicenseID(int InternationalLicenseID, ref int ApplicationID, ref int DriverID, ref int IssuedUsingLocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from InternationalLicenses
                             where InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;
                    ApplicationID = Convert.ToInt16(reader["ApplicationID"]);
                    DriverID = Convert.ToInt16(reader["DriverID"]);
                    IssuedUsingLocalLicenseID = Convert.ToInt16(reader["IssuedUsingLocalLicenseID"]);
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    IsActive = (bool)reader["IsActive"];
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

        static public int GetActiveInternationalLicense(int DriverID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select InternationalLicenseID from InternationalLicenses
                             where DriverID = @DriverID and IsActive=1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

            int InternationalLicenseID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    InternationalLicenseID = Convert.ToInt16(result);
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

            return InternationalLicenseID;
        }

        static public bool Delete(int InternationalLicenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"Delete * from InternationalLicenses
                             where InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

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

            return rowsAffected > 0;
        }


    }
}
