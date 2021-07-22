using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using TransportGrpc.Protos.Item;

namespace TransportServer.Configuration
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            
            CreateMap<Models.Item, ItemModel>()
                .ForMember(x => x.CreatedAt,
                opt => opt.MapFrom(src => Timestamp.FromDateTime(src.CreatedAt)));

            CreateMap<ItemModel, Models.Item>()
                .ForMember(x => x.CreatedAt,
                opt => opt.MapFrom(src => src.CreatedAt.ToDateTime()));
            
        }
    }
}
