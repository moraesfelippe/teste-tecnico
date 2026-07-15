import { useState, type FormEvent } from "react";
import type { Pessoa } from "../types";
import { pessoasApi, extrairMensagemDeErro } from "../services/api";

interface Props {
  pessoas: Pessoa[];
  carregando: boolean;
  /** Chamado após criar ou remover uma pessoa, para recarregar os dados no componente pai. */
  onAlterado: () => void;
}

/**
 * Aba de cadastro de pessoas: formulário de criação + listagem com opção
 * de exclusão. Ao excluir uma pessoa, o back-end remove em cascata todas
 * as transações associadas a ela (ver PessoaService.DeletarAsync).
 */
export default function PessoasTab({ pessoas, carregando, onAlterado }: Props) {
  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState("");
  const [enviando, setEnviando] = useState(false);
  const [erroForm, setErroForm] = useState<string | null>(null);

  async function handleSubmit(evento: FormEvent) {
    evento.preventDefault();
    setErroForm(null);

    const idadeNumero = Number(idade);
    if (!nome.trim()) {
      setErroForm("Informe o nome da pessoa.");
      return;
    }
    if (idade === "" || !Number.isInteger(idadeNumero) || idadeNumero < 0) {
      setErroForm("Informe uma idade válida.");
      return;
    }

    setEnviando(true);
    try {
      await pessoasApi.criar({ nome: nome.trim(), idade: idadeNumero });
      setNome("");
      setIdade("");
      onAlterado();
    } catch (e) {
      setErroForm(extrairMensagemDeErro(e));
    } finally {
      setEnviando(false);
    }
  }

  async function handleDeletar(pessoa: Pessoa) {
    const confirmar = window.confirm(
      `Remover ${pessoa.nome}? Todas as transações dessa pessoa também serão apagadas.`
    );
    if (!confirmar) return;

    try {
      await pessoasApi.deletar(pessoa.id);
      onAlterado();
    } catch (e) {
      setErroForm(extrairMensagemDeErro(e));
    }
  }

  return (
    <section className="grid-2">
      <div className="card">
        <h2>Nova pessoa</h2>
        <form onSubmit={handleSubmit} className="form">
          <label>
            Nome
            <input
              type="text"
              value={nome}
              onChange={(e) => setNome(e.target.value)}
              placeholder="Ex.: Maria Silva"
            />
          </label>
          <label>
            Idade
            <input
              type="number"
              min={0}
              value={idade}
              onChange={(e) => setIdade(e.target.value)}
              placeholder="Ex.: 32"
            />
          </label>
          {erroForm && <p className="mensagem-erro">{erroForm}</p>}
          <button type="submit" disabled={enviando}>
            {enviando ? "Salvando..." : "Cadastrar pessoa"}
          </button>
        </form>
      </div>

      <div className="card">
        <h2>Pessoas cadastradas</h2>
        {carregando ? (
          <p className="texto-vazio">Carregando...</p>
        ) : pessoas.length === 0 ? (
          <p className="texto-vazio">Nenhuma pessoa cadastrada ainda.</p>
        ) : (
          <ul className="lista">
            {pessoas.map((pessoa) => (
              <li key={pessoa.id} className="lista-item">
                <div>
                  <strong>{pessoa.nome}</strong>
                  <span className="texto-muted">
                    {" "}
                    · {pessoa.idade} anos
                    {pessoa.idade < 18 ? " · menor de idade" : ""}
                  </span>
                </div>
                <button
                  type="button"
                  className="botao-remover"
                  onClick={() => handleDeletar(pessoa)}
                  aria-label={`Remover ${pessoa.nome}`}
                >
                  Remover
                </button>
              </li>
            ))}
          </ul>
        )}
      </div>
    </section>
  );
}
