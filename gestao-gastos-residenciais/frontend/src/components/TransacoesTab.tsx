import { useEffect, useMemo, useState, type FormEvent } from "react";
import type { Pessoa, Transacao, TipoTransacao } from "../types";
import { transacoesApi, extrairMensagemDeErro } from "../services/api";

interface Props {
  pessoas: Pessoa[];
  transacoes: Transacao[];
  carregando: boolean;
  onAlterado: () => void;
}

const formatoMoeda = new Intl.NumberFormat("pt-BR", { style: "currency", currency: "BRL" });
const IDADE_MINIMA_PARA_RECEITA = 18;

// desabilita "Receita" no formulário quando a pessoa é menor — a validação que vale mesmo é no back-end
export default function TransacoesTab({ pessoas, transacoes, carregando, onAlterado }: Props) {
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState("");
  const [tipo, setTipo] = useState<TipoTransacao>("Despesa");
  const [pessoaId, setPessoaId] = useState<string>("");
  const [enviando, setEnviando] = useState(false);
  const [erroForm, setErroForm] = useState<string | null>(null);

  const pessoaSelecionada = useMemo(
    () => pessoas.find((p) => p.id === Number(pessoaId)) ?? null,
    [pessoas, pessoaId]
  );
  const pessoaEhMenorDeIdade = (pessoaSelecionada?.idade ?? 0) < IDADE_MINIMA_PARA_RECEITA;

  // se trocar pra uma pessoa menor de idade, força voltar pra "Despesa"
  useEffect(() => {
    if (pessoaSelecionada && pessoaEhMenorDeIdade && tipo === "Receita") {
      setTipo("Despesa");
    }
  }, [pessoaSelecionada, pessoaEhMenorDeIdade, tipo]);

  const mapaPessoasPorId = useMemo(() => {
    const mapa = new Map<number, Pessoa>();
    pessoas.forEach((p) => mapa.set(p.id, p));
    return mapa;
  }, [pessoas]);

  async function handleSubmit(evento: FormEvent) {
    evento.preventDefault();
    setErroForm(null);

    const valorNumero = Number(valor.replace(",", "."));

    if (!pessoaId) {
      setErroForm("Selecione a pessoa dona da transação.");
      return;
    }
    if (!descricao.trim()) {
      setErroForm("Informe a descrição da transação.");
      return;
    }
    if (!(valorNumero > 0)) {
      setErroForm("Informe um valor maior que zero.");
      return;
    }

    setEnviando(true);
    try {
      await transacoesApi.criar({
        descricao: descricao.trim(),
        valor: valorNumero,
        tipo,
        pessoaId: Number(pessoaId),
      });
      setDescricao("");
      setValor("");
      onAlterado();
    } catch (e) {
      setErroForm(extrairMensagemDeErro(e));
    } finally {
      setEnviando(false);
    }
  }

  return (
    <section className="grid-2">
      <div className="card">
        <h2>Nova transação</h2>
        <form onSubmit={handleSubmit} className="form">
          <label>
            Pessoa
            <select value={pessoaId} onChange={(e) => setPessoaId(e.target.value)}>
              <option value="">Selecione...</option>
              {pessoas.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.nome} ({p.idade} anos)
                </option>
              ))}
            </select>
          </label>

          <label>
            Descrição
            <input
              type="text"
              value={descricao}
              onChange={(e) => setDescricao(e.target.value)}
              placeholder="Ex.: Supermercado, Salário..."
            />
          </label>

          <label>
            Valor (R$)
            <input
              type="number"
              min={0}
              step="0.01"
              value={valor}
              onChange={(e) => setValor(e.target.value)}
              placeholder="Ex.: 150.00"
            />
          </label>

          <fieldset className="fieldset-tipo">
            <legend>Tipo</legend>
            <label className="radio">
              <input
                type="radio"
                name="tipo"
                value="Despesa"
                checked={tipo === "Despesa"}
                onChange={() => setTipo("Despesa")}
              />
              Despesa
            </label>
            <label className="radio">
              <input
                type="radio"
                name="tipo"
                value="Receita"
                checked={tipo === "Receita"}
                disabled={pessoaEhMenorDeIdade}
                onChange={() => setTipo("Receita")}
              />
              Receita
            </label>
          </fieldset>
          {pessoaEhMenorDeIdade && (
            <p className="texto-aviso">
              {pessoaSelecionada?.nome} é menor de idade: apenas despesas podem ser cadastradas.
            </p>
          )}

          {erroForm && <p className="mensagem-erro">{erroForm}</p>}

          <button type="submit" disabled={enviando || pessoas.length === 0}>
            {enviando ? "Salvando..." : "Cadastrar transação"}
          </button>
          {pessoas.length === 0 && (
            <p className="texto-muted">Cadastre uma pessoa antes de lançar transações.</p>
          )}
        </form>
      </div>

      <div className="card">
        <h2>Transações cadastradas</h2>
        {carregando ? (
          <p className="texto-vazio">Carregando...</p>
        ) : transacoes.length === 0 ? (
          <p className="texto-vazio">Nenhuma transação cadastrada ainda.</p>
        ) : (
          <ul className="lista">
            {transacoes.map((t) => (
              <li key={t.id} className="lista-item">
                <div>
                  <strong>{t.descricao}</strong>
                  <span className="texto-muted">
                    {" "}
                    · {mapaPessoasPorId.get(t.pessoaId)?.nome ?? `pessoa #${t.pessoaId}`}
                  </span>
                </div>
                <span className={`valor ${t.tipo === "Receita" ? "valor-receita" : "valor-despesa"}`}>
                  {t.tipo === "Receita" ? "+" : "−"} {formatoMoeda.format(t.valor)}
                </span>
              </li>
            ))}
          </ul>
        )}
      </div>
    </section>
  );
}
