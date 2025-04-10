using Microsoft.Data.SqlClient;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using System.Data;

namespace RazorHotelDB.Services
{
    public class HotelService : IHotelServiceAsync
    {
        private string connectionString = Secret.ConnectionString;
        private string getAllHotelsSql = "select Hotel_No, Name, Address, Image FROM Hotel";
        private string insertSql = "insert INTO Hotel Values(@ID, @Navn, @Adresse, @Billede)";
        private string deleteSql = "delete FROM Hotel WHERE Hotel_No = @ID";
        private string findHotelByIDSql = "select Hotel_No, Name, Address, Image FROM Hotel WHERE Hotel_No = @ID";
        private string findHotelsByNameSql = "select * from Hotel WHERE Name LIKE @Name";
        private string updateHotelSql = "update Hotel SET Name = @Navn, Address = @Adresse, Image = @Billede WHERE Hotel_No = @ID";

        public async Task<bool> CreateHotelAsync(Hotel hotel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertSql, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@ID", hotel.HotelNr);
                    command.Parameters.AddWithValue("@Navn", hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse", hotel.Adresse);
                    command.Parameters.AddWithValue("@Billede", hotel.Image != null ? hotel.Image : "");

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

        public async Task<Hotel> DeleteHotelAsync(int hotelNr)
        {
            Hotel? hotel = await GetHotelFromIdAsync(hotelNr);
            if (hotel == null) return null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@ID", hotelNr);

                    int noOfRows = await command.ExecuteNonQueryAsync();
                    //Thread.Sleep(1000); // Sleep for 1 second.
                    Console.WriteLine($"Antal fjernede i tabellen {noOfRows}");

                    if (noOfRows > 0) return hotel;
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
            return null;
        }

        public async Task<List<Hotel>> GetAllHotelAsync()
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getAllHotelsSql, connection);
                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    //Thread.Sleep(1000); // Sleep for 1 second.
                    while (await reader.ReadAsync())
                    {
                        int hotelNr = reader.GetInt32("Hotel_No");
                        string hotelNavn = reader.GetString("Name");
                        string hotelAdr = reader.GetString("Address");
                        string image = reader.GetString("Image");
                        Hotel hotel = new Hotel(hotelNr, hotelNavn, hotelAdr, image);
                        hoteller.Add(hotel);
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
            return hoteller;
        }

        public async Task<Hotel> GetHotelFromIdAsync(int hotelNr)
        {
            Hotel hotel = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(findHotelByIDSql, connection);
                    command.Parameters.AddWithValue("@ID", hotelNr);

                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    //Thread.Sleep(1000); // Sleep for 1 second.
                    while (await reader.ReadAsync())
                    {
                        int no = reader.GetInt32("Hotel_No");
                        string navn = reader.GetString("Name");
                        string adr = reader.GetString("Address");
                        string img = reader.GetString("Image");
                        hotel = new Hotel(no, navn, adr, img);
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
            return hotel;
        }

        public async Task<List<Hotel>> GetHotelsByNameAsync(string name)
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(findHotelsByNameSql, connection);
                    command.Parameters.AddWithValue("@Name", "%" + name + "%");
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int hotelNr = reader.GetInt32("Hotel_No");
                        string hotelNavn = reader.GetString("Name");
                        string hotelAdr = reader.GetString("Address");
                        string image = reader.GetString("Image");
                        Hotel hotel = new Hotel(hotelNr, hotelNavn, hotelAdr, image);
                        hoteller.Add(hotel);
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
            return hoteller;
        }

        public async Task<bool> UpdateHotelAsync(Hotel hotel, int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateHotelSql, connection);
                    command.Parameters.AddWithValue("@ID", hotel.HotelNr);
                    command.Parameters.AddWithValue("@Navn", hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse", hotel.Adresse);
                    command.Parameters.AddWithValue("@Billede", hotel.Image != null ? hotel.Image : "");

                    await command.Connection.OpenAsync();

                    int noOfRows = await command.ExecuteNonQueryAsync();
                    //Thread.Sleep(1000); // Sleep for 1 second.
                    Console.WriteLine($"Antal opdaterede i tabellen {noOfRows}");

                    if (noOfRows > 0) return true;
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
            return false;
        }
    }
}
