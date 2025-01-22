using Ponta.Contexto.Usuario.Contexto;
using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Contexto.Usuario.Repositorio;
using Ponta.Servico.Usuario;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IServicoUsuario, ServicoUsuario>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IContextoUsuario, ContextoUsuario>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/{guid}", async (IServicoUsuario servico, Guid guid) =>
{
    return await servico.ObterUsuarioPorGuidAsync(guid);
})
.WithName("BuscarUsuario");

app.MapPost(
string.Empty,
    async (IServicoUsuario servico, Ponta.Contexto.Usuario.Entidades.Usuario usuario) =>
    {
        return await servico.SalvarUsuarioAsync(usuario);
    })
    .WithName("NovoUsuario");

app.MapPut(
string.Empty,
    async (IServicoUsuario servico, Ponta.Contexto.Usuario.Entidades.Usuario usuario) =>
    {
        return await servico.AtualizarUsuarioAsync(usuario);
    })
    .WithName("AtualizarUsuario");

app.MapDelete("/{guid}", async (IServicoUsuario servico, Guid guid) =>
{
    return await servico.ExcluirUsuarioAsync(guid);
})
.WithName("BuscarUsuario");

await app.RunAsync();

