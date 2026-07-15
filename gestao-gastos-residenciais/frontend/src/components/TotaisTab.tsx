import type { TotalGeral } from "../types";

interface Props {
  totais: TotalGeral | null;
  carregando: boolean;
}

const formatoMoeda = new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" });

export default function TotaisTab({ totais, carregando }: Props) {
  if (carregando) {
    return <p className="texto-vazio">Carregando...</p>;
  }

  if (!totais || totais.pessoas.length === 0) {
    return (
      <p className="texto-vazio">
        Cadastre pessoas e transações para ver os totais aqui.
      </p>
    );
  }

  return (
    <section className="card">
      <h2>Totais por pessoa</h2>
      <div className="tabela-wrapper">
        <table className="tabela-totais">
          <thead>
            <tr>
              <th>Pessoa</th>
              <th className="col-valor">Receitas</th>
              <th className="col-valor">Despesas</th>
              <th className="col-valor">Saldo</th>
            </tr>
          </thead>
          <tbody>
            {totais.pessoas.map((p) => (
              <tr key={p.pessoaId}>
                <td>{p.nome}</td>
                <td className="col-valor valor-receita">{formatoMoeda.format(p.totalReceitas)}</td>
                <td className="col-valor valor-despesa">{formatoMoeda.format(p.totalDespesas)}</td>
                <td className={`col-valor ${p.saldo >= 0 ? "valor-receita" : "valor-despesa"}`}>
                  {formatoMoeda.format(p.saldo)}
                </td>
              </tr>
            ))}
          </tbody>
          <tfoot>
            <tr>
              <td>Total geral</td>
              <td className="col-valor valor-receita">{formatoMoeda.format(totais.totalReceitas)}</td>
              <td className="col-valor valor-despesa">{formatoMoeda.format(totais.totalDespesas)}</td>
              <td className={`col-valor ${totais.saldo >= 0 ? "valor-receita" : "valor-despesa"}`}>
                {formatoMoeda.format(totais.saldo)}
              </td>
            </tr>
          </tfoot>
        </table>
      </div>
    </section>
  );
}
