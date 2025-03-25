using Domain.Entities;

namespace Domain.Interfaces;

public interface ITripRepository
{
    Task BulkInsertTripsAsync(IEnumerable<Trip> trips);
    Task<int> GetPULocationWithHighestAvgTipAmountAsync();
    Task<IEnumerable<Trip>> GetTop100LongestFaresByDistanceAsync();
    Task<IEnumerable<Trip>> GetTop100LongestFaresByTimeAsync();
    Task<IEnumerable<Trip>> GetTripsByPULocationIDAsync(int puLocationId);
}