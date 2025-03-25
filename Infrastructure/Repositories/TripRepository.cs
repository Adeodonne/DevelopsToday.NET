using Dapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class TripRepository(IDatabaseFactory databaseFactory) : ITripRepository
    {
        public async Task BulkInsertTripsAsync(IEnumerable<Trip> trips)
        {
            using var connection = databaseFactory.GetConnection();
            connection.Open();

            var query = @"
                INSERT INTO dbo.Trips (TPEPPickupDatetime, TPEPDropoffDatetime, PassengerCount, TripDistance, StoreAndFwdFlag, PULocationID, DOLocationID, FareAmount, TipAmount)
                VALUES (@TPEPPickupDatetime, @TPEPDropoffDatetime, @PassengerCount, @TripDistance, @StoreAndFwdFlag, @PULocationID, @DOLocationID, @FareAmount, @TipAmount)";

            await connection.ExecuteAsync(query, trips);
        }

        public async Task<int> GetPULocationWithHighestAvgTipAmountAsync()
        {
            using var connection = databaseFactory.GetConnection();
            connection.Open();

            var query = @"
                SELECT TOP 1 PULocationID
                FROM dbo.Trips
                GROUP BY PULocationID
                ORDER BY AVG(TipAmount) DESC";

            return await connection.QuerySingleOrDefaultAsync<int>(query);
        }

        public async Task<IEnumerable<Trip>> GetTop100LongestFaresByDistanceAsync()
        {
            using var connection = databaseFactory.GetConnection();
            connection.Open();

            var query = @"
                SELECT TOP 100 *
                FROM dbo.Trips
                ORDER BY TripDistance DESC";

            return await connection.QueryAsync<Trip>(query);
        }

        public async Task<IEnumerable<Trip>> GetTop100LongestFaresByTimeAsync()
        {
            using var connection = databaseFactory.GetConnection();
            connection.Open();

            var query = @"
                SELECT TOP 100 *
                FROM dbo.Trips
                WHERE TPEPPickupDatetime IS NOT NULL AND TPEPDropoffDatetime IS NOT NULL
                ORDER BY DATEDIFF(MINUTE, TPEPPickupDatetime, TPEPDropoffDatetime) DESC";

            var trips = await connection.QueryAsync<Trip>(query);
            return trips.ToList();
        }

        public async Task<IEnumerable<Trip>> GetTripsByPULocationIDAsync(int puLocationId)
        {
            using var connection = databaseFactory.GetConnection();
            connection.Open();

            var query = "SELECT * FROM dbo.Trips WHERE PULocationID = @PULocationID";

            return await connection.QueryAsync<Trip>(query, new { PULocationID = puLocationId });
        }
    }
}