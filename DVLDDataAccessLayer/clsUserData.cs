using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsUserData
    {
        static public DataTable UsersList()
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"SELECT    Users.UserID, People.PersonID,
                              FullName = People.FirstName+ ' '+ People.SecondName+ ' ' + People.ThirdName+' '+People.LastName,
                              Users.UserName, Users.IsActive
                    FROM      People INNER JOIN
                                  Users ON People.PersonID = Users.PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable dtUsers = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dtUsers.Load(reader);
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

            return dtUsers;


        }

        static public bool GetUserByUserID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Users
                            where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            bool isfound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    isfound = true;

                    PersonID = Convert.ToInt16(reader["PersonID"]);
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
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

        static public bool GetUserByUserName(string UserName, ref int UserID, ref int PersonID, ref string Password, ref bool IsActive)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select * from Users
                            where UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            bool isfound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    isfound = true;

                    PersonID = Convert.ToInt16(reader["PersonID"]);
                    UserID = Convert.ToInt16(reader["UserID"]);
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
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


        static public bool IsExist(string UserName)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select isFound=1 from Users
                            where UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

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

        static public bool IsExist(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select isFound=1 from Users
                            where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        static public bool IsExist(string UserName, string Password)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"select isFound=1 from Users
                            where UserName = @UserName and Password = @Password";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

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


        static public int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"insert into Users(PersonID, UserName, Password, IsActive)
                             values(@PersonID, @UserName, @Password, @IsActive)
                             select SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            int NewUserID = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                NewUserID = Convert.ToInt16(result);

            }

            catch(Exception ex)
            {
                //error message
            }

            finally
            {
                connection.Close();
            }

            return NewUserID;

        }

        static public bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"update Users
                            set PersonID = @PersonID,
                            	UserName = @UserName,
                            	Password = @Password,
                            	IsActive = @IsActive
                            where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

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

        static public bool Delete(int UserID)
        {
            SqlConnection connection = new SqlConnection(clsDataParameters.ConnectionString);

            string query = @"delete from Users
                             where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

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
