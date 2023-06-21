using RideRate.Helpers;

namespace RideRate.Models
{
    public class Location : BaseClass
    {
        public Guid Id { get; set; }
        public string Start { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
