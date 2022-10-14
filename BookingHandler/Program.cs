using Microsoft.OpenApi.Models;
using BookingHandler.Services;
using BookingHandler.Repositories;
using BookingHandler.Data;

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

builder.Services.AddSingleton<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingContext, BookingContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


// app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

app.UseAuthorization();

app.MapControllers();

app.Run();
