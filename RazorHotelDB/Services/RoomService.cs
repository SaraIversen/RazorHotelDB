using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using RazorHotelDB.Enums;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using System.Data;

namespace RazorHotelDB.Services
{
    public class RoomService : IRoomService
    {
        private string connectionString = Secret.ConnectionString;
        private string getAllRoomsSql = "select Room_No, Hotel_No, Types, Price FROM Room"; 
        private string getAllRoomsForHotelSql = "SELECT Room_No, Types, Price FROM room WHERE hotel_no = @ID";
        private string insertSql = "insert INTO Room Values(@RoomID, @HotelID, @Type, @Price)";
        private string deleteSql = "delete FROM Room WHERE Hotel_No = @HID AND Room_No = @RID";
        private string findRoomByIDSql = "select Room_No, Hotel_No, Types, Price FROM Room WHERE Hotel_No = @HID AND Room_No = @RID";
        private string updateRoomSql = "update Room SET Room_No = @NewRoomID, Types = @Type, Price = @Pris WHERE Hotel_No = @HID AND Room_No = @RID";


        public async Task<bool> CreateRoomAsync(int hotelNr, Room room)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertSql, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@RoomID", room.RoomNr);
                    command.Parameters.AddWithValue("@HotelID", hotelNr);
                    command.Parameters.AddWithValue("@Type", room.Types.ToString());
                    command.Parameters.AddWithValue("@Price", room.Pris);

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

        public async Task<Room> DeleteRoomAsync(int roomNr, int hotelNr)
        {
            Room? room = await GetRoomFromIdAsync(roomNr, hotelNr);
            if (room == null) return null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    connection.Open();

                    command.Parameters.AddWithValue("@HID", hotelNr);
                    command.Parameters.AddWithValue("@RID", roomNr);

                    int noOfRows = await command.ExecuteNonQueryAsync();
                    //Thread.Sleep(1000); // Sleep for 1 second.
                    Console.WriteLine($"Antal fjernede i tabellen {noOfRows}");

                    if (noOfRows > 0) return room;
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

        public async Task<List<Room>> GetAllRoomAsync()
        {
            List<Room> rooms = new List<Room>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getAllRoomsSql, connection);
                    command.Connection.Open();

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int roomNr = reader.GetInt32("Room_No");
                        int hotelNr = reader.GetInt32("Hotel_No");
                        string types = reader.GetString("Types");
                        double price = reader.GetDouble("Price");
                        Room room = new Room(roomNr, types, price, hotelNr);
                        rooms.Add(room);
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
            return rooms;
        }

        public async Task<List<Room>> GetAllRoomForHotelAsync(int hotelNr)
        {
            List<Room> rooms = new List<Room>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(getAllRoomsForHotelSql, connection);
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", hotelNr);

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int roomNr = reader.GetInt32("Room_No");
                        string types = reader.GetString("Types");
                        double price = reader.GetDouble("Price");
                        Room room = new Room(roomNr, types, price, hotelNr);
                        rooms.Add(room);
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
            return rooms;
        }

        public async Task<Room> GetRoomFromIdAsync(int roomNr, int hotelNr)
        {
            Room room = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(findRoomByIDSql, connection);
                    command.Parameters.AddWithValue("@HID", hotelNr);
                    command.Parameters.AddWithValue("@RID", roomNr);

                    command.Connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    //Thread.Sleep(1000); // Sleep for 1 second.
                    while (await reader.ReadAsync())
                    {
                        int rNr = reader.GetInt32("Room_No");
                        int hNr = reader.GetInt32("Hotel_No");
                        string types = reader.GetString("Types");
                        double price = reader.GetDouble("Price");
                        room = new Room(rNr, types, price, hNr);
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
            return room;
        }

        public async Task<bool> UpdateRoomAsync(Room room, int oldRoomNr, int hotelNr)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateRoomSql, connection);
                    command.Parameters.AddWithValue("@NewRoomID", room.RoomNr);
                    command.Parameters.AddWithValue("@Type", room.Types.ToString());
                    command.Parameters.AddWithValue("@Pris", room.Pris);
                    command.Parameters.AddWithValue("@HID", hotelNr);
                    command.Parameters.AddWithValue("@RID", oldRoomNr);

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
