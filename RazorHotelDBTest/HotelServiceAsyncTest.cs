using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using RazorHotelDB.Interfaces;
using RazorHotelDB.Models;
using RazorHotelDB.Services;

namespace RazorHotelDBTest
{
    [TestClass]
    public sealed class HotelServiceAsyncTest
    {
        #region HotelService Tests
        [TestMethod]
        public async Task TestCreateHotelAsyncException()
        {
            // Arrange
            IHotelServiceAsync hotelService = new HotelService();
            List<Hotel> hotels = hotelService.GetAllHotelAsync().Result;

            Hotel aHotelInDB = hotels.First(); // Getting the first found hotel from the DB.
            Hotel newHotel = new Hotel(aHotelInDB.HotelNr, "Hotelnavn", "Testvej 123", ""); // Creating a hotel with an ID that already exists in the database!

            // Act & Assert
            await Assert.ThrowsExceptionAsync<SqlException>(async () =>
            {
                await hotelService.CreateHotelAsync(newHotel);
            });
        }
        #endregion

        #region RoomService Tests
        [TestMethod]
        public async Task TestGetRoomFromIdSuccess()
        {
            // Arrange
            IRoomService roomService = new RoomService();
            List<Room> rooms = roomService.GetAllRoomAsync().Result;

            Room aRoomInDB = rooms.First(); // Getting the first found room from the DB.

            // Act
            Room roomFromId = await roomService.GetRoomFromIdAsync(aRoomInDB.RoomNr, aRoomInDB.HotelNr); // Checking if the method returns a room from an ID (that we know exists).

            // Assert
            Assert.IsNotNull(roomFromId);
        }
        #endregion

        #region UserService Tests
        [TestMethod]
        public async Task TestVerifyUserFalse()
        {
            // Arrange
            IUserService userService = new UserService();
            User user = new User(-1, "testUsername", "testPassword", "");

            // Act
            User? loginUser = await userService.VerifyUser(user.UserName, user.Password);

            // Assert
            Assert.IsNull(loginUser);
        }
        #endregion
    }
}
