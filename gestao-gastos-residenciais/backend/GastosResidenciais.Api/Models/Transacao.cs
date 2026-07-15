namespace GastosResidenciais.Api.Models;

/// <summary>
/// Representa uma transação financeira (receita ou despesa) associada a
/// uma pessoa cadastrada no sistema.
/// </summary>
public class Transacao
{
    /// <summary>Identificador único, gerado automaticamente pelo banco de dados.</summary>
    public int Id { get; set; }

    /// <summary>Descrição livre da transação (ex.: "Salário", "Conta de luz").</summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>Valor monetário da transação. Deve ser sempre positivo.</summary>
    public decimal Valor { get; set; }

    /// <summary>Indica se a transação é uma receita ou uma despesa.</summary>
    public TipoTransacao Tipo { get; set; }

    /// <summary>Chave estrangeira para a pessoa dona da transação.</summary>
    public int PessoaId { get; set; }

    /// <summary>Propriedade de navegação usada pelo Entity Framework Core.</summary>
    public Pessoa? Pessoa { get; set; }
}
