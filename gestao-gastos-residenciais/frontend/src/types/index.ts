export type TipoTransacao = "Receita" | "Despesa";

export interface Pessoa {
  id: number;
  nome: string;
  idade: number;
}

export interface CriarPessoaInput {
  nome: string;
  idade: number;
}

export interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: number;
}

export interface CriarTransacaoInput {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: number;
}

export interface TotalPessoa {
  pessoaId: number;
  nome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface TotalGeral {
  pessoas: TotalPessoa[];
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface ApiErro {
  erro: string;
}