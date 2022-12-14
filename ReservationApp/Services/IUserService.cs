using ReservationApp.Models;
namespace ReservationApp.Services
{
    public interface IUserService
    {
        Task<Status> RegisterAsync(RegistrationModel model);
        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username);
        Task<List<Users_in_Role_ViewModel>> GetAllUsers( string roleSelect);
        Task<RegistrationModel> GetSingerUser(string userID);
    }
}
