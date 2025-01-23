using Ponta.Contexto.Tarefa.Enums;

namespace Ponta.Contexto.Tarefa.Entidades;
public class Tarefa
{
    DateTime dataCriacao;
    private DateTime dataInicio;
    private DateTime dataFim;

    public int Id { get; set; }

    public Guid Guid { get; set; }

    public string Titulo { get; set; }

    public string Descricao { get; set; }

    public DateTime DataCriacao
    {
        get => dataCriacao;
        set => dataCriacao = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
    }

    public DateTime DataInicio
    {
        get => dataInicio;
        set => dataInicio = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    public DateTime DataFim
    {
        get => dataFim;
        set => dataFim = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    public StatusTarefa Status { get; set; }

    public Prioridade Prioridade { get; set; }

    public Guid GuidUsuario { get; set; }

}
