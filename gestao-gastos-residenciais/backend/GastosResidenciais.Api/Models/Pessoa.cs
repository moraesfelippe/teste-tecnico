namespace GastosResidenciais.Api.Models;

/// <summary>
/// Representa uma pessoa do núcleo residencial. Cada pessoa pode ter
/// diversas transações financeiras (receitas e/ou despesas) associadas.
/// </summary>
public class Pessoa
{
    /// <summary>Identificador único, gerado automaticamente pelo banco de dados.</summary>
    public int Id { get; set; }

    /// <summary>Nome completo da pessoa.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Idade da pessoa. Usada para aplicar a regra de negócio que impede
    /// menores de 18 anos de cadastrarem receitas (ver TransacaoService).
    /// </summary>
    public int Idade { get; set; }

    /// <summary>
    /// Transações associadas a esta pessoa. O relacionamento é configurado
    /// com delete em cascata no AppDbContext: ao remover a pessoa, todas as
    /// suas transações são removidas automaticamente.
    /// </summary>
    public List<Transacao> Transacoes { get; set; } = new();
}
