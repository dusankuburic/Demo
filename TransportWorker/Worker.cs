using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using TransportGrpc.Protos.Item;

namespace TransportWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly ItemGenerator _generator;

        public Worker(
            ILogger<Worker> logger, 
            IConfiguration config, 
            ItemGenerator generator)
        {
            _logger = logger;
            _config = config;
            _generator = generator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Console.WriteLine("Waiting for server is running...");
            Thread.Sleep(2000);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

                using var channel = GrpcChannel.ForAddress(_config.GetValue<string>("WorkerService:ServerUrl"));
                var client = new ItemProtoService.ItemProtoServiceClient(channel);

                _logger.LogInformation("AddItemAsync started ...");
                var response = await client.AddItemAsync(await _generator.Do());
                _logger.LogInformation($"AddItem Response: {JsonConvert.SerializeObject(response, Formatting.Indented)}");

                await Task.Delay(_config.GetValue<int>("WorkerService:TaskInterval"), stoppingToken);
            }
        }
    }
}
