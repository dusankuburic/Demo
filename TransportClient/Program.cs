using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using TransportGrpc.Protos.Item;

namespace TransportClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Waiting for server to run...");
            Thread.Sleep(2000);

            using var chanel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new ItemProtoService.ItemProtoServiceClient(chanel);

            //await GetItem(client);
            //await GetAllItems(client);
            //await AddItem(client);
            //await DeleteItem(client);
            //await GetAllItems(client);
            //await UpdateItem(client);

            await AddMultipleItems(client);
            await GetAllItems(client);

            Console.ReadLine();
        }

        private static async Task GetItem(ItemProtoService.ItemProtoServiceClient client)
        {
            Console.WriteLine("GetItem started...");
            var response = await client.GetItemAsync(
                new GetItemRequest
                { 
                    Id = 1
                });
            Console.WriteLine($"GetItemAsync Response: { JsonConvert.SerializeObject(response, Formatting.Indented)}");
        }


        private static async Task GetAllItems(ItemProtoService.ItemProtoServiceClient client)
        {
            Console.WriteLine("GetAllItems started...");
            using var response = client.GetAllItems(new GetAllItemsRequest());
            await foreach(var data in response.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
        }

        private static async Task AddItem(ItemProtoService.ItemProtoServiceClient client)
        {
            Console.WriteLine("AddItem started ...");
            var response = await client.AddItemAsync(
                new AddItemRequest
                {
                    Item = new ItemModel
                    {
                        Name = "Zlato",
                        Description = "SAMO DA SE SIJA",
                        Price = 50000,
                        DamageStatus = RiskStatus.High,
                        HazardStatus = RiskStatus.Low,
                        CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                    }
                });
            Console.WriteLine($"AddItemAsync Response: {response}");
        }

        private static async Task UpdateItem(ItemProtoService.ItemProtoServiceClient client)
        {
            Console.WriteLine("UpdateItem started...");
            var response = await client.UpdateItemAsync(
                new UpdateItemRequest
                {
                    Item = new ItemModel
                    {
                        Id = 1,
                        Name = "Ziva",
                        Description = "Oznaka je Hg",
                        Price = 534,
                        DamageStatus = RiskStatus.Moderate,
                        HazardStatus = RiskStatus.High,
                        CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                    }
                });

            Console.WriteLine($"UpdateItemAsync Response: {response}");
        }

        private static async Task DeleteItem(ItemProtoService.ItemProtoServiceClient client)
        {
            Console.WriteLine("DeleteItem started...");
            var response = await client.DeleteItemAsync(
                new DeleteItemRequest
                {
                    ItemId = 3
                });
            Console.WriteLine($"DeletedProductAsync Response: {response.Success}");
        }


        private static async Task AddMultipleItems(ItemProtoService.ItemProtoServiceClient client)
        {
            Console.WriteLine("AddMultipleItems started...");

            using var clientStream = client.AddMultipleItems();

            for(int i = 1; i <= 10000; i++)
            {
                var itemModel = new ItemModel
                {
                    Name = $"Element {i}",
                    Description = $"Multiple item insert {i}",
                    Price = i,
                    DamageStatus = RiskStatus.High,
                    HazardStatus = RiskStatus.High,
                    CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                };

                await clientStream.RequestStream.WriteAsync(itemModel);
            }

            await clientStream.RequestStream.CompleteAsync();

            var responseStream = await clientStream;

            Console.WriteLine($"Status: {responseStream.Success}. Insert Count: {responseStream.InsertCount}");
        }

    }
}
