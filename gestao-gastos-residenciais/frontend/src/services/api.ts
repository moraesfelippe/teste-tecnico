import axios from "axios";
import type {
  Pessoa,
  CriarPessoaInput,
  Transacao,
  CriarTransacaoInput,
  TotalGeral,
  ApiErro,
} from "../types";

/**
 * Endereço base da API. Deve corresponder à porta configurada em
 * backend/GastosResidenciais.Api/Properties/launchSettings.json.
 * Caso o back-end rode em outra porta, ajuste aqui (ou crie um arquivo
 * .env com VITE_API_URL e troque a linha abaixo por import.meta.env.VITE_API_URL).
 */
const BASE_URL = "http://localhost:5199/api";

const api = axios.create({ baseURL: BASE_URL });

/**
 * Extrai uma mensagem de erro amigável de uma resposta da API, usando o
 * corpo `{ erro: string }` produzido pelo ExceptionHandlingMiddleware do
 * back-end quando uma regra de negócio é violada.
 */
export function extrairMensagemDeErro(erro: unknown): string {
  if (axios.isAxiosError(erro)) {
    const dados = erro.response?.data as ApiErro | string | undefined;

    if (dados && typeof dados === "object" && "erro" in dados) {
      return dados.erro;
    }
    if (typeof dados === "string" && dados.length > 0) {
      return dados;
    }
    if (erro.code === "ERR_NETWORK") {
      return "Não foi possível conectar à API. Verifique se o back-end está rodando em http://localhost:5199.";
    }
  }
  return "Ocorreu um erro inesperado. Tente novamente.";
}

/** Chamadas relacionadas ao cadastro de pessoas. */
export const pessoasApi = {
  listar: () => api.get<Pessoa[]>("/pessoas").then((r) => r.data),
  criar: (dto: CriarPessoaInput) => api.post<Pessoa>("/pessoas", dto).then((r) => r.data),
  deletar: (id: number) => api.delete(`/pessoas/${id}`),
};

/** Chamadas relacionadas ao cadastro de transações. */
export const transacoesApi = {
  listar: () => api.get<Transacao[]>("/transacoes").then((r) => r.data),
  criar: (dto: CriarTransacaoInput) => api.post<Transacao>("/transacoes", dto).then((r) => r.data),
};

/** Chamada relacionada à consulta de totais. */
export const totaisApi = {
  consultar: () => api.get<TotalGeral>("/totais").then((r) => r.data),
};
