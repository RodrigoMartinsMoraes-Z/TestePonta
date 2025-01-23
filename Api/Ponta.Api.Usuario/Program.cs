using Ponta.Contexto.Usuario.Contexto;
using Ponta.Contexto.Usuario.Interfaces;
using Ponta.Contexto.Usuario.Repositorio;
using Ponta.Servico.Usuario;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IServicoUsuario, ServicoUsuario>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IContextoUsuario, ContextoUsuario>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
.WithName("DeletarUsuario");

await app.RunAsync();

