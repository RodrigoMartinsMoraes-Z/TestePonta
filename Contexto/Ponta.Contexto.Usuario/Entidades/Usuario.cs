using System.Security.Cryptography;
using System.Text;

namespace Ponta.Contexto.Usuario.Entidades;

public class Usuario
{
    private string? senha;

    public int Id { get; set; }

    public Guid Guid { get; set; }

    public string? Nome { get; set; }

    public string? Login { get; set; }

    public string Senha
    {
        get => senha;
        set => senha = GerarSenhaCriptografada(value);
    }

    private static string GerarSenhaCriptografada(string senha)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(senha);

        byte[] hash = SHA256.HashData(bytes);

        StringBuilder builder = new();
        foreach (byte b in hash)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }
}
