using BookReviewer.InentityProvider;
using BookReviewer.InentityProvider.IdentityModels;
using BookReviewer.InentityProvider.Services;
using BookReviewer.Shared.Auth;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMongoIdentity(builder.Configuration);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.AddSingleton<JwtProvider>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{ 
    app.UseHttpsRedirection();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
