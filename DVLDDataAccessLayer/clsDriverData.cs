using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsDriverData
    {
        static public DataTable DriversList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Drivers_View";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtDrivers = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dtDrivers.Load(reader);
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

            return dtDrivers;
        }

        static public int AddNewDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"INSERT INTO Drivers
                                   (PersonID
                                   ,CreatedByUserID
                                   ,CreatedDate)
                             VALUES(@PersonID, @CreatedByUserID, @CreatedDate);
                        
                        	 select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

            int DriverID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if(result != null)
                {
                    DriverID = (int)result;
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

            return DriverID;
        }

        static public bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"UPDATE Drivers
                             SET PersonID = @PersonID,
                                CreatedByUserID = @CreatedByUserID,
                                CreatedDate = @CreatedDate
                           WHERE DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

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

        static public bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Drivers
                             where DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);


            bool isfound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    PersonID = Convert.ToInt16(reader["PersonID"]);
                    CreatedByUserID = Convert.ToInt16(reader["CreatedByUserID"]);
                    CreatedDate = (DateTime)reader["CreatedDate"];

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

        static public bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Drivers
                             where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);


            bool isfound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    DriverID = Convert.ToInt16(reader["DriverID"]);
                    CreatedByUserID = Convert.ToInt16(reader["CreatedByUserID"]);
                    CreatedDate = (DateTime)reader["CreatedDate"];

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

        static public bool IsExist(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select isfound=1 from Drivers
                             where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);


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


        static public bool Delete(int DriverID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"delete from Drivers
                            where DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", DriverID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }

            catch (SqlException ex)
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
