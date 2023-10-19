using System.Reflection;
using DtoAbstractLayer;
using LibraryDTO;
using Microsoft.OpenApi.Models;
using MyLibraryManager;
using OpenLibraryClient;
using StubbedDTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// log 
/*
var logFactory = new LoggerFactory()
            .AddConsole(LogLevel.Debug)
            .AddDebug();

var logger = logFactory.CreateLogger<Type>();
*/ 


//var buff = System.Environment.GetEnvironmentVariable("STUBAPI", System.EnvironmentVariableTarget.Process);
var buff = "stub";
//var buff = "database";
//var buff = "other";

// Server= "mydemoserver.mariadb.database.azure.com"; 
// Port=3306;
// Database= "mysuperdatabase";
// Uid= "pierreferreira";
// Pwd="pifpwd";
// SslMode=Required;

var server = System.Environment.GetEnvironmentVariable("DB_SERVER", System.EnvironmentVariableTarget.Process);
var db = System.Environment.GetEnvironmentVariable("DB_DATABASE", System.EnvironmentVariableTarget.Process);
var username = System.Environment.GetEnvironmentVariable("DB_USER", System.EnvironmentVariableTarget.Process);
var pwd = System.Environment.GetEnvironmentVariable("DB_PASSWORD", System.EnvironmentVariableTarget.Process);

var dbPlatformPath = $"Server={server};Database={db};Uid={username};Pwd={pwd};"; 

/*var dbPlatformPath = "Server={"+ System.Environment.GetEnvironmentVariable("db_server", System.EnvironmentVariableTarget.Process) +"};" +
    "Database={"+ System.Environment.GetEnvironmentVariable("db_database", System.EnvironmentVariableTarget.Process) +"};" +
    "Uid={"+ System.Environment.GetEnvironmentVariable("db_user", System.EnvironmentVariableTarget.Process) +"};" +
    "Pwd={"+ System.Environment.GetEnvironmentVariable("db_password", System.EnvironmentVariableTarget.Process) +"};" + 
    "SslMode=Preferred;";*/

// _serviceCollection.AddSingleton<IService>(x => 
//     new Service(x.GetRequiredService<IOtherService>(),
//                 x.GetRequiredService<IAnotherOne>(), 
//                 ""));

if(buff == "stub")
{
    builder.Services.AddSingleton<IDtoManager, StubbedDTO.Stub>();
    //logger.LogInformation("__________________________________________________________________________________________________ UTILISATION DU STUB __________________________________________________________________________________________________");
}
else if (buff == "database") {
    builder.Services.AddSingleton<IDtoManager, MyLibraryManager.MyLibraryMgr>( x => new MyLibraryMgr(dbPlatformPath));
    //logger.LogInformation("===================================================================================== UTILISATION DE LA BASE DE DONNée =====================================================================================");
}
else
{
    builder.Services.AddSingleton<IDtoManager, OpenLibClientAPI>();
    //logger.LogInformation("------------------------------------------------------------------------------- UTILISATION DE L API -------------------------------------------------------------------------------");
}

//builder.Services.AddSingleton<IDtoManager,OpenLibClientAPI>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


