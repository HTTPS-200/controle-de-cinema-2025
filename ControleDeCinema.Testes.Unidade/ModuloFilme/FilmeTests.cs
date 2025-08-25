using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;

namespace ControleDeCinema.Testes.Unidade.ModuloFilme;

[TestClass]
[TestCategory("Testes - Unidade - Filme")]
public sealed class FilmeTests
{
    private Filme? filme;

    // Teste campos obrigatorios, Nome do filme e Duração  CT018 / CT019
    [TestMethod]
    public void Deve_Impedir_Cadastro_Filme_Com_Campos_Obrigatorios_Vazios()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        filme = new Filme("", 0, true, genero);

        // Act
        var tituloVazio = string.IsNullOrWhiteSpace(filme.Titulo);
        var duracaoInvalida = filme.Duracao <= 0;

        // Assert
        Assert.IsTrue(tituloVazio, "O título do filme não pode estar vazio.");
        Assert.IsTrue(duracaoInvalida, "A duração do filme deve ser maior que zero.");
    }


    // Teste titulo duplicado CT020
    [TestMethod]
    public void Deve_Impedir_Cadastro_Filme_Com_Titulo_Duplicado()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var listFilme = new List<Filme>
        {
            new Filme("Vingadores", 120, true, genero),
            new Filme("Vigadores: Ultimato", 180, true, genero),
            new Filme("Batman", 150, false, genero)
        };

        var novoFilme = new Filme("Vingadores", 150, false, genero);

        // Act
        var tituloDuplicado = listFilme.Any(f =>
        f.Titulo.Equals(novoFilme.Titulo, StringComparison.OrdinalIgnoreCase));

        // Assert
        Assert.IsTrue(tituloDuplicado, "Já existe um filme registrado com este título.");
    }
}
