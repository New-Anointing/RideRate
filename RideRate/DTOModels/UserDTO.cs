

namespace RideRate.DTOModels
{
    public class UserDTO
    {

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Genders Gender { get; set; }
        public Roles Role { get; set; }

        public enum Roles
        {
            Contributors, Consumers
        }
        public enum Genders
        {
            Male, Female
        }
    }
}
