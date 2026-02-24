using ProductsService.API.Middleware;
using ProductsService.Business;
using ProductsService.DataAccess;
using FluentValidation.AspNetCore;
using ProductsService.API.Endpoints;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessDependencies(builder.Configuration);
builder.Services.AddBusinessDependencies();


builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
//Add model binder to read values from JSON to enum
builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add cors services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors();

//Swagger
app.UseSwagger(); //Adds endpoints that serve swagger.json
app.UseSwaggerUI();  //Adds swagger ui => interactive page to explore and test API endpoints 
//Auth
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapProductEndpoints();


app.Run();
