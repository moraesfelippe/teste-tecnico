namespace GastosResidenciais.Api.Models;

/// <summary>
/// Tipo de uma transação financeira.
/// Serializado como texto ("Receita"/"Despesa") na API para facilitar a
/// leitura, tanto pelo front-end quanto por quem consumir a API via Swagger.
/// </summary>
public enum TipoTransacao
{
    Receita = 0,
    Despesa = 1
}
