using AutoMapper;
using org.rsp.database.Table;
using org.rsp.entity.Model;
using org.rsp.entity.Request;
using org.rsp.entity.Response;

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
        
        CreateMap<Role,RoleModel>();
        CreateMap<User,UserModel>();
        CreateMap<UserRole,UserRoleModel>();
        CreateMap<UserRoleModel,UserRole>();
        CreateMap<RolePermission,RolePermissionModel>();
        CreateMap<AddUserRequest,User>();
        CreateMap<AddRoleRequest, Role>();
        CreateMap<Goods, GoodsResponse>()
            .ForMember(g=>g.GoodsCategoryName,
            o=>o.MapFrom(s=>s.GoodsCategory.GoodsCategoryName))
            .ForMember(g=>g.StoreHouseName,
                o=>o.MapFrom(s=>s.StoreHouse.StoreHouseName));

        CreateMap<Record, RecordModel>()
            .ForMember(g => g.StoreHouseName,
                o => o.MapFrom(s => s.StoreHouse.StoreHouseName))
            .ForMember(g => g.GoodsName,
                o => o.MapFrom(s => s.Goods.GoodsName));

        CreateMap<AddWareHouseRecordRequest, Record>();

    }
}