using CommerceApp.ConfigMiddleware;
using CommerceApp.ConfigService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigService();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigSwaggerGroup();

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
app.GetFortegyToken();
app.CustomForgeryValidate();

# endregion
app.MapControllers();

app.Run();
