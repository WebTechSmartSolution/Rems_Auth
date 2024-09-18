using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rems_Auth.Data;
using Rems_Auth.Middleware;
using Rems_Auth.Repositories;
using Rems_Auth.Services;
using Rems_Auth.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Email settings from configuration and environment variables
builder.Services.Configure<EmailSettings>(options =>
{
    options.SmtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? builder.Configuration["EmailSettings:SmtpServer"];
    options.SmtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? builder.Configuration["EmailSettings:SmtpPort"]);
    options.SenderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL") ?? builder.Configuration["EmailSettings:SenderEmail"];
    options.SenderPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD") ?? builder.Configuration["EmailSettings:SenderPassword"];
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Configure JWT settings from configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Configure database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// JWT middleware for handling token validation
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
