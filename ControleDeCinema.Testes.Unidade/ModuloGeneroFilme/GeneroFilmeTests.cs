using ControleDeCinema.Dominio.ModuloGeneroFilme;

namespace ControleDeCinema.Testes.Unidade.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes - Unidade - GeneroFilme")]
public sealed class GeneroFilmeTests
{
    private GeneroFilme? genero;
    protected IRepositorioGeneroFilme? repositorioGeneroFilme;

    [TestMethod]
    public void Deve_Impedir_Cadastro_Genero_Com_Descricao_Vazia()
    {
        // Arrange
        genero = new GeneroFilme("");

        // Act
        try
        {
            repositorioGeneroFilme?.Cadastrar(genero);
        }
        catch (Exception ex)
        {
            // Assert
            Assert.IsInstanceOfType(ex, typeof(ArgumentException));
            Assert.AreEqual("A descrição do gênero não pode estar vazia.", ex.Message);
        }
    }


    [TestMethod]
    public void Deve_Impedir_Cadastro_Genero_Com_Descricao_Duplicada()
    {
        // Arrange
        var genero1 = new GeneroFilme("Ação");
        var genero2 = new GeneroFilme("Comédia");
        var genero3 = new GeneroFilme("Drama");

        repositorioGeneroFilme?.Cadastrar(genero1);
        repositorioGeneroFilme?.Cadastrar(genero2);
        repositorioGeneroFilme?.Cadastrar(genero3);

        var listaGeneros = repositorioGeneroFilme?.SelecionarRegistros() ?? new List<GeneroFilme>();

        // Act
        var novoGenero = new GeneroFilme("Ação");
        
        try
        {
            repositorioGeneroFilme?.Cadastrar(novoGenero);
        }
        catch (Exception ex)
        {
            // Assert
            Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            Assert.AreEqual("Já existe um gênero registrado com esta descrição.", ex.Message);
        }
    }
}
