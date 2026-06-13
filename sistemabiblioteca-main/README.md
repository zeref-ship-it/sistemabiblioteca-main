# 🚀 ASP.NET Core MVC Web Application

Uma aplicação web robusta construída com a plataforma **.NET Core**, seguindo o padrão de arquitetura **MVC (Model-View-Controller)**.

---

## 🛠️ Tecnologias e Recursos Utilizados

*   **Framework:** ASP.NET Core (com C#)
*   **Padrão Arquitetural:** Model-View-Controller (MVC)


---

## ✅ Melhorias aplicadas

Este projeto recebeu as seguintes melhorias em relação à versão original:

- **Persistência com banco de dados (Entity Framework Core + SQLite)**: as antigas listas estáticas em memória (`_produtos`, `clientes`) foram substituídas por um `AppDbContext` real. Os dados agora ficam salvos em `biblioteca.db` e sobrevivem a reinícios da aplicação. Dados iniciais são populados automaticamente por `Data/DbSeeder.cs` na primeira execução.
- **Autenticação real da área administrativa**: o login agora usa autenticação por cookie (`CookieAuthenticationDefaults`). O `AdminController` e a listagem de clientes (`CadastroCliente/Index`) usam `[Authorize]`, então não é mais possível acessar `/Admin` direto pela URL sem estar logado. Foi adicionado um botão "Sair" (logout).
- **Rota padrão corrigida**: a página inicial pública (`Home/Index`) agora é a página padrão do site, em vez da área administrativa.
- **Validações no modelo `Produto`**: agora possui `[Required]`, `[StringLength]` e `[Range]`, com mensagens de erro em português, e as views `Criar`/`Editar` exibem essas mensagens.
- **Validações no modelo `Cliente`**: e-mail e telefone validados, com mensagens de erro exibidas no formulário de cadastro.
- **Correção de bug**: havia dois livros cadastrados com `Id = 11` na lista inicial; isso foi corrigido na nova base de dados (gerada via `DbSeeder`).
- **Limpeza de arquivos**: removidos `Models/Login.cshtml` e `Models/site.css`, que eram cópias soltas/erradas de arquivos que já existiam em `Views/Account/` e `wwwroot/css/`.

## ▶️ Como executar

```bash
dotnet restore
dotnet run
```

Na primeira execução, o arquivo `biblioteca.db` (SQLite) será criado automaticamente na raiz do projeto, já com os livros e produtos de exemplo.

Login administrativo de teste: `admin@admin.com` / `123`

> Se o `dotnet restore` falhar por incompatibilidade de versão dos pacotes do Entity Framework Core, rode:
> ```bash
> dotnet add package Microsoft.EntityFrameworkCore.Sqlite
> dotnet add package Microsoft.EntityFrameworkCore.Design
> dotnet add package Microsoft.EntityFrameworkCore.Tools
> ```
> para que o `dotnet` escolha automaticamente as versões compatíveis com o seu SDK instalado.

## 🔜 Próximos passos sugeridos

- Unificar os modelos `Livro` e `Produto` (hoje o sistema mistura conceito de "biblioteca" com "loja").
- Trocar a autenticação simplificada por **ASP.NET Core Identity** com senhas com hash.
- Adicionar controle de **empréstimos** (qual cliente está com qual livro e a data de devolução).
- Usar `dotnet ef migrations` em vez de `EnsureCreated()` para evoluir o esquema do banco com mais controle.
