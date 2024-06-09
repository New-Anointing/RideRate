using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;



namespace RideRate.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string HomeAddress { get; set; } = string.Empty;
        [Required]
        public string Gender { get; set; } = string.Empty;
        [Required] 
        public string Role { get; set; } = string.Empty;
        public Guid AppUserId { get; set; }
        public string RefershToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        [NotMapped]
        public int[]? VerificationToken { get; set; }
        public bool Verified { get; set; } = false;
        public DateTime VerifiedAt { get; set; }

        public string VerificationTokenJson
        {
            get => JsonSerializer.Serialize(VerificationToken);
            set => VerificationToken =  string.IsNullOrEmpty(value) ? null : JsonSerializer.Deserialize<int[]>(value);
        }
    }
}
