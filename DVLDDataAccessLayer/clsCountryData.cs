using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsCountryData
    {
        static public DataTable CoutriesList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Countries
                             order by CountryName";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtCountries = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dtCountries.Load(reader);
                    reader.Close();
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

            return dtCountries;

        }

        static public bool Find(string CountryName, ref int CountryID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select CountryID from Countries
                             where CountryName = @CountryName";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("CountryName", CountryName);

            bool isfound = false;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    isfound = true;
                    CountryID = Convert.ToInt16(result);
                }
            }

            catch (Exception ex)
            {
                //
            }

            finally
            {
                connection.Close();
            }

            return isfound;
        }

        static public bool Find(int CountryID, ref string CountryName)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select CountryName from Countries
                             where CountryID = @CountryID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("CountryID", CountryID);

            bool isfound = false;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    isfound = true;
                    CountryName =(string)result;
                }
            }

            catch (Exception ex)
            {
                //
            }

            finally
            {
                connection.Close();
            }

            return isfound;
        }


    }



}
