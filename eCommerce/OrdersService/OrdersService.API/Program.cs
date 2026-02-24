using FluentValidation.AspNetCore;
using OrdersService.API.Middleware;
using OrdersService.Business;
using OrdersService.Business.HttpClients;
using OrdersService.Business.Policies;
using OrdersService.DataAccess;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessDependencies(builder.Configuration);
builder.Services.AddBusinessDependencies(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add cors services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddTransient<IUsersMicroservicePolicies, UsersMicroservicePolicies>();
builder.Services.AddTransient<IProductsMicroservicePolicies, ProductsMicroservicePolicies>();
builder.Services.AddTransient<IPollyPolicies, PollyPolicies>();

builder.Services.AddHttpClient<UsersMicroserviceClient>(client =>
{
    client.BaseAddress = new Uri($"http://{builder.Configuration["UsersMicroserviceName"]}:{builder.Configuration["UsersMicroservicePort"]}");
})
.AddPolicyHandler(
   builder.Services.BuildServiceProvider()
   .GetRequiredService<IUsersMicroservicePolicies>()
   .GetCombinedPolicy());
/*
 // Will combine following policies to be executed in order specified in GetCombinedPolicy()
.AddPolicyHandler(
builder.Services.BuildServiceProvider()
.GetRequiredService<IUsersMicroservicePolicies>()
.GetRetryPolicy()
)
.AddPolicyHandler(
    builder.Services.BuildServiceProvider()
    .GetRequiredService<IUsersMicroservicePolicies>()
    .GetCircuitBreakerPolicy())
.AddPolicyHandler(
builder.Services.BuildServiceProvider()
.GetRequiredService<IUsersMicroservicePolicies>()
.GetTimeoutPolicy());
*/



builder.Services.AddHttpClient<ProductsMicroserviceClient>(client =>
{
    client.BaseAddress = new Uri($"http://{builder.Configuration["ProductsMicroserviceName"]}:{builder.Configuration["ProductsMicroservicePort"]}");
})
.AddPolicyHandler(
   builder.Services.BuildServiceProvider()
   .GetRequiredService<IProductsMicroservicePolicies>()
   .GetFallbackPolicy()
   )
.AddPolicyHandler(
   builder.Services.BuildServiceProvider()
   .GetRequiredService<IProductsMicroservicePolicies>()
   .GetBulkheadIsolationPolicy());

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

//Endpoints
app.MapControllers();

app.Run();
