using System.Net.Mail;
using BookReviewer.Email.Service;
using BookReviewer.Email.Service.Entities;
using BookReviewer.Email.Service.Repositories;
using BookReviewer.Shared.Auth;
using BookReviewer.Shared.Entities;
using BookReviewer.Shared.FluentValidation;
using BookReviewer.Shared.MassTransit;
using BookReviewer.Shared.MongoDb;
using BookReviewer.Shared.Repositories;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddJwtAuthorization(builder.Configuration);

builder.Services.AddMassTransitWithRabbitMQ(builder.Configuration);

builder.Services.AddSingleton<IEmailSubscribersRepository, MongoEmailSubscribersRepository>();
builder.Services.AddSingleton<IRepository<EmailBookInfo>, MongoDbRepository<EmailBookInfo>>();
builder.Services.AddFluentValidation();

builder.Services.AddMongoDbDatabase(builder.Configuration);

builder.Services.AddTransient<IEmailSendingService, EmailSendingService>(services => {
    var emailConfig = builder.Configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
    return new EmailSendingService(emailConfig!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
