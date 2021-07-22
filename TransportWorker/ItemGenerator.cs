using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TransportGrpc.Protos.Item;

namespace TransportWorker
{
    public class ItemGenerator
    {
        private readonly ILogger<ItemGenerator> _logger;
        private readonly IConfiguration _config;

        public ItemGenerator(
            ILogger<ItemGenerator> logger, 
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public Task<AddItemRequest> Do()
        {
            var itemName = $"{_config.GetValue<string>("WorkerService:ItemName")} :: {DateTimeOffset.Now}";
            var request = new AddItemRequest
            {
                Item = new ItemModel
                {
                    Name = itemName,
                    Description = $"desc: {itemName}",
                    Price = new Random().Next(1000),
                    DamageStatus = RiskStatus.Moderate,
                    HazardStatus = RiskStatus.Low,
                    CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                }
            };

            return Task.FromResult(request);
        }

    }
}
