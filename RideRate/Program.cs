using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RideRate.Data;
using RideRate.Models;
using RideRate.Services.Account;
using RideRate.Services.Auth;
using RideRate.Services.Locations;
using RideRate.Services.Rates;
using RideRate.Services.Registration;
using RideRate.Services.RolesInitializer;
using RideRate.Services.UserResolver;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


//configure database
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApiDbcontext>()
    .AddDefaultTokenProviders();

////Connect services together
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IRolesInitailizer, RolesInitailizer>();
builder.Services.AddScoped<ILoginServices, LoginServices>();
builder.Services.AddScoped<IUserResolverService, UserResolverService>();
builder.Services.AddScoped<ILocationServices, LocationServices>();
builder.Services.AddScoped<IAccountServices, AccountServices>();
IServiceCollection serviceCollection = builder.Services.AddScoped<IRatesServices, RateServices>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standrad Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RideRate", Version="v1" });
});

//Connect database
builder.Services.AddDbContext<ApiDbcontext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

//Add auth seervices
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(builder.Configuration.GetSection("JwtTokens:Key").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseDeveloperExceptionPage();

app.MapControllers();

SeedDatabase();

app.Run();

void SeedDatabase()
{
    using(var scope = app.Services.CreateScope())
    {
        var dbinitializer = scope.ServiceProvider.GetRequiredService<IRolesInitailizer>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        dbinitializer.Initialize(userManager);
    }
}