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
    public class clsLicenseClassData
    {
        static public DataTable LicenseClassesList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = "select * from LicenseClasses";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtLicenseClasses = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dtLicenseClasses.Load(reader);
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

            return dtLicenseClasses;

        }

        static public bool GetLicenseClassInfoByID(int LicenseClassID, ref string ClassName, ref string ClassDescription, ref int MinimumAllowedAge,
                                                    ref int DefaultValidityLength, ref decimal ClassFees)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from	LicenseClasses
                             where LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            bool isfound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    ClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = Convert.ToInt16(reader["MinimumAllowedAge"]);
                    DefaultValidityLength = Convert.ToInt16(reader["DefaultValidityLength"]);
                    ClassFees = Convert.ToDecimal(reader["ClassFees"]);

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

        static public bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID, ref string ClassDescription, ref int MinimumAllowedAge,
                                                    ref int DefaultValidityLength, ref decimal ClassFees)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from	LicenseClasses
                             where ClassName = @ClassName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", ClassName);

            bool isfound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    LicenseClassID = Convert.ToInt16(reader["LicenseClassID"]);
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = Convert.ToInt16(reader["MinimumAllowedAge"]);
                    DefaultValidityLength = Convert.ToInt16(reader["DefaultValidityLength"]);
                    ClassFees = Convert.ToDecimal(reader["ClassFees"]);


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
