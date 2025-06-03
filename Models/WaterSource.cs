namespace backend.Models
{
    public class WaterSource
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Localizacao { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public WaterSourceType Type { get; set; } 
        public int CreatedById { get; set; }
        public DateTime LastInspected { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public WaterSourceStatus Status { get; set; } = WaterSourceStatus.Potavel;
        public List<WaterSourceUpdate> WaterSourceUpdates { get; set; } = new List<WaterSourceUpdate>();
    }
    public enum WaterSourceType
    {
        Poco,
        Fonte,
        Rio,
        Lago,
        Reservatorio
    }
    public enum WaterSourceStatus
    {
        Potavel,
        Contaminada
    }
}
