using Ponta.Contexto.Usuario.Contexto;
using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Contexto.Usuario.Repositorio;
using Ponta.Servico.Login;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IServicoLogin, ServicoLogin>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IContextoUsuario, ContextoUsuario>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost("/{login}/{senha}", async (IServicoLogin servico, string login, string senha) =>
{
    return await servico.LoginAsync(login, senha);
})
.WithName("Login");

await app.RunAsync();


