using System;


namespace TransportServer.Models
{
  
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public RiskStatus HazardStatus { get; set; }
        public RiskStatus DamageStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum RiskStatus
    {
        LOW = 0,
        MODERATE = 1,
        HIGH = 2
    }
    
}
