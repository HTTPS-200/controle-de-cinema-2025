using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModuloFilme;

[TestClass]
[TestCategory("Integração - Filme")]
public sealed class RepositorioFilmeEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        repositorioGeneroFilme?.Cadastrar(genero);
        dbContext!.SaveChanges();
        var filme = new Filme("Vingadores", 120, true, genero);

        // Act
        repositorioFilme?.Cadastrar(filme);
        dbContext.SaveChanges();

        // Assert
        var registro = repositorioFilme.SelecionarRegistroPorId(filme.Id);
        Assert.AreEqual(filme, registro);
    }

    [TestMethod]
    public void Deve_Editar_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        repositorioGeneroFilme!.Cadastrar(genero);
        dbContext!.SaveChanges();

        var filme = new Filme("Vingadores", 120, true, genero);
        repositorioFilme?.Cadastrar(filme);
        dbContext.SaveChanges();

        var filmeEditado = new Filme("Vingadores: Ultimato", 180, true, genero);

        // Act
        var editadoComSucesso = repositorioFilme?.Editar(filme.Id, filmeEditado);
        dbContext.SaveChanges();

        // Assert
        var registroSelecionado = repositorioFilme?.SelecionarRegistroPorId(filme.Id);
        Assert.IsTrue(editadoComSucesso);
        Assert.AreEqual(filme, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        repositorioGeneroFilme!.Cadastrar(genero);
        dbContext!.SaveChanges();

        var filme = new Filme("Vingadores", 120, true, genero);
        repositorioFilme?.Cadastrar(filme);
        dbContext.SaveChanges();

        // Act
        var excluidoComSucesso = repositorioFilme?.Excluir(filme.Id);
        dbContext.SaveChanges();

        // Assert
        var registroSelecionado = repositorioFilme?.SelecionarRegistroPorId(filme.Id);
        Assert.IsTrue(excluidoComSucesso);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        repositorioGeneroFilme!.Cadastrar(genero);
        dbContext!.SaveChanges();

        var filme1 = new Filme("Vingadores", 120, true, genero);
        var filme2 = new Filme("Batman", 150, false, genero);
        var filme3 = new Filme("Spider-Man", 130, false, genero);

        repositorioFilme?.Cadastrar(filme1);
        repositorioFilme?.Cadastrar(filme2);
        repositorioFilme?.Cadastrar(filme3);
        dbContext.SaveChanges();

        List<Filme> listaFilmes = [filme1, filme2, filme3];
        var listFilmesOrdenados = listaFilmes.OrderBy(f => f.Titulo).ToList();

        // Act
        var registros = repositorioFilme?.SelecionarRegistros().OrderBy(f => f.Titulo).ToList();

        // Assert
        CollectionAssert.Equals(listFilmesOrdenados, registros);
    }

}
