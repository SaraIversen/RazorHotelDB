using RazorHotelDB.Models;

namespace RazorHotelDB.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Indsætter en ny bruger i databasen
        /// </summary>
        /// <param name="user">den user der skal indsættes</param>
        /// <returns>Sand hvis der er gået godt ellers falsk</returns>
        Task<bool> CreateUserAsync(User user);

        /// <summary>
        /// Henter alle brugere fra databasen
        /// </summary>
        /// <returns>Liste af users</returns>
        Task<List<User>> GetAllUsers();

        /// <summary>
        /// Henter alle brugere fra databasen og tjekker om brugernavnet og kodeordet matcher med en specifik bruger
        /// </summary>
        /// <param name="userName">brugernvanet der skal indsættes</param>
        /// <param name="password">kodeordet der skal indsættes</param>
        /// <returns>Sand hvis der er gået godt ellers falsk</returns>
        Task<User?> VerifyUser(string userName, string password);
    }
}
