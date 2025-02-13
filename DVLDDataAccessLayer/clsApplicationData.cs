using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDDataAccessLayer
{
    public class clsApplicationData
    {
        static public DataTable ApplicationsList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = "select * from Applications";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtApplications = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtApplications.Load(reader);
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

            return dtApplications;

        }
        
        static public bool GetApplicationInfoByApplicationID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate,
                             ref int ApplicationTypeID, ref int ApplicationStatus, ref DateTime LastStatusDate, 
                             ref decimal PaidFees, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from	Applications
                             where ApplicationID = @ApplicationID";

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

                    ApplicantPersonID = Convert.ToInt16(reader["ApplicantPersonID"]);
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = Convert.ToInt16(reader["ApplicationTypeID"]);
                    ApplicationStatus = Convert.ToInt16(reader["ApplicationStatus"]);
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = Convert.ToDecimal(reader["PaidFees"]);
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

        static public int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, int ApplicationStatus, DateTime LastStatusDate,
                                                             decimal PaidFees, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"insert into Applications(ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees,CreatedByUserID)
                             values(@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID)
                             select SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int NewApplicationID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                NewApplicationID = Convert.ToInt16(result);

            }

            catch (Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return NewApplicationID;

        }

        static public bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, int ApplicationStatus, DateTime LastStatusDate,
                                                             decimal PaidFees, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE Applications
                             SET    ApplicantPersonID = @ApplicantPersonID,
                                    ApplicationDate = @ApplicationDate,
                                    ApplicationTypeID = @ApplicationTypeID,
                                    ApplicationStatus = @ApplicationStatus,
                                    LastStatusDate = @LastStatusDate,
                                    PaidFees = @PaidFees,
                                    CreatedByUserID = @CreatedByUserID

                             WHERE  ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;

        }


        static public bool Delete(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"delete from Applications
                             where ApplicationID = @ApplicationID";
    
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        static public bool IsApplicationExists(int ApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select isfound=1 from	Applications
                             where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result  = command.ExecuteScalar();
                isfound = result!= null;

               
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

        static public bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return GetActiveApplicationID(PersonID, ApplicationTypeID) != -1;
        }
        static public int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select ActiveApplicationID = ApplicationID from Applications
                             where ApplicantPersonID = @PersonID and ApplicationTypeID = @ApplicationTypeID and ApplicationStatus = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            int AppID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();
                AppID = Convert.ToInt16(result);

            }

            catch (Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return AppID;

        }
        static public int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT    ActiveApplicationID = Applications.ApplicationID
                             FROM      Applications INNER JOIN
                                           LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID INNER JOIN
                                           LocalDrivingLicenseApplications AS LocalDrivingLicenseApplications_1 ON Applications.ApplicationID = LocalDrivingLicenseApplications_1.ApplicationID
                             
                             WHERE ApplicantPersonID = @ApplicantPersonID
                              and ApplicationTypeID = @ApplicationTypeID
                              and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                              and ApplicationStatus =1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            int AppID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();
                if(result!=null)
                {
                    AppID = Convert.ToInt16(result);

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

            return AppID;

        }

        static public bool UpdateStatus(int ApplicationID, short NewStatus)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"update Applications
		                     SET ApplicationStatus = @NewStatus,
		                     	LastStatusDate = @LastStatusDate
		                     	where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NewStatus", NewStatus);
            command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }

            catch (Exception ex)
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
