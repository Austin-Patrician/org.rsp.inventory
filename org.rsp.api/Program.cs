using org.rsp.api.Extensions;
using org.rsp.management.MapProfile;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCors();
// config the redis:  builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.ConfigureSqlServer(builder.Configuration);
builder.Services.ConfigureRepositoryWrapper();

//DI automapper
builder.Services.AddAutoMapper(typeof(InventoryMapperProfile).Assembly);
builder.Services.RegisterAllServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AnyPolicy");

app.MapControllers();

app.Run();

