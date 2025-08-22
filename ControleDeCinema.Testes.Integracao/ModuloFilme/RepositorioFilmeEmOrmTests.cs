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
        repositorioGeneroFilme!.Cadastrar(genero);
        dbContext!.SaveChanges();
        var filme = new Filme("Vingadores", 120, true, genero);

        // Act
        repositorioFilme!.Cadastrar(filme);
        dbContext.SaveChanges();

        // Assert
        var registro = repositorioFilme.SelecionarRegistroPorId(filme.Id);
        Assert.AreEqual(filme, registro);
    }

}
