using DemoHashPassword.DAL.Interfaces;
using DemoHashPassword.DAL.Repositories;
using DemoHashPasword.BLL.Intefaces;
using DemoHashPasword.BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Ajout des différents services
builder.Services.AddControllers();

#region Connection SQL
builder.Services.AddScoped<SqlConnection>(sp =>
{
    return new SqlConnection(builder.Configuration.GetConnectionString("default"));
});
#endregion

#region Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
#endregion
#region Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

#region Service de Hashage
builder.Services.AddScoped<IHashService, BCryptService>();
//builder.Services.AddScoped<IHashService, Argon2Service>();
#endregion

#region Service de gestion du JWT
builder.Services.AddScoped<JWTService>();
#endregion
#endregion Services
#endregion


#region Middlewar pour JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
            ValidAudience = builder.Configuration["JwtOptions:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]))
        };
    }
);
#endregion

#region Middleware pour les régles d'autorisation
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("adminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("authPolicy", policy => policy.RequireAuthenticatedUser());
});
#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Habituellement vide
// Cette configuration permet de jouer avec des JWT
builder.Services.AddSwaggerGen(
    c => {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Demo du Hash de mot de passe",
            Description = "Demonstration pour la création une Web API de gestion simple d'utilisateur"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header, //Dans le Header Http

            Description = "JWT Bearer : \r\n Enter  Token"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[] {}

            }
        });
    });




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cors => cors
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
            );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
