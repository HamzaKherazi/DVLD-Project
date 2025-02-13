using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsApplicationTypeData
    {
        static public DataTable ApplicationTypesList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = "select * from ApplicationTypes";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtApplicationTypes = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dtApplicationTypes.Load(reader);
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

            return dtApplicationTypes;

        }

        static public bool GetApplicationTypeInfo(int ApplicationTypeID, ref string ApplicationTypeTitle, ref decimal ApplicationFees)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from ApplicationTypes
                             where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    isfound = true;
                    ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    ApplicationFees = (decimal)reader["ApplicationFees"];
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

            return isfound;

        }



        static public bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE ApplicationTypes
                            SET ApplicationTypeTitle = @ApplicationTypeTitle,
                                ApplicationFees = @ApplicationFees
                          WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch(Exception ex)
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
