using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsTestTypeData
    {
        static public DataTable TestTypesList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = "select * from TestTypes";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtTestTypes = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtTestTypes.Load(reader);
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

            return dtTestTypes;

        }

        static public bool GetTestTypeInfo(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription, ref decimal TestTypeFees)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from TestTypes
                             where TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;
                    TestTypeTitle = (string)reader["TestTypeTitle"];
                    TestTypeDescription = (string)reader["TestTypeDescription"];
                    TestTypeFees = Convert.ToDecimal(reader["TestTypeFees"]);

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

        static public bool UpdateTestType(int testTypeID, string testTypeTitle, string testTypeDescription, decimal testTypeFees)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE TestTypes
                            SET TestTypeTitle = @TestTypeTitle,
                                TestTypeDescription = @TestTypeDescription,
                                TestTypeFees = @TestTypeFees
                          WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            command.Parameters.AddWithValue("@TestTypeTitle", testTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", testTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", testTypeFees);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //erroe message
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;

        }


    }
}
