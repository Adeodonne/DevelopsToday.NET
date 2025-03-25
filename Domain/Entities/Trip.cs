namespace Domain.Entities;

public class Trip
{
    public DateTime? TPEPPickupDatetime { get; set; }
    public DateTime? TPEPDropoffDatetime { get; set; }
    public int? PassengerCount { get; set; }
    public double? TripDistance { get; set; }
    public string StoreAndFwdFlag { get; set; }
    public int? PULocationID { get; set; }
    public int? DOLocationID { get; set; }
    public decimal? FareAmount { get; set; }
    public decimal? TipAmount { get; set; }
}