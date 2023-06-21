using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;

namespace RideRate.Services.Rates
{
    public interface IRatesServices
    {
        public Task<GenericResponse<Rate>> Create(Guid locationId, RateDTO request);
        public Task<GenericResponse<IEnumerable<Rate>>> GetAllRates();
        public Task<GenericResponse<IEnumerable<Rate>>> UserRates();
        public Task<GenericResponse<IEnumerable<Rate>>> Rates();
        public Task<GenericResponse<IEnumerable<Rate>>> SendForApproval();
        public Task<GenericResponse<IEnumerable<Rate>>> SentRates();
        public Task<GenericResponse<Rate>> Edit(Guid Id, RateDTO request);
        public Task<GenericResponse<Rate>> Delete(Guid Id);
        public Task<GenericResponse<Rate>> ApproveRate(Guid Id, ApprovalStatusDTO request);
        public Task<GenericResponse<Rate>> RemoveRate(Guid Id);
    }
}
