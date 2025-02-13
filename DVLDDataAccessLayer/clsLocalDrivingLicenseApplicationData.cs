using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDDataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationData
    {
        static public DataTable LocalDrivingLicenseApplicationsList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = "select * from LocalDrivingLicenseApplications_View";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtLocalDrivingLicenseApplications = new DataTable();
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtLocalDrivingLicenseApplications.Load(reader);
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

            return dtLocalDrivingLicenseApplications;

        }

        static public bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from	LocalDrivingLicenseApplications
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            bool isfound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    ApplicationID = Convert.ToInt16(reader["ApplicationID"]);
                    LicenseClassID = Convert.ToInt16(reader["LicenseClassID"]);


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
        static public bool GetLocalDrivingLicenseApplicationInfoByApplicationID(int ApplicationID, ref int LocalDrivingLicenseApplicationID, ref int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from	LocalDrivingLicenseApplications
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

                    ApplicationID = Convert.ToInt16(reader["LocalDrivingLicenseApplicationID"]);
                    LicenseClassID = Convert.ToInt16(reader["LicenseClassID"]);


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

        static public int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"insert into LocalDrivingLicenseApplications(ApplicationID, LicenseClassID)
                             values(@ApplicationID, @LicenseClassID)
                             select SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            

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

        static public bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE LocalDrivingLicenseApplications
                             SET    ApplicationID = @ApplicationID,
                                    LicenseClassID = @LicenseClassID

                             WHERE  LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
           

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

        static public bool Delete(int LocalDrivingLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"delete from LocalDrivingLicenseApplications
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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




        //

        


        static public short GetPassedTests(int LocalDrivingLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select PassedTestCount from LocalDrivingLicenseApplications_View
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            short PassedTests = 0;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if(result != null)
                {
                    PassedTests = Convert.ToInt16(result);
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

            return PassedTests;
        }

        static public int GetActiveLicense(int PersonID, int LicenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT    Licenses.LicenseID
                             FROM      Licenses INNER JOIN
                                           Applications ON Licenses.ApplicationID = Applications.ApplicationID INNER JOIN
                                           People ON Applications.ApplicantPersonID = People.PersonID
                             			  where People.PersonID=@PersonID and LicenseClass=@LicenseClassID and Licenses.IsActive=1";
                             
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            int LicenseID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    LicenseID = Convert.ToInt16(result);
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

            return LicenseID;
        }


    }
}
