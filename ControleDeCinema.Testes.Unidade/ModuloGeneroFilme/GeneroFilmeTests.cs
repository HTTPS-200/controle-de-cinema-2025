using Microsoft.EntityFrameworkCore;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Infraestrutura.Orm.ModuloGeneroFilme;
using ControleDeCinema.Infraestrutura.Orm.Compartilhado;

namespace ControleDeCinema.Testes.Unidade.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes - Unidade - GeneroFilme")]
public sealed class GeneroFilmeTests
{
    protected ControleDeCinemaDbContext? dbContext;
    protected RepositorioGeneroFilmeEmOrm? repositorioGeneroFilme;

    [TestInitialize]
    public void ConfigurarTeste()
    {
        var options = new DbContextOptionsBuilder<ControleDeCinemaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        dbContext = new ControleDeCinemaDbContext(options);
        dbContext.Database.EnsureCreated();

        repositorioGeneroFilme = new RepositorioGeneroFilmeEmOrm(dbContext);
    }


    //Sistema não lança exceções para campos obrigatórios ou duplicados
    [TestMethod]
    public void Deve_Validar_Nome_Obrigatorio_E_Nome_Duplicado_Ao_Cadastrar_Genero()
    {
        var repositorio = repositorioGeneroFilme ?? throw new InvalidOperationException("Repositorio não inicializado.");

        try
        {
            var generoInvalido = new GeneroFilme("");
            Assert.Fail("Cadastro com nome vazio deveria lançar exceção.");
        }
        catch (ArgumentException ex)
        {
            Assert.AreEqual("O campo 'Nome' é obrigatório.", ex.Message);
        }

        var generoOriginal = new GeneroFilme("Ação");
        repositorio.Cadastrar(generoOriginal);

        try
        {
            var generoDuplicado = new GeneroFilme("Ação");
            repositorio.Cadastrar(generoDuplicado);
            Assert.Fail("Cadastro com nome duplicado deveria lançar exceção.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual("Já existe um gênero com esse nome.", ex.Message);
        }
    }
}
