using System.Net.Mail;
using BookReviewer.Email.Service;
using BookReviewer.Email.Service.Repositories;
using BookReviewer.Shared.Auth;
using BookReviewer.Shared.MassTransit;
using BookReviewer.Shared.MongoDb;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddJwtAuthorization(builder.Configuration);

builder.Services.AddMassTransitWithRabbitMQ(builder.Configuration);

builder.Services.AddSingleton<IEmailSubscribersRepository, MongoEmailSubscribersRepository>(services => {
    var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    var database = new MongoClient(mongoDbSettings!.ConnectionString).GetDatabase(mongoDbSettings.DatabaseName);
    return new MongoEmailSubscribersRepository(mongoDatabase: database);
});

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
