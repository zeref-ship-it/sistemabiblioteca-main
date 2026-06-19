RELATÓRIO TÉCNICO – SISTEMA BIBLIOTECA (ASP.NET CORE MVC + EF CORE + SQLITE)

**Autor:** Luiz Carlos Caetano
**Disciplina:** PROGRAMAÇÃO PARA WEB II - PROF RUBEN PRADO
**Contexto:** Desafio técnico — relatório para o CTO sobre a base tecnológica do projeto

---

## Introdução

Este relatório apresenta os principais componentes tecnológicos utilizados no desenvolvimento do Sistema Biblioteca. O objetivo é explicar as escolhas realizadas durante a implementação do projeto, destacando o uso da Injeção de Dependência no ASP.NET Core, o papel do Entity Framework Core como ferramenta de acesso a dados e as características do SQLite como banco de dados utilizado nesta fase do desenvolvimento.

Além disso, também apresento algumas limitações identificadas durante o projeto e os próximos passos previstos para a evolução da aplicação.

# 1. Injeção de Dependência no ASP.NET Core

## O que é Injeção de Dependência e por que ela foi utilizada

A Injeção de Dependência (Dependency Injection – DI) é um padrão de desenvolvimento que permite que uma classe receba os objetos necessários para seu funcionamento sem precisar criá-los diretamente. Em vez de instanciar dependências dentro do próprio código, elas são fornecidas automaticamente pelo framework.

Durante o desenvolvimento do Sistema Biblioteca utilizei esse recurso principalmente para disponibilizar o AppDbContext aos controladores da aplicação. Dessa forma, os controladores ficam responsáveis apenas pelas regras de negócio e pelo processamento das requisições, sem precisar conhecer detalhes de configuração do banco de dados.

Um exemplo disso pode ser observado no construtor dos controladores:

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

Essa abordagem reduz o acoplamento entre os componentes do sistema e facilita futuras alterações na infraestrutura da aplicação.

Caso o contexto fosse criado manualmente em cada controlador, seria necessário repetir configurações e modificar diversos trechos de código sempre que ocorresse alguma mudança relacionada ao banco de dados.

No projeto, toda a configuração ficou centralizada no arquivo Program.cs:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=biblioteca.db"));
```

Com isso, qualquer controlador que necessite acessar o banco apenas solicita o AppDbContext e o ASP.NET Core fornece automaticamente a instância adequada.

## Ciclos de vida dos serviços

Ao registrar serviços no container de dependências do ASP.NET Core, é necessário definir seu ciclo de vida.

O ciclo Transient cria uma nova instância toda vez que o serviço é solicitado. Esse modelo costuma ser utilizado em serviços leves e sem armazenamento de estado.

O ciclo Scoped cria uma única instância durante cada requisição HTTP. Todos os componentes envolvidos naquela requisição utilizam a mesma instância. Esse é o comportamento padrão utilizado pelo Entity Framework Core para o DbContext.

Já o ciclo Singleton cria apenas uma instância durante toda a execução da aplicação. Essa mesma instância é compartilhada por todos os usuários e requisições.

## Por que o DbContext não deve ser Singleton

O DbContext não foi desenvolvido para ser compartilhado simultaneamente entre várias requisições. Caso fosse registrado como Singleton, poderiam ocorrer problemas de concorrência, compartilhamento indevido de dados e aumento excessivo do consumo de memória.

Além disso, o Entity Framework mantém internamente informações sobre as entidades monitoradas. Em um cenário de longa duração, isso poderia provocar degradação de desempenho e comportamentos inesperados.

Por esse motivo, o ciclo Scoped é considerado a opção mais adequada para aplicações web, garantindo que cada requisição trabalhe de forma isolada.

# 2. Entity Framework Core e ORM

## O papel do ORM no desenvolvimento

O Entity Framework Core é um ORM (Object-Relational Mapper), ou seja, uma ferramenta que realiza o mapeamento entre objetos da aplicação e estruturas relacionais do banco de dados.

Na prática, isso significa que posso trabalhar diretamente com classes C# sem precisar escrever comandos SQL para as operações mais comuns.

No Sistema Biblioteca foram criadas entidades como Livro, Cliente e Produto. Essas entidades são expostas através do AppDbContext por meio de propriedades DbSet:

```csharp
public DbSet<Produto> Produtos => Set<Produto>();
public DbSet<Cliente> Clientes => Set<Cliente>();
public DbSet<Livro> Livros => Set<Livro>();
```

Essa abordagem trouxe ganhos significativos de produtividade durante o desenvolvimento, pois permitiu focar na implementação das funcionalidades do sistema em vez de dedicar tempo à construção manual de consultas SQL.

Operações como inserção, atualização e remoção de registros passaram a ser realizadas através de métodos disponibilizados pelo próprio framework.

Outro benefício importante é a proteção contra ataques de SQL Injection, já que o Entity Framework realiza a parametrização adequada das consultas geradas.

## Utilização da abordagem Code First

Neste projeto optei pela estratégia Code First.

Nessa abordagem, as classes da aplicação representam a fonte principal de definição da estrutura dos dados. Conforme novas entidades ou propriedades são criadas, o Entity Framework pode gerar automaticamente as tabelas e relacionamentos necessários.

Essa estratégia facilitou bastante o desenvolvimento inicial, pois permitiu modelar o sistema diretamente em código, sem a necessidade de criar manualmente scripts SQL para cada alteração.

## Migrations e evolução do banco de dados

As Migrations são o mecanismo utilizado pelo Entity Framework para controlar a evolução do esquema do banco de dados.

Sempre que uma entidade sofre alterações, é possível gerar uma migration contendo as modificações necessárias para atualizar a estrutura existente sem perder dados já armazenados.

Atualmente o projeto utiliza o método:

```csharp
db.Database.EnsureCreated();
```

Essa solução foi suficiente durante a fase inicial de desenvolvimento e testes, pois cria automaticamente o banco e suas tabelas quando necessário.

Entretanto, reconheço que essa abordagem possui limitações para ambientes mais avançados, já que não mantém histórico de alterações nem permite o gerenciamento adequado da evolução do banco de dados.

Por esse motivo, uma das próximas melhorias planejadas é substituir o EnsureCreated() pelo uso completo de Migrations, tornando o processo de manutenção e atualização mais seguro e profissional.

# 3. Avaliação do SQLite no Projeto

## Benefícios do SQLite durante o desenvolvimento

O SQLite foi escolhido para a fase atual do Sistema Biblioteca devido à sua simplicidade de utilização.

Como se trata de um banco baseado em arquivo, não há necessidade de instalar ou configurar um servidor dedicado. Isso permitiu iniciar o desenvolvimento rapidamente e simplificou a distribuição do projeto para testes.

Outro ponto positivo é a portabilidade. O banco fica armazenado em um único arquivo, facilitando cópias, backups e compartilhamento entre diferentes ambientes de desenvolvimento.

Para projetos acadêmicos e protótipos, essas características tornam o SQLite uma alternativa bastante eficiente.

## Limitações relacionadas à concorrência

Apesar das vantagens, o SQLite apresenta limitações importantes quando o volume de acessos cresce.

Seu mecanismo de bloqueio trabalha principalmente em nível de arquivo, permitindo múltiplas leituras simultâneas, mas restringindo operações de escrita concorrentes.

Em um cenário com grande quantidade de usuários realizando cadastros, atualizações ou empréstimos ao mesmo tempo, podem ocorrer filas de processamento, aumento do tempo de resposta e erros relacionados ao bloqueio do banco.

Por esse motivo, o SQLite é mais indicado para ambientes de desenvolvimento, testes ou aplicações de pequeno porte.

## Migração futura para PostgreSQL ou SQL Server

Pensando em uma futura implantação em produção, considero necessária a migração para um banco de dados cliente-servidor, como PostgreSQL ou SQL Server.

Essas plataformas oferecem mecanismos mais avançados de concorrência, melhor gerenciamento de conexões, suporte a grandes volumes de dados e recursos corporativos importantes, como replicação, backup automatizado e alta disponibilidade.

Uma vantagem da arquitetura atual é que a utilização do Entity Framework reduz significativamente o impacto dessa mudança.

Grande parte da aplicação permanecerá inalterada, sendo necessário principalmente:

* Substituir o provedor de banco de dados utilizado pelo Entity Framework;
* Atualizar a string de conexão;
* Configurar o novo servidor;
* Gerar e aplicar as migrations no ambiente de destino.

Dessa forma, a transição tende a ser relativamente simples quando houver necessidade de escalar a aplicação.

# Conclusão

Durante o desenvolvimento do Sistema Biblioteca, a combinação entre ASP.NET Core MVC, Entity Framework Core e SQLite atendeu adequadamente aos objetivos do projeto.

A Injeção de Dependência contribuiu para uma estrutura mais organizada e desacoplada. O Entity Framework Core facilitou o acesso aos dados e acelerou a implementação das funcionalidades. Já o SQLite permitiu criar e testar o sistema rapidamente, sem a complexidade de um servidor de banco de dados completo.

Apesar disso, algumas melhorias já estão previstas para as próximas etapas, especialmente a adoção de Migrations como mecanismo oficial de versionamento do banco e a futura migração para uma solução mais robusta, como PostgreSQL ou SQL Server.

Essas mudanças permitirão que a aplicação esteja mais preparada para cenários de crescimento e para uma possível utilização em ambiente de produção.
