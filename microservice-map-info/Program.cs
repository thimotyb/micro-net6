using GoogleMapInfo;
using microservice_map_info.Services;
using Serilog;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog support
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
   // .WriteTo.Seq("http://localhost:5341")
    .ReadFrom.Configuration(ctx.Configuration));

Serilog.Debugging.SelfLog.Enable(Console.Error);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<GoogleDistanceApi>();

// Add for GRPC Support
builder.Services.AddGrpc();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

// Expose promotheus metrics
app.UseHttpMetrics();


app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DistanceInfoService>();
    endpoints.MapMetrics(); // endpoint per Prometheus
});

app.Run();
