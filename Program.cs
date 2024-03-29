using MongoDB.Driver;
using shiroDotnetRestfulDocker.Models;
using shiroDotnetRestfulDocker.Repositories;

var builder = WebApplication.CreateBuilder(args);

// for logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// enable cross origin 
var LocalhostDev = "_localhostDev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: LocalhostDev,
        policy =>
        {
            policy.WithOrigins("*",
                                "http://www.contoso.com");
        });
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<OrderJnjDatabaseSettings>(
    builder.Configuration.GetSection("OrderJnjDatabase"));

builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
{
    //Set a secret
    //dotnet user-secrets set "Movies:ServiceApiKey" "12345"--project "C:\apps\WebApp1\src\WebApp1"

    var restaurantsApiKey = builder.Configuration["Orderjnj:ServiceApiKey"];
    var restaurantsConnectionUrl = builder.Configuration["Orderjnj:ConnectionUrl"];

    var mongoUrlBuilder = new MongoUrlBuilder();
    mongoUrlBuilder.Parse(restaurantsConnectionUrl);
    mongoUrlBuilder.Username = "admin";
    mongoUrlBuilder.Password = restaurantsApiKey;
    var settings = MongoClientSettings.FromConnectionString(mongoUrlBuilder.ToString());
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);
    return new MongoClient(settings);
});

builder.Services.AddSingleton<UserCredentialsRepository>();
builder.Services.AddSingleton<RestaurantsRepository>();
builder.Services.AddSingleton<FoodOrdersRepository>();
builder.Services.AddSingleton<UserProfilesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(LocalhostDev);

app.UseAuthorization();

app.MapControllers();

app.Run();
