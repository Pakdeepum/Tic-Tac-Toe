using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Tictactoe.Service;
using Tictactoe.Service.Authentication;
using Tictactoe.Service.Extensions;
using Tictactoe.Service.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Tictactoe.Service.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Tictactoe.Service.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

#region Add CORS Policies to Service
// CORS
string allowOriginsText = builder.Configuration["CORS:AllowOrigins"];
string[] allowOrigins = allowOriginsText.Split(",", StringSplitOptions.RemoveEmptyEntries);
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "LaroApiCorsPolicy",
        builder =>
            builder
                .AllowAnyHeader()
                .WithOrigins(allowOrigins)
                .WithMethods(
                    HttpMethod.Get.Method,
                    HttpMethod.Post.Method,
                    HttpMethod.Put.Method,
                    HttpMethod.Delete.Method,
                    HttpMethod.Options.Method
                )
                .AllowCredentials()
    );

    options.AddPolicy(
        "LaroHubCorsPolicy",
        builder => builder.AllowAnyHeader().WithOrigins(allowOrigins).AllowAnyMethod()
    );
});
#endregion

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen(c =>
{
    var bearerSecuritySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", bearerSecuritySchema);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { bearerSecuritySchema, new[] { "Bearer" } },
    });

    c.OperationFilter<SwaggerEnpointDescriptionFilter>();
    c.SchemaFilter<SwaggerModelFormatFilter>();
    c.OperationFilter<SwaggerExcludeFilter>();
});

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

builder.Services.AddAuthorization();

builder.Services.InjectServicesCollection();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("LaroApiCorsPolicy");

app.MapControllers();

app.Run();
