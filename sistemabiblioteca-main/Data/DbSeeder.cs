using sistemabiblioteca.Models;

namespace sistemabiblioteca.Data;

/// <summary>
/// Popula o banco de dados com dados iniciais, caso ele esteja vazio.
/// Substitui as antigas listas estáticas em memória dos controllers.
/// </summary>
public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Produtos.Any())
        {
            context.Produtos.AddRange(
                new Produto { Nome = "Dom Casmurro", Descricao = "Obra prima de Machado de Assis.", Preco = 35.00m, Categoria = "Livro" },
                new Produto { Nome = "O Alquimista", Descricao = "Fábula mística de Paulo Coelho.", Preco = 42.90m, Categoria = "Livro" },
                new Produto { Nome = "A Revolução dos Bichos", Descricao = "Clássico de George Orwell sobre política e sociedade.", Preco = 29.90m, Categoria = "Livro" },
                new Produto { Nome = "1984", Descricao = "Distopia de George Orwell sobre vigilância e controle.", Preco = 39.90m, Categoria = "Livro" },
                new Produto { Nome = "O Pequeno Príncipe", Descricao = "Obra de Antoine de Saint-Exupéry sobre amizade e valores.", Preco = 24.90m, Categoria = "Livro" },
                new Produto { Nome = "Capitães da Areia", Descricao = "Romance de Jorge Amado sobre jovens em situação de rua.", Preco = 34.90m, Categoria = "Livro" },
                new Produto { Nome = "Memórias Póstumas de Brás Cubas", Descricao = "Clássico de Machado de Assis narrado por um defunto.", Preco = 37.50m, Categoria = "Livro" },
                new Produto { Nome = "Harry Potter e a Pedra Filosofal", Descricao = "Primeira aventura do jovem bruxo Harry Potter.", Preco = 49.90m, Categoria = "Livro" },
                new Produto { Nome = "Senhor dos Anéis: A Sociedade do Anel", Descricao = "Início da épica jornada criada por Tolkien.", Preco = 59.90m, Categoria = "Livro" },
                new Produto { Nome = "Código Limpo", Descricao = "Guia de boas práticas para desenvolvimento de software.", Preco = 89.90m, Categoria = "Livro" }
            );
        }

        if (!context.Livros.Any())
        {
            context.Livros.AddRange(
                new Livro { Titulo = "A Arte da Guerra", Autor = "Sun Tzu" },
                new Livro { Titulo = "Guerra e Paz", Autor = "Liev Tolstói" },
                new Livro { Titulo = "Nada de Novo no Front", Autor = "Erich Maria Remarque" },
                new Livro { Titulo = "Stalingrado", Autor = "Antony Beevor" },
                new Livro { Titulo = "A Segunda Guerra Mundial", Autor = "Winston Churchill" },
                new Livro { Titulo = "Tempestade de Aço", Autor = "Ernst Jünger" },
                new Livro { Titulo = "Band of Brothers", Autor = "Stephen E. Ambrose" },
                new Livro { Titulo = "A Guerra Não Tem Rosto de Mulher", Autor = "Svetlana Alexievich" },
                new Livro { Titulo = "O Diário de Anne Frank", Autor = "Anne Frank" },
                new Livro { Titulo = "Pearl Harbor", Autor = "Craig Nelson" },
                new Livro { Titulo = "A Queda de Berlim 1945", Autor = "Antony Beevor" },
                new Livro { Titulo = "Sobre a Guerra", Autor = "Carl von Clausewitz" },
                new Livro { Titulo = "Os Canhões de Agosto", Autor = "Barbara W. Tuchman" },
                new Livro { Titulo = "O Longo Caminho para Casa", Autor = "Ernest Hemingway" },
                new Livro { Titulo = "A Grande Guerra", Autor = "Marc Ferro" },
                new Livro { Titulo = "A Guerra do Peloponeso", Autor = "Tucídides" },
                new Livro { Titulo = "A Hora da Estrela", Autor = "Clarice Lispector" },
                new Livro { Titulo = "Vidas Secas", Autor = "Graciliano Ramos" },
                new Livro { Titulo = "Iracema", Autor = "José de Alencar" },
                new Livro { Titulo = "O Cortiço", Autor = "Aluísio Azevedo" },
                new Livro { Titulo = "Quincas Borba", Autor = "Machado de Assis" },
                new Livro { Titulo = "Triste Fim de Policarpo Quaresma", Autor = "Lima Barreto" },
                new Livro { Titulo = "Macunaíma", Autor = "Mário de Andrade" },
                new Livro { Titulo = "Grande Sertão: Veredas", Autor = "Guimarães Rosa" },
                new Livro { Titulo = "O Guarani", Autor = "José de Alencar" },
                new Livro { Titulo = "Auto da Compadecida", Autor = "Ariano Suassuna" }
            );
        }

        context.SaveChanges();
    }
}
