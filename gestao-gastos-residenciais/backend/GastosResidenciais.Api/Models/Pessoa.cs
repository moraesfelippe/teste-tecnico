namespace GastosResidenciais.Api.Models;

public class Pessoa
{
        public int Id { get; set; }

    
    public string Nome { get; set; } = string.Empty;

    // usada na regra de menor de idade
    public int Idade { get; set; }

    // delete em cascata no AppDbContext
    public List<Transacao> Transacoes { get; set; } = new();
}