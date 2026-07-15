import axios from "axios";
import type {
  Pessoa,
  CriarPessoaInput,
  Transacao,
  CriarTransacaoInput,
  TotalGeral,
  ApiErro,
} from "../types";

// ajuste aqui se a API rodar em outra porta
const BASE_URL = "http://localhost:5199/api";

const api = axios.create({ baseURL: BASE_URL });

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

export const pessoasApi = {
  listar: () => api.get<Pessoa[]>("/pessoas").then((r) => r.data),
  criar: (dto: CriarPessoaInput) => api.post<Pessoa>("/pessoas", dto).then((r) => r.data),
  deletar: (id: number) => api.delete(`/pessoas/${id}`),
};

export const transacoesApi = {
  listar: () => api.get<Transacao[]>("/transacoes").then((r) => r.data),
  criar: (dto: CriarTransacaoInput) => api.post<Transacao>("/transacoes", dto).then((r) => r.data),
};

export const totaisApi = {
  consultar: () => api.get<TotalGeral>("/totais").then((r) => r.data),
};