//This file is the entry point for this application.

using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.MIddleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// For DI

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();









var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// app.UseHttpsRedirection();


app.UseMiddleware<ExceptionMiddleware>();

//Configure the http request pipeline for routing request.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));


//Thes two authentication and authorization middleware are always to be added between cors and mapcontroller middleware

//This checks if you have valid token
app.UseAuthentication();
//Ok so you have valid token but what you are allowed to do. ie claims
app.UseAuthorization();

app.MapControllers();

app.Run();
