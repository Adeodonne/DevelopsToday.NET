using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class TripService(ITripRepository tripRepository) : ITripService
    {
        public async Task<int> GetPULocationWithHighestAvgTipAmountAsync()
        {
            return await tripRepository.GetPULocationWithHighestAvgTipAmountAsync();
        }

        public async Task<IEnumerable<Trip>> GetTop100LongestFaresByDistanceAsync()
        {
            return await tripRepository.GetTop100LongestFaresByDistanceAsync();
        }

        public async Task<IEnumerable<Trip>> GetTop100LongestFaresByTimeAsync()
        {
            return await tripRepository.GetTop100LongestFaresByTimeAsync();
        }

        public async Task<IEnumerable<Trip>> GetTripsByPULocationIDAsync(int puLocationId)
        {
            return await tripRepository.GetTripsByPULocationIDAsync(puLocationId);
        }
    }
}