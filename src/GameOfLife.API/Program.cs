using GameOfLife.Infrastructure.Repositories;
using GameOfLife.Application.Services;
using GameOfLife.Application.Interfaces;
using GameOfLife.Infrastructure;
using GameOfLife.API.Extensions;
using GameOfLife.Domain.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add MongoDB context
builder.Services.AddInfrastructure(builder.Configuration);

// Register dependencies
builder.Services.AddScoped<BoardService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new BoolMultiDimensionalArrayConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }