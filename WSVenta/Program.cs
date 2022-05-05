
using WSVenta.Services;
using WSVenta.Models.Common;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var  Micors = "MiCors";
var builder = WebApplication.CreateBuilder(args);



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Micors,
                      policy =>
                      {
                          policy.WithHeaders("*");
                          policy.WithOrigins("*");
                          policy.WithMethods("*");

                      });
});



// Inyeccion de dependencias important!!!
builder.Services.AddScoped<IUserService, UserService>();
///
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

//Json WebToken (Jwt)
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);
builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(d =>
  {
      d.RequireHttpsMetadata = false;
      d.SaveToken = true;
      d.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
      };
  });


// Add services to the container.
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(Micors);
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();

