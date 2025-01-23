using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Ponta.Contexto.Tarefa.Contexto;
using Ponta.Contexto.Tarefa.Enums;
using Ponta.Contexto.Tarefa.Interfaces;
using Ponta.Contexto.Tarefa.Repositorio;
using Ponta.Servico.Tarefas;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

// Adicionar HttpContextAccessor para acessar o HttpContext
builder.Services.AddHttpContextAccessor();

// Adicionar outros serviços
builder.Services.AddScoped<IServicoTarefas, ServicoTarefas>();
builder.Services.AddScoped<IRepositorioTarefa, RepositorioTarefa>();
builder.Services.AddScoped<IContextoTarefa, ContextoTarefa>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header usando o esquema Bearer. \r\n\r\n " +
                                "Digite 'Bearer' [espaço] e, em seguida, seu token na entrada de texto abaixo.\r\n\r\n" +
                                "Exemplo: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] { }
                }
        });
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/",
    [Authorize] async (IServicoTarefas servico, HttpContext httpContext) =>
    {
        var token = string.Empty;

        // Verificar se o header Authorization está presente
        if (httpContext.Request.Headers.ContainsKey("Authorization"))
        {
            // Obter o valor do header Authorization
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();

            // O valor do header Authorization geralmente é "Bearer {token}"
            if (authHeader.StartsWith("Bearer "))
            {
                token = authHeader.Substring("Bearer ".Length).Trim();
            }
        }

        Guid guidUsuarioLogado = ObterUsuario(token);

        if (guidUsuarioLogado == Guid.Empty)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(await servico.BuscarTarefas());
    })
    .WithName("BuscarTarefas");

app.MapGet("/{status}",
    [Authorize] async (IServicoTarefas servico, StatusTarefa status, HttpContext httpContext) =>
    {
        var token = string.Empty;

        // Verificar se o header Authorization está presente
        if (httpContext.Request.Headers.ContainsKey("Authorization"))
        {
            // Obter o valor do header Authorization
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();

            // O valor do header Authorization geralmente é "Bearer {token}"
            if (authHeader.StartsWith("Bearer "))
            {
                token = authHeader.Substring("Bearer ".Length).Trim();
            }
        }

        Guid guidUsuarioLogado = ObterUsuario(token);

        if (guidUsuarioLogado == Guid.Empty)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(await servico.BuscarTarefas(status));
    })
    .WithName("BuscarTarefasPorStatus");

Guid ObterUsuario(string token)
{
    var tokenHandler = new JwtSecurityTokenHandler();

    var validationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("chave-super-segura-e-bem-maior-para-teste-1234567890"))
    };

    try
    {
        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        var jwtToken = (JwtSecurityToken)validatedToken;

        var userId = jwtToken.Claims.First(claim => claim.Type == "nameid").Value;

        return Guid.Parse(userId);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao decodificar o token: {ex.Message}");
        return Guid.Empty;
    }
}

await app.RunAsync();
