using CsvHelper.Configuration;
using Domain.Entities;
using Infrastructure.Mappers.Converters;

namespace Infrastructure.Mappers;

public sealed class TripMap : ClassMap<Trip>
{
    public TripMap()
    {
        Map(m => m.TPEPPickupDatetime)
            .Name("tpep_pickup_datetime")
            .TypeConverter<NullableDateTimeConverter>();

        Map(m => m.TPEPDropoffDatetime)
            .Name("tpep_dropoff_datetime")
            .TypeConverter<NullableDateTimeConverter>();

        Map(m => m.PassengerCount).Name("passenger_count");
        Map(m => m.TripDistance).Name("trip_distance");
        Map(m => m.StoreAndFwdFlag).Name("store_and_fwd_flag");
        Map(m => m.PULocationID).Name("PULocationID");
        Map(m => m.DOLocationID).Name("DOLocationID");
        Map(m => m.FareAmount).Name("fare_amount");
        Map(m => m.TipAmount).Name("tip_amount");
    }
}