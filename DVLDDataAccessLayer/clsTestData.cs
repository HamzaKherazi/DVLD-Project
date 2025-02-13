using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsTestData
    {
        static public int GetTestTrials(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT    COUNT(Tests.TestID)
                             FROM      TestAppointments INNER JOIN
                                           Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID=@TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            int trials = 0;

            try
            {    
                connection.Open();

                object result = command.ExecuteScalar();
                
                if(result!=null)
                {
                    trials = (int)result;
                }
            }
            catch(Exception ex)
            {
                //error message
            }
            finally
            {
                connection.Close();
            }

            return trials;

        }
        
        static public DataTable GetAllTests()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);
            
            string query = "select * from Tests";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtTests = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                dtTests.Load(reader);

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
                
            return dtTests;



        }

        static public int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);
            string query = @"INSERT INTO Tests
                                       (TestAppointmentID
                                       ,TestResult
                                       ,Notes
                                       ,CreatedByUserID)
                             VALUES(@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
                             select SCOPE_IDENTITY();

                             update TestAppointments
                             set IsLocked = 1
                             where TestAppointmentID=@TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            if(Notes != null)
            {
                command.Parameters.AddWithValue("@Notes", Notes);
            }
            else
            {
                command.Parameters.AddWithValue("@Notes", DBNull.Value);
            }
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            int TestID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();  
                
                if(result!=null)
                {
                    TestID = Convert.ToInt16(result);
                }
            }

            catch(Exception ex)
            {
                //
            }
            finally
            {
                connection.Close();
            }

            return TestID;
               


        }

        static public bool GetTestInfo(int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Tests
                             where TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);

            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    isfound = true;
                    TestAppointmentID = Convert.ToInt16(reader["TestAppointmentID"]);
                    TestResult = (bool)reader["TestResult"];
                    if (reader["Notes"]!=DBNull.Value)
                    {
                        Notes = (string)reader["Notes"];
                    }
                    else
                    {
                        Notes=null;
                    }

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



    }
}
