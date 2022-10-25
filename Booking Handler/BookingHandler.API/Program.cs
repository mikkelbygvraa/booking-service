using Microsoft.OpenApi.Models;

using BookingHandler.Data;
using BookingHandler.Services;
using BookingHandler.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddHostedService<BookingWorker>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking Handler Service", Version = "v1" });
});

#region Project Dependencies

builder.Services.AddSingleton<IBookingContext, BookingContext>();
builder.Services.AddSingleton<IBookingRepository, BookingRepository>();

#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking Handler Service V1");
});


app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

app.UseAuthorization();

app.MapControllers();

app.Run();
