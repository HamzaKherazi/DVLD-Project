using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsDetainedLicenseData
    {

        static public DataTable DetainedLicensesList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from DetainedLicenses_View
                             Order By DetainDate Desc";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtDetainedLicenses = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtDetainedLicenses.Load(reader);
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

            return dtDetainedLicenses;
        }

        static public bool GetDetainedLicenseInfo(int LicenseID, ref int DetainID, ref DateTime DetainDate, ref decimal FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT TOP 1 * from DetainedLicenses
                             WHERE LicenseID = @LicenseID
                             ORDER BY DetainDate DESC";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            bool isfound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;
                    DetainID = Convert.ToInt16(reader["DetainID"]);
                    DetainDate = (DateTime)reader["DetainDate"];
                    FineFees = Convert.ToDecimal(reader["FineFees"]);
                    CreatedByUserID = Convert.ToInt16(reader["CreatedByUserID"]);
                    IsReleased = (bool)reader["IsReleased"];
                    
                    if (reader["ReleaseDate"] != DBNull.Value)
                    {
                        ReleaseDate = (DateTime)reader["ReleaseDate"];
                    }
                    else
                    {
                        ReleaseDate = DateTime.Now;
                    }

                    if (reader["ReleasedByUserID"] != DBNull.Value)
                    {
                        ReleasedByUserID = Convert.ToInt16(reader["ReleasedByUserID"]);
                    }
                    else
                    {
                        ReleasedByUserID = -1;
                    }

                    if (reader["ReleaseApplicationID"] != DBNull.Value)
                    {
                        ReleaseApplicationID = Convert.ToInt16(reader["ReleaseApplicationID"]);
                    }
                    else
                    {
                        ReleaseApplicationID = -1;
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

        static public int DetainLicense(int LicenseID, decimal FineFees, int CreatedByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"INSERT INTO [dbo].[DetainedLicenses]
                             ([LicenseID]
                             ,[DetainDate]
                             ,[FineFees]
                             ,[CreatedByUserID]
                             ,[IsReleased]
                             ,[ReleaseDate]
                             ,[ReleasedByUserID]
                             ,[ReleaseApplicationID])
                       VALUES
                             (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased, @ReleaseDate, @ReleasedByUserID, @ReleaseApplicationID);
                          select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DateTime.Now);
            command.Parameters.AddWithValue("@FineFees", FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsReleased", false);
            command.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);
            command.Parameters.AddWithValue("@ReleasedByUserID", DBNull.Value);
            command.Parameters.AddWithValue("@ReleaseApplicationID", DBNull.Value);

            int DetainID = -1;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                    DetainID = Convert.ToInt16(result);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return DetainID;
        }

        static public bool ReleaseLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"Update DetainedLicenses
                             set IsReleased = 1,
                             	ReleaseDate = @ReleaseDate,
                             	ReleasedByUserID =@ReleasedByUserID,
                             	ReleaseApplicationID=@ReleaseApplicationID
                             	where DetainID = @DetainID ;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

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


    }
}
