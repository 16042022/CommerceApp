using CommerceApp.ConfigMiddleware;
using CommerceApp.ConfigService;
using CommerceCore.Application.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigService();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigSwaggerGroup();

// Get the connection DB string
builder.Services.AddOptions();
var DbConnect = builder.Configuration.GetSection("MongoConnection").GetSection("shopDevDB");
builder.Services.Configure<ShopDevDBSetting>(DbConnect);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.CustomSwaggerUX();
}

app.UseHttpsRedirection();
// Compression middleware
app.UseResponseCompression();
// Add static file here (if need)
#region CustomMiddleware
app.UseHttpLogging();
// Exception handle middleware
app.HandleException();

// Anti forgery & other security config
app.SecurityMiddleware();

# endregion
app.MapControllers();

app.Run();
