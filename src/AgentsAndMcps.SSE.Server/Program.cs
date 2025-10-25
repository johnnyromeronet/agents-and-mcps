using AgentsAndMcps.SSE.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => 
{ 
    options.AddPolicy("CorsPolicy", policy => 
    { 
        policy.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials(); 
    }); 
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SseTransportManager>();
builder.Services.AddMcpServer().WithToolsFromAssembly(typeof(AgentsAndMcps.Console.Tools.LocationTool).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();
