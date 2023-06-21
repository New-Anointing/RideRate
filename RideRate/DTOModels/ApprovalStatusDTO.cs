namespace RideRate.DTOModels
{
    public class ApprovalStatusDTO
    {
        public ReviewStatus Status { get; set; }

        public enum ReviewStatus
        {
            Approved,
            Rejected
        }
    }
}
