using System.ComponentModel.DataAnnotations;
using RideRate.Helpers;

namespace RideRate.Models
{
    public class Rate : BaseClass
    {
        public Guid Id { get; set; }
        [Required]
        public double Price1 { get; set; }
        public double Price2 { get; set; }
        [Required]
        public virtual Location Location { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;
        public bool SentForApproval { get; set; } = false;
    }

}
