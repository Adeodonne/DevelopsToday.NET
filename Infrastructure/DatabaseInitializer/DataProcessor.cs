using System.Collections;
using System.Globalization;
using CsvHelper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Mappers;
using TimeZoneConverter;

namespace Infrastructure.DatabaseInitializer;

public class DataProcessor(ITripRepository tripRepository) : IDataProcessor
{
    public async Task ProcessCsv()
    {
        string inputPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Infrastructure", "DatabaseInitializer",
            "CSVData",
            "sample-cab-data.csv");
        string duplicatesPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Infrastructure",
            "DatabaseInitializer", "CSVData",
            "duplicates.csv");

        var estTimeZone = TZConvert.GetTimeZoneInfo("Eastern Standard Time");

        using var reader = new StreamReader(inputPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<TripMap>();

        var trips = csv.GetRecords<Trip>()
            .Where(r => r is { PassengerCount: > 0, TripDistance: not null } &&
                        !string.IsNullOrEmpty(r.StoreAndFwdFlag) &&
                        r is
                        {
                            PULocationID: not null, DOLocationID: not null, FareAmount: not null,
                            TipAmount: not null
                        })
            .ToList();

        foreach (var trip in trips)
        {
            if (trip.TPEPPickupDatetime.HasValue)
            {
                trip.TPEPPickupDatetime =
                    TimeZoneInfo.ConvertTimeToUtc(trip.TPEPPickupDatetime.Value, estTimeZone);
            }

            if (trip.TPEPDropoffDatetime.HasValue)
            {
                trip.TPEPDropoffDatetime =
                    TimeZoneInfo.ConvertTimeToUtc(trip.TPEPDropoffDatetime.Value, estTimeZone);
            }

            trip.StoreAndFwdFlag = trip.StoreAndFwdFlag?.Trim();
        }

        trips.RemoveAll(trip =>
        {
            if (trip.TPEPPickupDatetime == null || trip.TPEPDropoffDatetime == null)
            {
                return true;
            }

            switch (trip.StoreAndFwdFlag)
            {
                case "N" or "0":
                    trip.StoreAndFwdFlag = "No";
                    return false;
                case "Y" or "1":
                    trip.StoreAndFwdFlag = "Yes";
                    return false;
                default:
                    return true;
            }
        });

        var duplicates = trips
            .GroupBy(r => new
            {
                tpep_pickup_datetime = r.TPEPPickupDatetime, tpep_dropoff_datetime = r.TPEPDropoffDatetime,
                passenger_count = r.PassengerCount
            })
            .Where(g => g.Count() > 1)
            .SelectMany(g => g.Skip(1))
            .ToList();

        trips.RemoveAll(r => duplicates.Contains(r));

        await using var writer = new StreamWriter(duplicatesPath);
        await using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csvWriter.WriteRecordsAsync((IEnumerable)duplicates);

        Console.WriteLine("Count of trips: " + trips.Count);
        await tripRepository.BulkInsertTripsAsync(trips);
    }
}