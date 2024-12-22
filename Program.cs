using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Logging;
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
    options.SmtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? builder.Configuration["EmailSettings:SmtpPort"] ?? "25");
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
builder.Services.AddScoped<IListingRepository, ListingRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IAdminService, AdminService>(); // Example for Admin service
builder.Services.AddScoped<IAdminRepository, AdminRepository>();


// Configure JWT authentication
//var key = Convert.FromBase64String(builder.Configuration["JwtSettings:Secret"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["JwtSettings:Secret"])),
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],  // Make sure this matches the 'aud' claim in the token
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
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // Apply migrations
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


// Enable detailed error pages
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();
//IdentityModelEventSource.ShowPII = true;

//app.UseDeveloperExceptionPage(); // only for testing, remove in production

app.UseCors("AllowAll");
app.UseHttpsRedirection();
// JWT middleware for handling token validation
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
