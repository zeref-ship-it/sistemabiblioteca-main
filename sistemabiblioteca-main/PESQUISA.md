# Relatório Técnico — Sistema Biblioteca (ASP.NET Core MVC + EF Core + SQLite)

**Autor:** Luiz Carlos Caetano
**Disciplina:** PROGRAMAÇÃO PARA WEB II - PROF RUBEN PRADO
**Contexto:** Desafio técnico — relatório para o CTO sobre a base tecnológica do projeto

---

## Introdução

Este relatório explica, de forma técnica e objetiva, os três pilares que sustentam o **Sistema Biblioteca**: a Injeção de Dependência do ASP.NET Core, o funcionamento do Entity Framework Core como ORM, e os limites do SQLite como banco de dados. O objetivo é justificar as decisões técnicas atuais e indicar o caminho de evolução da plataforma.

---

## 1. ⚙️ O Motor do ASP.NET (Injeção de Dependência)

### O que é Injeção de Dependência (DI) e qual problema ela resolve

**Injeção de Dependência (Dependency Injection)** é um padrão de projeto em que uma classe **não cria** os objetos de que precisa para funcionar — ela apenas **declara** essa necessidade (geralmente no construtor), e um componente externo, chamado **container de DI**, se encarrega de fornecer (“injetar”) essa dependência pronta para uso.

No nosso projeto, isso aparece o tempo todo. Por exemplo, o `AccountController` e o `AdminController` recebem um `AppDbContext` no construtor, em vez de fazer algo como `new AppDbContext()` dentro de cada método:

```csharp
public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }
}
```

O **problema que isso resolve** é o **acoplamento forte**. Se o Controller criasse o `AppDbContext` manualmente, ele precisaria saber detalhes de configuração (string de conexão, provedor de banco, etc.) e ficaria “preso” a essa implementação específica. Isso traz várias dores:

- Dificulta a troca de implementação (ex: trocar SQLite por PostgreSQL no futuro).
- Dificulta testes automatizados, já que não é possível substituir o banco real por um “dublê” (mock/fake) nos testes.
- Espalha lógica de configuração por todo o código, em vez de centralizá-la em um único lugar.

Com DI, toda a configuração fica centralizada no `Program.cs`, em uma única linha:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=biblioteca.db"));
```

A partir daí, qualquer Controller que precisar conversar com o banco simplesmente **pede** um `AppDbContext` no construtor, e o ASP.NET Core entrega a instância correta automaticamente.

### Os três ciclos de vida: Transient, Scoped e Singleton

Quando registramos um serviço no container de DI, precisamos dizer **quanto tempo** aquela instância deve viver. Existem três opções:

- **Transient (`AddTransient`)**: uma **nova instância é criada toda vez** que o serviço é solicitado, mesmo que seja dentro da mesma requisição. É ideal para serviços leves, sem estado (stateless), e que não guardam nenhuma informação entre chamadas.

- **Scoped (`AddScoped`)**: **uma única instância é criada por requisição HTTP**. Ou seja, durante todo o processamento de uma requisição (do início ao fim), todo mundo que pedir aquele serviço vai receber a **mesma** instância. Quando a requisição termina, essa instância é descartada. É exatamente o que usamos para o `AppDbContext`, já que `AddDbContext` registra o contexto como **Scoped por padrão**.

- **Singleton (`AddSingleton`)**: **uma única instância é criada para toda a aplicação**, e essa mesma instância é reaproveitada em **todas** as requisições, de **todos** os usuários, durante todo o tempo de vida do servidor.

### Por que o banco de dados nunca deve ser Singleton?

O `DbContext` do Entity Framework **não é thread-safe**. Isso significa que ele não foi projetado para ser usado por múltiplas requisições simultaneamente. Se ele fosse registrado como Singleton:

- Vários usuários acessando o sistema ao mesmo tempo estariam compartilhando a **mesma instância** do contexto, e suas operações de leitura/escrita no banco se misturariam, causando erros de concorrência e dados corrompidos.
- O EF Core mantém um **cache interno de entidades rastreadas** (change tracker) por instância de contexto. Com Singleton, esse cache cresceria indefinidamente e nunca seria liberado, levando a vazamento de memória.
- Uma requisição poderia, sem querer, salvar ou alterar dados de outra requisição que ainda estivesse em andamento.

Por isso, **Scoped é a escolha correta**: cada requisição recebe seu próprio `AppDbContext`, isolado, que vive apenas durante aquela requisição e é descartado (`Dispose`) ao final, liberando a conexão com o banco.

---

## 2. 🪄 A Mágica do Banco de Dados (EF Core e ORM)

### O que é uma ferramenta ORM e qual a vantagem para o tempo de desenvolvimento

**ORM (Object-Relational Mapper)** é uma ferramenta que faz a “tradução” entre o mundo orientado a objetos (classes, propriedades, listas) e o mundo relacional do banco de dados (tabelas, colunas, chaves estrangeiras). Em vez de escrever SQL manualmente, o desenvolvedor trabalha apenas com classes C# comuns.

No nosso projeto, classes como `Produto`, `Cliente` e `Livro` são simples classes C# (POCOs), e o EF Core as mapeia automaticamente para tabelas do banco. O `AppDbContext` expõe essas entidades como `DbSet<T>`:

```csharp
public DbSet<Produto> Produtos => Set<Produto>();
public DbSet<Cliente> Clientes => Set<Cliente>();
public DbSet<Livro> Livros => Set<Livro>();
```

A grande **vantagem para o time** é a produtividade: em vez de escrever `INSERT INTO`, `SELECT * FROM` e `UPDATE` manualmente, basta escrever código C# como `_context.Livros.Add(novoLivro)` e `_context.SaveChanges()`. Isso:

- Reduz drasticamente a quantidade de código repetitivo (boilerplate).
- Diminui o risco de erros de digitação em SQL e de vulnerabilidades como **SQL Injection**, já que o EF Core gera as queries de forma parametrizada e segura.
- Permite que o desenvolvedor pense no domínio do problema (livros, clientes) em vez de sintaxe de banco de dados.
- Facilita a troca de banco de dados no futuro, já que boa parte do código não depende do provedor específico (SQLite, SQL Server, PostgreSQL).

### O que significa trabalhar com a abordagem Code-First

**Code-First** significa que o **código é a fonte da verdade** do modelo de dados — o banco de dados é uma consequência do código, e não o contrário.

Na prática, isso quer dizer:

1. O desenvolvedor escreve as classes C# (entidades), como `Produto` e `Cliente`.
2. O EF Core analisa essas classes e suas propriedades.
3. A partir delas, o EF Core **gera a estrutura do banco** (tabelas, colunas, tipos, chaves primárias e estrangeiras).

É o oposto da abordagem **Database-First**, em que o banco já existe e as classes são geradas a partir dele. No nosso projeto, em nenhum momento escrevemos um `CREATE TABLE` — apenas modelamos as classes em C#, e o EF Core cuidou da estrutura do banco.

### Como funcionam as Migrations

As **Migrations** são o mecanismo do EF Core para evoluir o esquema do banco de dados de forma controlada e versionada, conforme o modelo de classes muda ao longo do tempo.

O fluxo de trabalho típico com Migrations é:

1. O desenvolvedor altera uma entidade (ex: adiciona uma propriedade `Email` na classe `Cliente`).
2. Roda `dotnet ef migrations add NomeDaMigration` no terminal.
3. O EF Core compara o **modelo atual das classes** com o **último snapshot salvo** (um arquivo C# gerado automaticamente que representa o estado anterior do banco) e gera um novo arquivo de migration contendo os métodos `Up()` (o que aplicar) e `Down()` (como reverter).
4. Ao rodar `dotnet ef database update`, o EF Core executa, em ordem, todas as migrations que ainda não foram aplicadas naquele banco.

Para saber **o que já foi criado e o que é novo**, o EF Core mantém, dentro do próprio banco de dados, uma tabela de controle chamada **`__EFMigrationsHistory`**. Essa tabela guarda o nome de cada migration já aplicada. Assim, ao rodar `database update`, o EF Core:

- Consulta a tabela `__EFMigrationsHistory`.
- Compara com a lista de migrations existentes no projeto.
- Aplica **apenas as que ainda não constam** na tabela de histórico, na ordem correta.

> **Observação importante sobre o estado atual do projeto:** atualmente, o `Program.cs` deste projeto usa `db.Database.EnsureCreated()` em vez de Migrations:
> ```csharp
> var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
> db.Database.EnsureCreated();
> ```
> O `EnsureCreated()` é uma abordagem mais simples: ele cria o banco e as tabelas **de uma vez**, com base no modelo atual, mas **não gera histórico de versões** e **não consegue aplicar alterações incrementais** em um banco que já existe (se o modelo mudar, ele não atualiza o banco automaticamente). É adequado para protótipos e ambientes de estudo, mas **não deve ser usado em produção**. O projeto já possui os pacotes `Microsoft.EntityFrameworkCore.Design` e `Microsoft.EntityFrameworkCore.Tools` instalados, então a migração para o uso de Migrations reais (`dotnet ef migrations add` / `dotnet ef database update`) é simples e recomendada como próximo passo de evolução técnica.

---

## 3. 🗄️ O Limite do nosso SQLite

### Vantagens do SQLite no ambiente de desenvolvimento e testes

O SQLite é o banco de dados configurado atualmente no projeto (`Data Source=biblioteca.db`), e isso traz vantagens claras **para a fase atual**:

- **Zero configuração (Zero-config)**: não é preciso instalar nem configurar um servidor de banco de dados separado. O banco inteiro é um único arquivo `.db`.
- **Portabilidade**: o arquivo `biblioteca.db` pode ser copiado, versionado e movido junto com o projeto, facilitando demonstrações e testes locais.
- **Velocidade de setup**: qualquer novo desenvolvedor do time clona o repositório, roda o projeto, e o banco já é criado automaticamente — sem precisar de credenciais, IP de servidor ou instalação adicional.
- **Leveza**: por rodar embutido no próprio processo da aplicação (não é client-servidor), tem baixíssimo overhead, ideal para desenvolvimento, testes automatizados e demonstrações.

### O grande ponto fraco do SQLite em concorrência

O SQLite tem uma limitação estrutural conhecida: ele utiliza **bloqueio em nível de arquivo (file-level locking)**. Isso significa que, embora o SQLite permita **múltiplas leituras simultâneas**, ele só permite **uma única escrita por vez** no banco inteiro. Enquanto uma escrita está em andamento, todas as outras tentativas de escrita (e, dependendo do modo de jornal usado, até leituras) precisam **esperar na fila**.

Em um cenário com **10.000 acessos simultâneos**, especialmente se houver um volume razoável de operações de escrita (cadastrar cliente, registrar empréstimo de livro, etc.), isso se torna um gargalo crítico:

- As escritas concorrentes são **serializadas** (uma de cada vez), gerando filas de espera.
- Em alta concorrência, isso resulta em **erros de "database is locked"** (banco bloqueado) e tempos de resposta inaceitáveis para os usuários.
- Diferente de bancos cliente-servidor, o SQLite não foi projetado para gerenciar múltiplas conexões de rede concorrentes vindas de servidores diferentes — ele assume, em geral, um único processo acessando o arquivo.

### Quando migrar para um banco robusto em nuvem (PostgreSQL/SQL Server)

A migração deixa de ser opcional e passa a ser necessária quando aparecem sinais como:

- **Alta concorrência de escrita**: muitos usuários cadastrando, atualizando ou excluindo dados ao mesmo tempo — exatamente o cenário dos 10.000 acessos simultâneos mencionado pelo CTO.
- **Necessidade de escalar horizontalmente**: rodar a aplicação em múltiplos servidores/instâncias ao mesmo tempo (load balancing), o que exige um banco que aceite conexões de rede de várias origens simultaneamente — algo que o SQLite, por ser um arquivo local, não suporta bem.
- **Necessidade de recursos avançados**: controle de acesso granular por usuário no próprio banco, replicação, backups automatizados em nuvem, alta disponibilidade (failover) e auditoria robusta.
- **Crescimento do volume de dados**: bancos como PostgreSQL e SQL Server são otimizados para grandes volumes de dados com índices avançados e otimizador de consultas mais sofisticado.

Bancos como **PostgreSQL** ou **SQL Server** resolvem esses problemas porque são bancos **cliente-servidor**: rodam como um serviço independente, aceitam múltiplas conexões de rede simultâneas, têm controle de concorrência muito mais sofisticado (MVCC — *Multiversion Concurrency Control*, no caso do PostgreSQL) e permitem que várias escritas aconteçam de forma muito mais eficiente e segura ao mesmo tempo.

A boa notícia é que, graças ao **EF Core** (pilar 2 deste relatório), essa migração é relativamente simples no nosso projeto: como o código já trabalha com abstrações do EF Core (`DbContext`, `DbSet<T>`, LINQ) em vez de SQL puro, a troca se resume principalmente a:

1. Trocar o pacote NuGet do provedor (`Microsoft.EntityFrameworkCore.Sqlite` → `Npgsql.EntityFrameworkCore.PostgreSQL`, por exemplo).
2. Trocar `options.UseSqlite(...)` por `options.UseNpgsql(...)` no `Program.cs`.
3. Atualizar a string de conexão.
4. Recriar/aplicar as Migrations no novo banco.

---

## Conclusão

A base tecnológica atual (ASP.NET Core MVC + EF Core + SQLite) é uma escolha **sólida e correta para a fase de desenvolvimento e testes** do Sistema Biblioteca. A Injeção de Dependência garante um código desacoplado e testável; o EF Core, como ORM Code-First, acelera o desenvolvimento e elimina a necessidade de SQL manual; e o SQLite oferece simplicidade máxima para essa etapa do projeto.

Entretanto, para suportar a meta de **10.000 acessos simultâneos**, é necessário planejar a migração para um banco cliente-servidor robusto, como PostgreSQL ou SQL Server, além de formalizar o uso de **Migrations** (em vez de `EnsureCreated()`) como prática de versionamento de banco de dados — preparando o projeto para um ambiente de produção real.
