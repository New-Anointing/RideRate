using Microsoft.EntityFrameworkCore;
using RideRate.Data;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;
using RideRate.Services.UserResolver;
using RideRate.Utilities;
using System.Net;

namespace RideRate.Services.Rates
{
    public class RateServices : IRatesServices
    {
        private readonly ApiDbcontext _context;
        private readonly IUserResolverService _iuserResolverService;
        private Rate rate = new();

        public RateServices
        (
            ApiDbcontext context,
            IUserResolverService iuserResolverService
        )
        {
            _context = context;
            _iuserResolverService = iuserResolverService;
        }
        private Guid appUserId => _iuserResolverService.GetUniqueAppUserId(); 
        private GenericResponse<Rate> errResponse => _iuserResolverService.RateGenericResponse();
        private GenericResponse<IEnumerable<Rate>> listErrResponse => _iuserResolverService.RateListGenericResponse();

        //Create rates
        public async Task<GenericResponse<Rate>> Create(Guid locationId, RateDTO request)
        {
            try
            {
                var locationExist = await _context.Location.FirstOrDefaultAsync(l=> l.Id == locationId && l.AppUserId == appUserId);
                if (locationExist == null)
                {
                    return new GenericResponse<Rate>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Data = null,
                        Message = "No location with this id exist",
                        Success = false
                    };
                }

                rate.AppUserId = appUserId;
                rate.Id = Guid.NewGuid();
                rate.Location = locationExist;
                rate.Price1 = request.Price1;
                rate.Price2 = request.Price2;


                await _context.Rate.AddAsync(rate);
                await _context.SaveChangesAsync();

                return new GenericResponse<Rate>
                {
                    StatusCode = HttpStatusCode.Created,
                    Data = rate,
                    Message = "Rates Created Successfully",
                    Success = true
                };
            }catch(Exception ex)
            {
                errResponse.Message = $"An error occured {ex.Message}";
                return errResponse;
            }

        }

        //get all approved rates for the users
        public async Task<GenericResponse<IEnumerable<Rate>>> GetAllRates()
        {
            try
            {
                var rates = await _context.Rate.Where(r => r.IsApproved == true && r.IsDeleted == false).Include(l => l.Location).ToListAsync();
                if(rates == null)
                {
                    return new GenericResponse<IEnumerable<Rate>>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No Rates Have been created yet",
                        Success = true
                    };
                }

                return new GenericResponse<IEnumerable<Rate>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = rates,
                    Message = "Data Loaded Successfully",
                    Success = true
                };
            }catch(Exception ex)
            {
                listErrResponse.Message = $"An error occured {ex.Message}";
                return listErrResponse;
            }
        }

        //get all rates not sent for approval
        public async Task<GenericResponse<IEnumerable<Rate>>> UserRates()
        {
            try
            {
                var rates = await _context.Rate.Include(l => l.Location).Where(l=>l.AppUserId == appUserId && l.SentForApproval == false && l.IsDeleted == false).ToListAsync();
                if(rates == null)
                {
                    return new GenericResponse<IEnumerable<Rate>>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No Rates Have been created yet",
                        Success = true
                    };
                }

                return new GenericResponse<IEnumerable<Rate>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = rates,
                    Message = "Data Loaded Successfully",
                    Success = true
                };
            }catch(Exception ex)
            {
                listErrResponse.Message = $"An error occured {ex.Message}";
                return listErrResponse;
            }
        }

        //get all rates created
        public async Task<GenericResponse<IEnumerable<Rate>>> Rates()
        {
            try
            {
                var rates = await _context.Rate.Include(l => l.Location).Where(l=>l.AppUserId == appUserId && l.IsDeleted == false).ToListAsync();
                if(rates == null)
                {
                    return new GenericResponse<IEnumerable<Rate>>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No Rates Have been created yet",
                        Success = true
                    };
                }

                return new GenericResponse<IEnumerable<Rate>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = rates,
                    Message = "Data Loaded Successfully",
                    Success = true
                };
            }catch(Exception ex)
            {
                listErrResponse.Message = $"An error occured {ex.Message}";
                return listErrResponse;
            }
        }

        //get all rates sent for approval
        public async Task<GenericResponse<IEnumerable<Rate>>> SentRates()
        {
            try
            {
                var rates = await _context.Rate.Include(l => l.Location).Where(l=>l.AppUserId == appUserId && l.SentForApproval == true && l.IsApproved == false && l.Status == SD.Pending && l.IsDeleted == false).ToListAsync();
                if(rates == null)
                {
                    return new GenericResponse<IEnumerable<Rate>>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No Rates Have been created yet",
                        Success = true
                    };
                }

                return new GenericResponse<IEnumerable<Rate>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = rates,
                    Message = "Data Loaded Successfully",
                    Success = true
                };
            }catch(Exception ex)
            {
                listErrResponse.Message = $"An error occured {ex.Message}";
                return listErrResponse;
            }
        }

        //edit unsent rate 
        public async Task<GenericResponse<Rate>> Edit(Guid Id, RateDTO request)
        {
            try
            {
                var rateExist = await _context.Rate.FirstOrDefaultAsync(r => r.Id == Id && r.SentForApproval == false);
                if(rateExist == null)
                {
                    return new GenericResponse<Rate>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "Rate does not exist",
                        Success = false,
                    };
                }
                rateExist.Price1 = request.Price1;
                rateExist.Price2 = request.Price2;
                rateExist.DateUpdated = DateTime.Now;
                rateExist.DateModified = DateTime.Now;

                _context.Update(rateExist);
                await _context.SaveChangesAsync();
                return new GenericResponse<Rate>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = rateExist,
                    Message = "rate updated successfully",
                    Success = true
                };

            }
            catch(Exception ex)
            {
                errResponse.Message = $"An error occured {ex.Message}";
                return errResponse;
            }

        }

        //Delete unsent rate
        public async Task<GenericResponse<Rate>> Delete(Guid Id)
        {
            try
            {
                var rateExist = await _context.Rate.FirstOrDefaultAsync(r => r.Id == Id && r.SentForApproval == false && r.IsDeleted == false);
                if (rateExist == null)
                {
                    return new GenericResponse<Rate>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "Rate does not exist",
                        Success = false,
                    };
                }

                _context.Rate.Remove(rateExist);
                await _context.SaveChangesAsync();

                return new GenericResponse<Rate>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = null,
                    Message = "Rate deleted successfully",
                    Success = true
                };
            }
            catch(Exception ex)
            {
                errResponse.Message = $"An error occured {ex.Message}";
                return errResponse;
            }
        }

        //send rates for approval
        public async Task<GenericResponse<IEnumerable<Rate>>> SendForApproval()
        {
            try
            {
                var ratesForApproval = await _context.Rate.Where(r=> r.AppUserId == appUserId && r.SentForApproval == false && r.IsDeleted == false ).Include(r=> r.Location).ToListAsync();
                if(!ratesForApproval.Any())
                {
                    return new GenericResponse<IEnumerable<Rate>>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "No Pending rates to be reviewed",
                        Success = false
                    };
                }

                foreach(var rates in ratesForApproval)
                {
                    rates.SentForApproval= true;
                    rates.Status = SD.Pending;
                }
                _context.UpdateRange(ratesForApproval);
                await _context.SaveChangesAsync();
                return new GenericResponse<IEnumerable<Rate>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = ratesForApproval,
                    Message = "Sent for approval successfully",
                    Success = true
                };
            }
            catch(Exception ex)
            {
                listErrResponse.Message = $"An error occured {ex.Message}";
                return listErrResponse;
            }
        }

        //Approve rate 
        public async Task<GenericResponse<Rate>> ApproveRate(Guid Id, ApprovalStatusDTO request)
        {
            try
            {
                var rateToBeApproved =  await _context.Rate.FirstOrDefaultAsync(r => r.Id == Id && r.SentForApproval == true && r.IsDeleted == false);
                if(rateToBeApproved == null)
                {
                    return new GenericResponse<Rate>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Data = null,
                        Message = "rate has not been sent for approval or rate does not exist",
                        Success = false
                    };
                }
                rateToBeApproved.Status = request.Status.ToString();

                if(rateToBeApproved.Status == SD.Approved)
                {
                    rateToBeApproved.IsApproved = true;
                }



                _context.Update(rateToBeApproved);
                await _context.SaveChangesAsync();

                return new GenericResponse<Rate>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = rateToBeApproved,
                    Message = "rate has been successfully reviewed",
                    Success = true
                };
                
            }
            catch(Exception ex)
            {
                errResponse.Message = $"An error occured {ex.Message}";
                return errResponse;
            }
        }

        //Remove outdated rates 
        public async Task<GenericResponse<Rate>> RemoveRate(Guid Id)
        {
            try
            {
                var rateExist = await _context.Rate.FirstOrDefaultAsync(r=>r.Id == Id && r.IsApproved == true && r.IsDeleted == false);
                if(rateExist == null)
                {
                    return new GenericResponse<Rate>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Data = null,
                        Message = "No such rate exist",
                        Success = false
                    };
                }

                rateExist.IsDeleted = true;
                _context.Update(rateExist);
                await _context.SaveChangesAsync();
                return new GenericResponse<Rate>
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Data = null,
                    Message = "Rate has ben successfully removed",
                    Success = true
                };
                
            }
            catch(Exception ex)
            {
                errResponse.Message = $"An error occured {ex.Message}";
                return errResponse;
            }
        }
    }
}
