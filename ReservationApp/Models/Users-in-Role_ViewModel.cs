using MessagePack;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Models
{
    public class Users_in_Role_ViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string Role { get; set; }
    }
}
