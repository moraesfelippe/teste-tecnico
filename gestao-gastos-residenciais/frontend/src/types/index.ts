/**
 * Tipos compartilhados pelo front-end, espelhando os DTOs expostos pela
 * API do back-end (GastosResidenciais.Api).
 */

/** Tipo de uma transação financeira. */
export type TipoTransacao = "Receita" | "Despesa";

/** Pessoa cadastrada no sistema. */
export interface Pessoa {
  id: number;
  nome: string;
  idade: number;
}

/** Dados necessários para cadastrar uma nova pessoa. */
export interface CriarPessoaInput {
  nome: string;
  idade: number;
}

/** Transação financeira (receita ou despesa) cadastrada no sistema. */
export interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: number;
}

/** Dados necessários para cadastrar uma nova transação. */
export interface CriarTransacaoInput {
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: number;
}

/** Totais financeiros (receitas, despesas e saldo) de uma única pessoa. */
export interface TotalPessoa {
  pessoaId: number;
  nome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

/** Consolidado geral de totais: cada pessoa + o total geral do sistema. */
export interface TotalGeral {
  pessoas: TotalPessoa[];
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

/** Formato de erro retornado pela API (ver ExceptionHandlingMiddleware no back-end). */
export interface ApiErro {
  erro: string;
}
