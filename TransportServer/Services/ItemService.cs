using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportGrpc.Protos.Item;
using TransportServer.Data;
using TransportServer.Models;

namespace TransportServer.Services
{
    public class ItemService: ItemProtoService.ItemProtoServiceBase
    {
        private readonly TransportContext _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemService> _logger;

        public ItemService(
            TransportContext ctx,
            IMapper mapper,
            ILogger<ItemService> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }
        public override async Task<ItemModel> GetItem(
            GetItemRequest request,
            ServerCallContext context)
        {
            var item = await _ctx.Items.FindAsync(request.Id);
            if (item is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Item with id={request.Id} is not found"));
            }

            var itemModel = _mapper.Map<ItemModel>(item);

            return itemModel;
        }


        public override async Task GetAllItems(
            GetAllItemsRequest request, 
            IServerStreamWriter<ItemModel> responseStream, 
            ServerCallContext context)
        {
            var items = await _ctx.Items.ToListAsync();

            foreach(var item in items)
            {
                var itemModel = _mapper.Map<ItemModel>(item);
                await responseStream.WriteAsync(itemModel);
            }
        }

        public override async Task<ItemModel> AddItem(
            AddItemRequest request,
            ServerCallContext context)
        {
            var item = _mapper.Map<Item>(request.Item);

            _ctx.Items.Add(item);
            await _ctx.SaveChangesAsync();

            _logger.LogInformation($"Item successfully added: {item.Id}:{item.Name}");

            var itemModel = _mapper.Map<ItemModel>(item);
            return itemModel;
        }


        public override async Task<ItemModel> UpdateItem(
            UpdateItemRequest request, 
            ServerCallContext context)
        {
            var item = _mapper.Map<Item>(request.Item);

            bool isExist = await _ctx.Items
                .AnyAsync(x => x.Id == item.Id);
            if(!isExist)
            {
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Item with ID={item.Id} is not found"));
            }

            _ctx.Entry(item).State = EntityState.Modified;

            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            var itemModel = _mapper.Map<ItemModel>(item);
            return itemModel;
        }

        public override async Task<DeleteItemResponse> DeleteItem(
            DeleteItemRequest request, 
            ServerCallContext context)
        {
            var item = await _ctx.Items.FindAsync(request.ItemId);

            if(item is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound,
                    $"Item with id={request.ItemId} is not found"));
            }

            _ctx.Items.Remove(item);
            var deletedCount = await _ctx.SaveChangesAsync();

            var response = new DeleteItemResponse
            {
                Success = deletedCount > 0
            };

            return response;
        }

        public override async Task<AddMultipleItemsResponse> AddMultipleItems(
            IAsyncStreamReader<ItemModel> requestStream,
            ServerCallContext context)
        {
            while(await requestStream.MoveNext())
            {
                var item = _mapper.Map<Item>(requestStream.Current);
                _ctx.Items.Add(item);
            }

            var insertCount = await _ctx.SaveChangesAsync();

            var response = new AddMultipleItemsResponse
            {
                Success = insertCount > 0,
                InsertCount = insertCount
            };

            return response;
        }
    }
}
