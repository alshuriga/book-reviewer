using BookReviewer.Shared.Entities;
using BookReviewer.Shared.MongoDb;
using BookReviewer.Shared.Repositories;
using BookReviewer.Shared.MassTransit;
using BookReviewer.Shared.Auth;
using BookReviewer.Shared.FluentValidation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidation();
builder.Services.AddMongoDbDatabase(builder.Configuration);
builder.Services.AddSingleton<IRepository<Review>, MongoDbRepository<Review>>();

builder.Services.AddMassTransitWithRabbitMQ(builder.Configuration);

builder.Services.AddJwtAuthorization(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
