using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsTestAppointmentData
    {
        static public DataTable TestAppointmentsList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from TestAppointments";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtAppointments = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dtAppointments.Load(reader);
                }

                reader.Close();

            }

            catch(Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return dtAppointments;

        }
        static public DataTable GetTestAppointmentsForLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select TestAppointmentID,AppointmentDate, PaidFees, IsLocked from TestAppointments
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            DataTable dtAppointments = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtAppointments.Load(reader);
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

            return dtAppointments;

        }



        static public int AddNewAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees,
                              int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"INSERT INTO TestAppointments
                                   (TestTypeID
                                   ,LocalDrivingLicenseApplicationID
                                   ,AppointmentDate
                                   ,PaidFees
                                   ,CreatedByUserID
                                   ,IsLocked
                                   ,RetakeTestApplicationID)
                             VALUES(@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate,@PaidFees, @CreatedByUserID,@IsLocked,@RetakeTestApplicationID);

                             select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);

            if(RetakeTestApplicationID !=-1)
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            }
            else
            {
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

            }


            int AppointmentID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    AppointmentID = Convert.ToInt16(result);
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

            return AppointmentID;
        }

        static public bool UpdateTestAppointment(int TestAppointmentID, DateTime AppointmentDate, bool IsLocked)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE TestAppointments
                             SET AppointmentDate = @AppointmentDate,
                                 IsLocked = @IsLocked
                             where TestAppointmentID =@TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            if(IsLocked)
            {
                command.Parameters.AddWithValue("@IsLocked", 1);

            }
            else
            {
                command.Parameters.AddWithValue("@IsLocked", 0);

            }

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


        static public bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate, ref decimal PaidFees,
                              ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from TestAppointments
                             where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isfound = true;

                    TestTypeID = Convert.ToInt16(reader["TestTypeID"]);
                    LocalDrivingLicenseApplicationID = Convert.ToInt16(reader["LocalDrivingLicenseApplicationID"]);
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                    CreatedByUserID = Convert.ToInt16(reader["CreatedByUserID"]);
                    IsLocked = (bool)reader["IsLocked"];
                    if (reader["RetakeTestApplicationID"] != DBNull.Value)
                    {
                        RetakeTestApplicationID = Convert.ToInt16(reader["RetakeTestApplicationID"]);
                    }
                    else
                    {
                        RetakeTestApplicationID = -1;

                    }
                            
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

        static public bool GetLastTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID, ref int TestAppointmentID, ref DateTime AppointmentDate, ref decimal PaidFees,
                              ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {

            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select top 1 * from TestAppointments
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND TestTypeID=@TestTypeID
                             ORDER BY TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isfound = true;

                    TestAppointmentID = Convert.ToInt16(reader["TestAppointmentID"]);
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                    CreatedByUserID = Convert.ToInt16(reader["CreatedByUserID"]);
                    IsLocked = (bool)reader["IsLocked"];
                    if (reader["RetakeTestApplicationID"] != DBNull.Value)
                    {
                        RetakeTestApplicationID = Convert.ToInt16(reader["RetakeTestApplicationID"]);
                    }
                    else
                    {
                        RetakeTestApplicationID = -1;

                    }

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

        static public DataTable GetTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestType)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select TestAppointmentID,AppointmentDate, PaidFees, IsLocked from TestAppointments
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID=@TestType
                             ORDER BY TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestType", TestType);

            DataTable dtAppointments = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtAppointments.Load(reader);
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

            return dtAppointments;

        }


        static public bool Delete(int TestAppointmentID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"Delete from TestAppointments
                             where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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

        static public bool IsTestAppointmentLockedForLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select isfound=1 from TestAppointments
                             where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID and IsLocked = 1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isfound = (result != null);

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
        
        static public bool IsExists(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select isfound=1 from TestAppointments
                             where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool isfound = false;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isfound = (result != null);

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

        static public int GetTestID(int TestAppointmentID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT    Tests.TestID
                             FROM      TestAppointments INNER JOIN
                                           Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE TestAppointments.TestAppointmentID =@TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            int TestID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if(result != null)
                {
                    TestID = Convert.ToInt16(result);
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

            return TestID;
        }

    }
}
