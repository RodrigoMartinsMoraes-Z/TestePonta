using Ponta.Contexto.Tarefa.Contexto;
using Ponta.Contexto.Tarefa.Enums;
using Ponta.Contexto.Tarefa.Interfaces;
using Ponta.Contexto.Tarefa.Repositorio;
using Ponta.Servico.Tarefas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServicoTarefas, ServicoTarefas>();
builder.Services.AddScoped<IRepositorioTarefa, RepositorioTarefa>();
builder.Services.AddScoped<IContextoTarefa, ContextoTarefa>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", async (IServicoTarefas servico) =>
{
    return await servico.BuscarTarefas();
})
.WithName("BuscarTarefas");

app.MapGet("/{status}", async (IServicoTarefas servico, StatusTarefa status) =>
{
    return await servico.BuscarTarefas(status);
})
.WithName("BuscarTarefasPorStatus");

await app.RunAsync();


