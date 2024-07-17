using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ApiExamples.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiExamples.Utils;
using ApiExamples.Repositories;
using ApiExamples.Data;

var builder = WebApplication.CreateBuilder(args);

//Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddDbContext<ApiExamplesContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!)
);

builder.Services.AddScoped<IArticlesRepository, ArticlesRepository>();

builder.Services
.AddIdentityCore<User>(opt =>
{
    opt.User.RequireUniqueEmail = true;

})
.AddRoles<Role>()
.AddEntityFrameworkStores<ApiExamplesContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opt =>
  {
      opt.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:TokenKey"]))

      };
  });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {//add JWT authorize widget to Swagger
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer + your token in the box below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true"); //token saved to localStorage of Swagger
    });
}

app.UseMiddleware<CustomMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (!builder.Environment.IsEnvironment("Test"))
{
    var scope = app.Services.CreateScope();
    using (var context = scope.ServiceProvider.GetRequiredService<ApiExamplesContext>())
    {
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            await context.Database.MigrateAsync(); //apply migrations
            await DbInitializer.Initialize(context, userManager);
        }
    }
}

app.Run();

public partial class Program { } //needed for WebApplicationFactory<Program>