using RideRate.Helpers;

namespace RideRate.Models
{
    public class TokenFamily : BaseClass
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
