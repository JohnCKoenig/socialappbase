
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileAppAPI.DBModels;
using MobileAppAPI.RTS;
using MobileAppAPI.Services.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
//Create configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// JWT Authentication Configuration
var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]); 

//Add JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // false for development
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, //false for development
        ValidateAudience = false // false for development
    };
});

//Configure redis service
builder.Services.AddSingleton<RedisService>(sp => new RedisService(configuration));

// Database Context Configuration
string? connectionString = configuration.GetConnectionString("LocalDB");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); disabled for development

app.UseAuthentication(); // Enable JWT authentication
app.UseMiddleware<RedisTokenValidationMiddleware>(); //Add token session validation middleware

app.UseRouting();
app.UseAuthorization();
//Supress this, as we are moving mapping for organizational purposes
#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapHubs(); // Using the extension method to map hubs
});
#pragma warning restore ASP0014

app.MapControllers();


app.Run();