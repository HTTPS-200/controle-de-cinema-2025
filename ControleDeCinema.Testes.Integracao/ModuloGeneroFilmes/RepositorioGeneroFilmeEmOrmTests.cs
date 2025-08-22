using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModuloGeneroFilmes;


[TestClass]
[TestCategory("Integracao -  GeneroFilme")]
public sealed class RepositorioGeneroFilmeEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");

        // Act
        repositorioGeneroFilme!.Cadastrar(generoFilme);
        dbContext!.SaveChanges();

        // Assert
        var registro = repositorioGeneroFilme.SelecionarRegistroPorId(generoFilme.Id);
        Assert.AreEqual(generoFilme, registro);

    }

    [TestMethod]
    public void Deve_Selecionar_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilme1 = new GeneroFilme("Ação");
        var generoFilme2 = new GeneroFilme("Ficção");
        var generoFilme3 = new GeneroFilme("Suspense");
        repositorioGeneroFilme!.Cadastrar(generoFilme1);
        repositorioGeneroFilme.Cadastrar(generoFilme2);
        repositorioGeneroFilme.Cadastrar(generoFilme3);

        dbContext!.SaveChanges();
        List<GeneroFilme> listGeneroFilmes = [generoFilme1, generoFilme2, generoFilme3];

        // Act
        var registros = repositorioGeneroFilme.SelecionarRegistros();

        // Assert
        CollectionAssert.Equals(listGeneroFilmes, registros);
    }

    [TestMethod]
    public void Deve_Editar_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        repositorioGeneroFilme!.Cadastrar(generoFilme);
        dbContext!.SaveChanges();

        var generoFilmeEditado = new GeneroFilme("Aventura");
        // Act
        var editadoComSucesso = repositorioGeneroFilme.Editar(generoFilme.Id, generoFilmeEditado);
        dbContext.SaveChanges();

        // Assert
        var registroSelecionado = repositorioGeneroFilme.SelecionarRegistroPorId(generoFilme.Id);
        Assert.IsTrue(editadoComSucesso);
        Assert.AreEqual(generoFilme, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        repositorioGeneroFilme!.Cadastrar(generoFilme);
        dbContext!.SaveChanges();

        // Act
        var excluidoComSucesso = repositorioGeneroFilme.Excluir(generoFilme.Id);
        dbContext.SaveChanges();
        // Assert

        var registroSelecionado = repositorioGeneroFilme.SelecionarRegistroPorId(generoFilme.Id);
        Assert.IsTrue(excluidoComSucesso);
        Assert.IsNull(registroSelecionado);
    }
}
