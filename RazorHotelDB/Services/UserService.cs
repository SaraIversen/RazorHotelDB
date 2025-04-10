using Microsoft.Data.SqlClient;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using System.Data;
using System.Threading.Tasks;

namespace RazorHotelDB.Services
{
    public class UserService : IUserService
    {
        private string connectionString = Secret.ConnectionString;
        private string getAllUsersSql = "select Id, UserName, Password, Email FROM Users";
        private string insertSql = "insert INTO Users Values(@Id, @UserName, @Password, @Email)";

        public async Task<bool> CreateUserAsync(User user)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertSql, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    int noOfRows = await command.ExecuteNonQueryAsync();
                    //Thread.Sleep(1000); // Sleep for 1 second.
                    Console.WriteLine($"Antal indsatte i tabellen {noOfRows}");

                    if (noOfRows == 1) return true;
                }
                catch (SqlException sqlEx)
                {
                    //Console.WriteLine(sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
            return false;
        }

        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getAllUsersSql, connection);
                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    //Thread.Sleep(1000); // Sleep for 1 second.
                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32("Id");
                        string userName = reader.GetString("UserName");
                        string password = reader.GetString("Password");
                        string email = reader.GetString("Email");
                        User user = new User(id, userName, password, email);
                        users.Add(user);
                    }
                    reader.Close();
                }
                catch (SqlException sqlEx)
                {
                    //Console.WriteLine("Database error" + sqlEx.Message);
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Generel fejl: " + ex.Message);
                    throw ex;
                }
            }
            return users;
        }

        public async Task<User?> VerifyUser(string userName, string password)
        {
            List<User> users = await GetAllUsers();
            foreach (var user in users)
            {
                if (userName.Equals(user.UserName) && password.Equals(user.Password))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
