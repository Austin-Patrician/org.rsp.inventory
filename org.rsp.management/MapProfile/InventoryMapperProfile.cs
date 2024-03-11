using AutoMapper;
using org.rsp.database.Table;
using org.rsp.entity.Request;

namespace org.rsp.management.MapProfile;

public class InventoryMapperProfile : Profile
{
    public InventoryMapperProfile()
    {
        //using way is CreateMap<TSource, TDestination>();
        CreateMap<UpdateGoodsCategoryRequest, GoodsCategory>();
        CreateMap<AddGoodsCategoryRequest, GoodsCategory>();
        CreateMap<AddStoreHouseRequest, StoreHouse>();
        CreateMap<AddGoodsRequest, Goods>();
        CreateMap<AddRecordRequest, Record>();
    }
}