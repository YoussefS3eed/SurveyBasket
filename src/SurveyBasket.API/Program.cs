using SurveyBasket.API.Middleware;
using SurveyBasket.Application.Extensions;
using SurveyBasket.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<RequestTimeLoggingMiddleware>();


//builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();


var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestTimeLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

//app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();