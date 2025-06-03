namespace backend.Models
{
    public class WaterSourceUpdate
    {
        public int Id { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
        public string Descricao { get; set; }
        public WaterSourceStatus OldStatus { get; set; } = WaterSourceStatus.Potavel;
        public WaterSourceStatus Status { get; set; } = WaterSourceStatus.Potavel;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int WaterSourceId { get; set; }
        public WaterSource WaterSource { get; set; }
    }
}
