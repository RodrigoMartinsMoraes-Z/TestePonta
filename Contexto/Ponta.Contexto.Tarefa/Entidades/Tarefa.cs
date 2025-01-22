using Ponta.Contexto.Tarefa.Enums;

namespace Ponta.Contexto.Tarefa.Entidades;
public class Tarefa
{
    DateTime dataCriacao;

    public int Id { get; set; }

    public Guid Guid { get; set; }

    public string Titulo { get; set; }

    public string Descricao { get; set; }

    public DateTime DataCriacao { get => dataCriacao; set => dataCriacao = DateTime.Today; } //Sempre adiciona a tarefa com a data atual

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public StatusTarefa Status { get; set; }

    public Prioridade Prioridade { get; set; }

    public Guid GuidUsuario { get; set; }

}
