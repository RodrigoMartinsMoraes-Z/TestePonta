using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Ponta.Contexto.Usuario.Contexto;
using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Contexto.Usuario.Repositorio;
using Ponta.Servico.Login;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar JWT
var key = Encoding.ASCII.GetBytes("chave-super-segura-e-bem-maior-para-teste-1234567890");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IServicoLogin, ServicoLogin>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IContextoUsuario, ContextoUsuario>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meu API V1");
        c.RoutePrefix = string.Empty;
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/{login}/{senha}", async (IServicoLogin servico, string login, string senha) =>
{
    return await servico.LoginAsync(login, senha);
})
.WithName("Login");

await app.RunAsync();


