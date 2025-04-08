using ContentService.Api.Endpoints;
using ContentService.Application;
using ContentService.Infrastructure;
using Dapr;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your token as 'Bearer {your_token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    opt =>
    {
        opt.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        opt.RequireHttpsMetadata = false;
        opt.MetadataAddress = builder.Configuration["Keycloak:MetadataAddress"]!;
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Keycloak:Audience"],
            ValidIssuer = builder.Configuration["Keycloak:ValidIssuer"],
            ValidateIssuer = true,
            ValidateAudience = true
        };
        opt.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"JWT Auth failed: {ctx.Exception}");
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine("JWT Auth success!");
                return Task.CompletedTask;
            }
        };
    });



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAspire", builder =>
    {
        builder.WithOrigins("https://localhost:7293", "http://localhost:5012")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAspire");

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
    
}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection(); 


//app.UseCloudEvents();  <= Use?
app.MapSubscribeHandler();

app.MapGet("/hello", () => "Hello World!").AllowAnonymous();

/* Flow:
ContentService => contentSubmitted => ContentSafetyService moderates => contentApproved => ContentService saves => contentToPublish => ContentService saves => contentPublished

With workflow:
// High-Level Overview //

1. User Creates Forum → ContentService receives request.

2. Triggers Workflow → WorkflowService orchestrates the process.

3. Runs Content Safety Checks → ContentSafetyService validates content.

4. Updates State Store → If approved, ContentService persists forum.

5. Workflow Completes → Final status is updated, and forum is published.

*/
app.MapForumEndpoints();
app.MapPostEndpoints();
app.MapCommentEndpoints();

app.Run();

