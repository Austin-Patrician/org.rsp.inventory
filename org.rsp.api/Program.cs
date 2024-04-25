using org.rsp.api.Auth;
using org.rsp.api.Extensions;
using org.rsp.entity.Helper;
using org.rsp.management.MapProfile;

var builder = WebApplication.CreateBuilder(args);


builder.JwtAuthentication();
builder.Services.ConfigureCors();

builder.Services.AddControllers();
// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// config the redis:  builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.ConfigureSqlServer(builder.Configuration);
builder.Services.ConfigureRepositoryWrapper();
//注册过滤器
builder.Services.AddControllers(o =>
{
    o.Filters.Add<YesAttribute>();
});

builder.Services.AddMemoryCache();
//DI automapper
builder.Services.AddAutoMapper(typeof(InventoryMapperProfile).Assembly);
builder.Services.RegisterAllServices();
builder.Services.AddSingleton<MyRedis<string,string>>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AnyPolicy");

app.MapControllers();

app.Run();

