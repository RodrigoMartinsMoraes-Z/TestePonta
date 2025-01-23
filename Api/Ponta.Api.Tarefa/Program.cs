using Ponta.Contexto.Tarefa.Contexto;
using Ponta.Contexto.Tarefa.Entidades;
using Ponta.Contexto.Tarefa.Interfaces;
using Ponta.Contexto.Tarefa.Repositorio;
using Ponta.Servico.Tarefa;

using Tarefa = Ponta.Contexto.Tarefa.Entidades.Tarefa;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IServicoTarefa, ServicoTarefa>();
builder.Services.AddScoped<IRepositorioTarefa, RepositorioTarefa>();
builder.Services.AddScoped<IContextoTarefa, ContextoTarefa>();

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

app.MapPost("/", async (IServicoTarefa servico, Tarefa tarefa) =>
{
    return await servico.NovaTarefa(tarefa, tarefa.GuidUsuario);
})
.WithName("BuscarTarefa");

app.MapPut(
    "/",
    async (IServicoTarefa servico, Tarefa tarefa) =>
    {
        return await servico.AtualizarTarefa(tarefa, tarefa.GuidUsuario);
    })
    .WithName("AtualizarTarefa");

await app.RunAsync();

