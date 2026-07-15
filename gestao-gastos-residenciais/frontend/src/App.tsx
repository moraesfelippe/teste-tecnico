import { useCallback, useEffect, useState } from "react";
import "./index.css";
import { pessoasApi, transacoesApi, totaisApi, extrairMensagemDeErro } from "./services/api";
import type { Pessoa, Transacao, TotalGeral } from "./types";
import PessoasTab from "./components/PessoasTab";
import TransacoesTab from "./components/TransacoesTab";
import TotaisTab from "./components/TotaisTab";

type Aba = "pessoas" | "transacoes" | "totais";

// guarda o estado das 3 abas e recarrega tudo quando algo muda em qualquer uma delas
function App() {
  const [abaAtiva, setAbaAtiva] = useState<Aba>("pessoas");

  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [totais, setTotais] = useState<TotalGeral | null>(null);

  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState<string | null>(null);

  const carregarTudo = useCallback(async () => {
    setErro(null);
    try {
      const [pessoasResp, transacoesResp, totaisResp] = await Promise.all([
        pessoasApi.listar(),
        transacoesApi.listar(),
        totaisApi.consultar(),
      ]);
      setPessoas(pessoasResp);
      setTransacoes(transacoesResp);
      setTotais(totaisResp);
    } catch (e) {
      setErro(extrairMensagemDeErro(e));
    } finally {
      setCarregando(false);
    }
  }, []);

  useEffect(() => {
    carregarTudo();
  }, [carregarTudo]);

  return (
    <div className="app-shell">
      <header className="app-header">
        <h1>Casa em Dia</h1>
  <span className="app-autor">Sistema de finanças residenciais</span>
  <p>Cadastre as pessoas da casa, registre receitas e despesas e acompanhe o saldo de cada uma.</p>
</header>

      <nav className="tab-nav" aria-label="Seções do sistema">
        <button
          type="button"
          className={abaAtiva === "pessoas" ? "tab tab-ativa" : "tab"}
          onClick={() => setAbaAtiva("pessoas")}
        >
          Pessoas
        </button>
        <button
          type="button"
          className={abaAtiva === "transacoes" ? "tab tab-ativa" : "tab"}
          onClick={() => setAbaAtiva("transacoes")}
        >
          Transações
        </button>
        <button
          type="button"
          className={abaAtiva === "totais" ? "tab tab-ativa" : "tab"}
          onClick={() => setAbaAtiva("totais")}
        >
          Totais
        </button>
      </nav>

      {erro && <div className="banner banner-erro">{erro}</div>}

      <main className="app-content">
        {abaAtiva === "pessoas" && (
          <PessoasTab pessoas={pessoas} carregando={carregando} onAlterado={carregarTudo} />
        )}
        {abaAtiva === "transacoes" && (
          <TransacoesTab
            pessoas={pessoas}
            transacoes={transacoes}
            carregando={carregando}
            onAlterado={carregarTudo}
          />
        )}
        {abaAtiva === "totais" && <TotaisTab totais={totais} carregando={carregando} />}
      </main>
      <footer className="app-footer">
  <p className="app-assinatura">Desenvolvido por Felipe Moraes</p>
  <p className="app-meta">
  © 2026 Casa em Dia · <a href="mailto:felipemartins213@gmail.com">felipemartins213@gmail.com</a> · <a href="tel:+5522999999999">(21) 995002250</a>
</p>
</footer>
    </div>
  );
}

export default App;
