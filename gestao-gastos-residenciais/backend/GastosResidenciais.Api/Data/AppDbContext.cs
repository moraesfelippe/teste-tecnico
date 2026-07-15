using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Data;

/// <summary>
/// Contexto do Entity Framework Core responsável pelo acesso ao banco
/// SQLite e pelo mapeamento das entidades Pessoa e Transacao.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Regra de negócio: ao deletar uma pessoa, todas as suas transações
        // devem ser removidas junto (delete em cascata). Isso é reforçado
        // em PessoaService, que carrega as transações antes de remover a
        // pessoa, garantindo o comportamento independentemente da
        // configuração de chaves estrangeiras do SQLite em tempo de execução.
        modelBuilder.Entity<Pessoa>()
            .HasMany(p => p.Transacoes)
            .WithOne(t => t.Pessoa)
            .HasForeignKey(t => t.PessoaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Guarda o enum como texto ("Receita"/"Despesa") no banco, o que
        // torna os dados legíveis ao inspecionar o arquivo gastos.db diretamente.
        modelBuilder.Entity<Transacao>()
            .Property(t => t.Tipo)
            .HasConversion<string>();
    }
}
