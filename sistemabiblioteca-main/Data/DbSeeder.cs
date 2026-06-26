using sistemabiblioteca.Models;

namespace sistemabiblioteca.Data;

/// <summary>
/// Popula o banco de dados com dados iniciais, caso ele esteja vazio.
/// Catálogo temático de RPG (D&D, Pathfinder, fantasia, sistemas e suplementos).
/// </summary>
public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Produtos.Any())
        {
            context.Produtos.AddRange(
                // ── Dungeons & Dragons ──
                new Produto { Nome = "D&D: Livro do Jogador", Autor = "Wizards of the Coast", Descricao = "O guia essencial para criar e jogar personagens de Dungeons & Dragons 5ª edição. Raças, classes, magias e regras de combate.", Preco = 249.90m, Categoria = "D&D", ImagemUrl = "/images/livros/dnd-livro-jogador.svg", Estoque = 25 },
                new Produto { Nome = "D&D: Guia do Mestre", Autor = "Wizards of the Coast", Descricao = "Ferramentas e conselhos para o Mestre conduzir aventuras memoráveis, criar mundos e desafiar heróis.", Preco = 259.90m, Categoria = "D&D", ImagemUrl = "/images/livros/dnd-mestre.svg", Estoque = 18 },
                new Produto { Nome = "D&D: Manual dos Monstros", Autor = "Wizards of the Coast", Descricao = "Centenas de criaturas para povoar suas masmorras: dragões, demônios, mortos-vivos e horrores indescritíveis.", Preco = 249.90m, Categoria = "D&D", ImagemUrl = "/images/livros/dnd-monstros.svg", Estoque = 20 },

                // ── Pathfinder ──
                new Produto { Nome = "Pathfinder: Regras Básicas", Autor = "Paizo Publishing", Descricao = "Sistema de RPG de fantasia profundo e personalizável. Tudo o que você precisa para começar suas aventuras.", Preco = 229.90m, Categoria = "Pathfinder", ImagemUrl = "/images/livros/pathfinder-core.svg", Estoque = 15 },
                new Produto { Nome = "Pathfinder: Bestiário", Autor = "Paizo Publishing", Descricao = "Coletânea de monstros ilustrados para o sistema Pathfinder, com estatísticas completas e lore.", Preco = 209.90m, Categoria = "Pathfinder", ImagemUrl = "/images/livros/pathfinder-bestiario.svg", Estoque = 12 },

                // ── Sistemas / Outros RPGs ──
                new Produto { Nome = "Tormenta20", Autor = "Jambô Editora", Descricao = "O maior RPG de fantasia brasileiro. Explore o mundo de Arton com regras dinâmicas e cheias de ação.", Preco = 199.90m, Categoria = "Sistema RPG", ImagemUrl = "/images/livros/tormenta20.svg", Estoque = 30 },
                new Produto { Nome = "Call of Cthulhu", Autor = "Sandy Petersen", Descricao = "RPG de horror cósmico baseado nos contos de H.P. Lovecraft. Investigue mistérios e enfrente a loucura.", Preco = 189.90m, Categoria = "Sistema RPG", ImagemUrl = "/images/livros/call-of-cthulhu.svg", Estoque = 14 },
                new Produto { Nome = "Vampiro: A Máscara", Autor = "White Wolf", Descricao = "Interprete um vampiro em um mundo de trevas góticas, intrigas políticas e sede de sangue.", Preco = 219.90m, Categoria = "Sistema RPG", ImagemUrl = "/images/livros/vampiro-mascara.svg", Estoque = 10 },

                // ── Romances de Fantasia ──
                new Produto { Nome = "O Nome do Vento", Autor = "Patrick Rothfuss", Descricao = "A lendária história de Kvothe, do menino prodígio ao mago mais notório de seu tempo. Crônica do Matador do Rei.", Preco = 64.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/o-nome-do-vento.svg", Estoque = 40 },
                new Produto { Nome = "O Hobbit", Autor = "J.R.R. Tolkien", Descricao = "A jornada de Bilbo Bolseiro pela Terra-média em busca do tesouro guardado pelo dragão Smaug.", Preco = 49.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/o-hobbit.svg", Estoque = 50 },
                new Produto { Nome = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien", Descricao = "A épica trilogia que definiu a alta fantasia. A Sociedade do Anel parte para destruir o Um Anel.", Preco = 119.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/senhor-aneis.svg", Estoque = 35 },
                new Produto { Nome = "O Caminho dos Reis", Autor = "Brandon Sanderson", Descricao = "Primeiro volume de O Arquivo das Tempestades. Um mundo de tempestades mágicas e antigas ordens de cavaleiros.", Preco = 89.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/caminho-dos-reis.svg", Estoque = 22 },
                new Produto { Nome = "Mistborn: O Império Final", Autor = "Brandon Sanderson", Descricao = "Em um mundo onde a cinza cai do céu, uma ladra descobre poderes de Alomancia para derrubar um deus-imperador.", Preco = 74.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/mistborn.svg", Estoque = 28 },
                new Produto { Nome = "A Guerra dos Tronos", Autor = "George R.R. Martin", Descricao = "O primeiro livro de As Crônicas de Gelo e Fogo. Intrigas, traições e guerra pelo Trono de Ferro.", Preco = 79.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/game-of-thrones.svg", Estoque = 33 },
                new Produto { Nome = "Conan, o Bárbaro", Autor = "Robert E. Howard", Descricao = "As aventuras clássicas de espada e feitiçaria do mais famoso bárbaro da Era Hiboriana.", Preco = 59.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/conan.svg", Estoque = 19 },
                new Produto { Nome = "The Witcher: O Último Desejo", Autor = "Andrzej Sapkowski", Descricao = "Conheça Geralt de Rívia, o bruxo caçador de monstros. Coletânea de contos que iniciam a saga.", Preco = 54.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/witcher.svg", Estoque = 26 },
                new Produto { Nome = "Dragonlance: Dragões do Crepúsculo", Autor = "Weis & Hickman", Descricao = "Companheiros heróicos enfrentam o retorno dos dragões e da Rainha das Trevas em Krynn.", Preco = 62.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/dragonlance.svg", Estoque = 17 },
                new Produto { Nome = "Reinos Esquecidos: Salvação", Autor = "R.A. Salvatore", Descricao = "As aclamadas aventuras de Drizzt Do'Urden, o elfo negro, no cenário dos Reinos Esquecidos de D&D.", Preco = 57.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/forgotten-realms.svg", Estoque = 21 },

                // ── Suplementos / Acessórios temáticos ──
                new Produto { Nome = "Manual de Masmorras e Armadilhas", Autor = "Guilda dos Mestres", Descricao = "Guia repleto de mapas, masmorras prontas e armadilhas engenhosas para desafiar seus jogadores.", Preco = 99.90m, Categoria = "Suplemento", ImagemUrl = "/images/livros/manual-masmorras.svg", Estoque = 16 },
                new Produto { Nome = "Grimório de Feitiços Arcanos", Autor = "Arquimago Eldrin", Descricao = "Centenas de magias ilustradas e descritas para conjuradores de todos os níveis. Um tomo essencial.", Preco = 109.90m, Categoria = "Suplemento", ImagemUrl = "/images/livros/grimorio-feiticos.svg", Estoque = 13 },
                new Produto { Nome = "Compêndio das Bestas Lendárias", Autor = "Ordem dos Caçadores", Descricao = "Catálogo de criaturas raras e lendárias, suas fraquezas, ecologia e tesouros guardados.", Preco = 94.90m, Categoria = "Suplemento", ImagemUrl = "/images/livros/compendio-bestas.svg", Estoque = 18 },
                new Produto { Nome = "Atlas dos Reinos Perdidos", Autor = "Cartógrafo Real", Descricao = "Mapas detalhados de continentes, cidades e masmorras para ambientar suas campanhas.", Preco = 84.90m, Categoria = "Suplemento", ImagemUrl = "/images/livros/atlas-reinos.svg", Estoque = 15 },
                new Produto { Nome = "Crônicas do Reino Anão", Autor = "Thorin Forjabarba", Descricao = "A história e a cultura dos anões: suas forjas, runas, batalhas e fortalezas subterrâneas.", Preco = 69.90m, Categoria = "Fantasia", ImagemUrl = "/images/livros/cronicas-anao.svg", Estoque = 20 },
                new Produto { Nome = "O Livro dos Dados do Destino", Autor = "Mestre Aleatório", Descricao = "Tabelas de eventos aleatórios, tesouros e encontros para enriquecer qualquer sessão de RPG.", Preco = 47.90m, Categoria = "Suplemento", ImagemUrl = "/images/livros/dados-destino.svg", Estoque = 24 }
            );
        }

        if (!context.Livros.Any())
        {
            context.Livros.AddRange(
                new Livro { Titulo = "D&D: Livro do Jogador", Autor = "Wizards of the Coast" },
                new Livro { Titulo = "D&D: Guia do Mestre", Autor = "Wizards of the Coast" },
                new Livro { Titulo = "D&D: Manual dos Monstros", Autor = "Wizards of the Coast" },
                new Livro { Titulo = "Pathfinder: Regras Básicas", Autor = "Paizo Publishing" },
                new Livro { Titulo = "Pathfinder: Bestiário", Autor = "Paizo Publishing" },
                new Livro { Titulo = "Tormenta20", Autor = "Jambô Editora" },
                new Livro { Titulo = "Call of Cthulhu", Autor = "Sandy Petersen" },
                new Livro { Titulo = "Vampiro: A Máscara", Autor = "White Wolf" },
                new Livro { Titulo = "O Nome do Vento", Autor = "Patrick Rothfuss" },
                new Livro { Titulo = "O Hobbit", Autor = "J.R.R. Tolkien" },
                new Livro { Titulo = "O Senhor dos Anéis", Autor = "J.R.R. Tolkien" },
                new Livro { Titulo = "O Caminho dos Reis", Autor = "Brandon Sanderson" },
                new Livro { Titulo = "Mistborn: O Império Final", Autor = "Brandon Sanderson" },
                new Livro { Titulo = "A Guerra dos Tronos", Autor = "George R.R. Martin" },
                new Livro { Titulo = "Conan, o Bárbaro", Autor = "Robert E. Howard" },
                new Livro { Titulo = "The Witcher: O Último Desejo", Autor = "Andrzej Sapkowski" },
                new Livro { Titulo = "Dragonlance: Dragões do Crepúsculo", Autor = "Weis & Hickman" },
                new Livro { Titulo = "Reinos Esquecidos: Salvação", Autor = "R.A. Salvatore" }
            );
        }

        context.SaveChanges();
    }
}
