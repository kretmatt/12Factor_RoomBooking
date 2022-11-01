using System.Text.Json.Serialization;
using Common.Entities;
using Common.Validators;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using RoomBooking.BusinessLogic;
using RoomBooking.BusinessLogic.Interfaces;
using RoomBooking.DataAccess;
using RoomBooking.DataAccess.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.AddControllers().AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);
builder.Services.AddLogging();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RoomBookingContext>(options =>
{
    options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString")?.Trim('\"')??throw new Exception("There is no connection string for the database"));
});

builder.Services.AddScoped<IDALRepository<Room>, GenericDALRepository<Room>>();
builder.Services.AddScoped<IDALRepository<Building>, GenericDALRepository<Building>>();
builder.Services.AddScoped<IDALRepository<Person>, GenericDALRepository<Person>>();
builder.Services.AddScoped<IDALRepository<Booking>, GenericDALRepository<Booking>>();
builder.Services.AddScoped<IRoomBookingUnitOfWork, RoomBookingUnitOfWork>();

builder.Services.AddScoped<ISimpleCRUDLogic<Room>, RoomBusinessLogic>();
builder.Services.AddScoped<ISimpleCRUDLogic<Building>, BuildingBusinessLogic>();
builder.Services.AddScoped<ISimpleCRUDLogic<Person>, PersonBusinessLogic>();
builder.Services.AddScoped<ISimpleCRUDLogic<Booking>, BookingBusinessLogic>();

builder.Services.AddScoped<IValidator<Room>, RoomValidator>();
builder.Services.AddScoped<IValidator<Building>, BuildingValidator>();
builder.Services.AddScoped<IValidator<Person>, PersonValidator>();
builder.Services.AddScoped<IValidator<Booking>, BookingValidator>();



var app = builder.Build();
app.UseForwardedHeaders();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.MapControllers();

app.Run();
