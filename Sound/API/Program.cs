using Application.Repositories.IRepositories;
using Application.Repositories;
using Application.Services.IServices;
using Application.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISoundRepo, SoundRepo>();
builder.Services.AddScoped<ISoundService, SoundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();