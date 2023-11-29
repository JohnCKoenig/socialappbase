
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileAppAPI.DBModels;
using MobileAppAPI.RTS;
using MobileAppAPI.Services.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string? redisConnString = "localhost:6379,password=!Password123";

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<RedisService>(sp => new RedisService(redisConnString));

// JWT Authentication Configuration
var key = Encoding.ASCII.GetBytes("6TbhRAKWj++f8unf1eCGmsgMel+1mxvJnqMnFhrtlrs="); 

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

// Database Context Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

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