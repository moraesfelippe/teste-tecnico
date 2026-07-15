# Controle de Gastos Residenciais

Sistema de controle de gastos residenciais com cadastro de pessoas, cadastro
de transações (receitas/despesas) e consulta de totais por pessoa e geral.

- **Back-end:** .NET 8 / ASP.NET Core Web API (C#)
- **Front-end:** React + TypeScript (Vite)
- **Persistência:** SQLite (arquivo `gastos.db`, criado automaticamente na
  primeira execução do back-end — os dados permanecem salvos após fechar a
  aplicação)

## Estrutura do repositório

```
backend/GastosResidenciais.Api/
├── Controllers/     # Endpoints HTTP (finos, delegam para os Services)
├── Services/        # Regras de negócio (interfaces + implementações)
├── Data/            # AppDbContext (Entity Framework Core)
├── Models/          # Entidades (Pessoa, Transacao, TipoTransacao)
├── DTOs/            # Objetos de entrada/saída da API
├── Exceptions/       # Exceções de domínio (regra de negócio / não encontrado)
├── Middleware/       # Tratamento centralizado de erros
└── Program.cs        # Composição da aplicação (DI, CORS, Swagger, etc.)

frontend/
└── src/
    ├── components/    # PessoasTab, TransacoesTab, TotaisTab
    ├── services/      # Cliente HTTP (axios) para a API
    ├── types/         # Tipos TypeScript espelhando os DTOs da API
    └── App.tsx         # Navegação por abas e estado compartilhado
```

## Regras de negócio implementadas

- **Pessoa:** identificador único autogerado, nome e idade. Suporta criação,
  listagem e remoção.
- **Exclusão em cascata:** ao remover uma pessoa, todas as suas transações
  são apagadas automaticamente (`PessoaService.DeletarAsync`, reforçado pela
  configuração `OnDelete(DeleteBehavior.Cascade)` no `AppDbContext`).
- **Transação:** identificador único autogerado, descrição, valor, tipo
  (receita/despesa) e o id da pessoa dona da transação — que precisa
  existir previamente no cadastro de pessoas. Suporta criação e listagem
  (sem edição/remoção, conforme especificado).
- **Menores de idade:** pessoas com menos de 18 anos só podem cadastrar
  **despesas**. Uma tentativa de cadastrar receita para um menor é
  rejeitada com HTTP 400 (`TransacaoService.CriarAsync`). O front-end já
  desabilita a opção "Receita" no formulário quando a pessoa selecionada é
  menor de idade, mas a validação que realmente importa acontece no back-end.
- **Totais:** para cada pessoa, soma de receitas, soma de despesas e saldo
  (receitas − despesas); ao final, o total geral somando todas as pessoas
  (`TotaisService.ConsultarAsync`).

## Como executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) (usado para rodar o front-end)

### 1. Back-end (API)

```bash
cd backend/GastosResidenciais.Api
dotnet restore
dotnet run
```

A API sobe em `http://localhost:5199` (porta fixada em
`Properties/launchSettings.json`). O arquivo `gastos.db` é criado
automaticamente na pasta do projeto na primeira execução.

Com a API rodando, a documentação interativa (Swagger) fica disponível em:
`http://localhost:5199/swagger`

### 2. Front-end

Em outro terminal:

```bash
cd frontend
npm install
npm run dev
```

Acesse `http://localhost:5173` no navegador. O front-end já está
configurado para conversar com a API em `http://localhost:5199`
(`frontend/src/services/api.ts`).

> Se preferir rodar a API em outra porta, ajuste `BASE_URL` em
> `frontend/src/services/api.ts` e a origem liberada em `Program.cs` (CORS).

## Principais endpoints da API

| Método | Rota               | Descrição                                             |
|--------|--------------------|--------------------------------------------------------|
| GET    | `/api/pessoas`     | Lista todas as pessoas                                  |
| POST   | `/api/pessoas`     | Cadastra uma pessoa (`{ nome, idade }`)                 |
| DELETE | `/api/pessoas/{id}`| Remove uma pessoa e suas transações (cascata)           |
| GET    | `/api/transacoes`  | Lista todas as transações                               |
| POST   | `/api/transacoes`  | Cadastra uma transação (`{ descricao, valor, tipo, pessoaId }`) |
| GET    | `/api/totais`      | Totais por pessoa + total geral                         |

`tipo` aceita os valores `"Receita"` ou `"Despesa"`.

## Decisões técnicas

- **SQLite** foi escolhido para a persistência por ser um único arquivo,
  sem exigir instalação de um servidor de banco separado, atendendo ao
  requisito de que os dados sobrevivam ao fechar a aplicação.
- **Camada de serviços** separada dos controllers para isolar as regras de
  negócio (facilita testes e leitura do código).
- **Exceções de domínio + middleware central** (`ExceptionHandlingMiddleware`)
  evitam repetir tratamento de erro em cada endpoint e padronizam o formato
  de erro retornado (`{ "erro": "mensagem" }`).
- **`EnsureCreated()`** é usado para criar o schema do banco a partir do
  modelo do EF Core, dispensando a geração de migrations formais — suficiente
  para o escopo deste desafio.

## Possíveis melhorias futuras

- Testes automatizados (unitários para os Services, principalmente a regra
  de menor de idade; e/ou testes de integração para os endpoints).
- Paginação nas listagens, caso o volume de dados cresça bastante.
- Autenticação, caso o sistema precise ser usado por mais de um núcleo
  familiar de forma isolada.
