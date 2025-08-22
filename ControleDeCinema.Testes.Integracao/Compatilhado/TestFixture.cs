using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Infraestrutura.Orm.Compartilhado;
using ControleDeCinema.Infraestrutura.Orm.ModuloFilme;
using ControleDeCinema.Infraestrutura.Orm.ModuloGeneroFilme;
using ControleDeCinema.Infraestrutura.Orm.ModuloSala;
using ControleDeCinema.Infraestrutura.Orm.ModuloSessao;
using DotNet.Testcontainers.Containers;
using FizzWare.NBuilder;
using Testcontainers.PostgreSql;

namespace ControleDeCinema.Testes.Integracao.Compatilhado;

[TestClass]
public abstract class TestFixture
{
    protected ControleDeCinemaDbContext? dbContext;

    protected RepositorioFilmeEmOrm? repositorioFilme;
    protected RepositorioGeneroFilmeEmOrm? repositorioGeneroFilme;
    protected RepositorioSalaEmOrm? repositorioSala;
    protected RepositorioSessaoEmOrm? repositorioSessao;

    public static IDatabaseContainer? dbContainer;

    [AssemblyInitialize]
    public static async Task Setup(TestContext _)
    {
        dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithName("controle-de-cinema-container")
            .WithDatabase("controle-de-cinema-db")
            .WithUsername("postgres")
            .WithPassword("MinhaSenhaFraca")
            .WithCleanUp(true)
            .Build();

        await InicializarContainerBancoDadosAsync(dbContainer);   
    }

    [AssemblyCleanup]
    public static async Task Teardown()
    {
        await PararContainerBancoDadosAsync();
    }

    [TestInitialize]
    public void ConfigurarTeste()
    {
        if (dbContainer is null)
            throw new ArgumentNullException("O Banco de dados não foi inicializado.");

        dbContext = ControleDeCinemaDbContextFactory.CriarDbContext(dbContainer.GetConnectionString());

        ConfigurarTabelas(dbContext);

        repositorioFilme = new RepositorioFilmeEmOrm(dbContext);
        repositorioGeneroFilme = new RepositorioGeneroFilmeEmOrm(dbContext);
        repositorioSala = new RepositorioSalaEmOrm(dbContext);
        repositorioSessao = new RepositorioSessaoEmOrm(dbContext);

        BuilderSetup.SetCreatePersistenceMethod<GeneroFilme>(repositorioGeneroFilme.Cadastrar);
        BuilderSetup.SetCreatePersistenceMethod<IList<GeneroFilme>>(repositorioGeneroFilme.CadastrarEntidades);

        BuilderSetup.SetCreatePersistenceMethod<Filme>(repositorioFilme.Cadastrar);
        BuilderSetup.SetCreatePersistenceMethod<IList<Filme>>(repositorioFilme.CadastrarEntidades);

        BuilderSetup.SetCreatePersistenceMethod<Sala>(repositorioSala.Cadastrar);
        BuilderSetup.SetCreatePersistenceMethod<IList<Sala>>(repositorioSala.CadastrarEntidades);

        BuilderSetup.SetCreatePersistenceMethod<Sessao>(repositorioSessao.Cadastrar);
        BuilderSetup.SetCreatePersistenceMethod<IList<Sessao>>(repositorioSessao.CadastrarEntidades);
    }

    private static void ConfigurarTabelas(ControleDeCinemaDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.GenerosFilme.RemoveRange(dbContext.GenerosFilme);
        dbContext.Filmes.RemoveRange(dbContext.Filmes);
        dbContext.Salas.RemoveRange(dbContext.Salas);
        dbContext.Sessoes.RemoveRange(dbContext.Sessoes);

        dbContext.SaveChanges();
    }

    private static async Task InicializarContainerBancoDadosAsync(IDatabaseContainer dbContainer)
    {
        await dbContainer.StartAsync();
    }

    private static async Task PararContainerBancoDadosAsync()
    {
        if (dbContainer is null)
            throw new ArgumentNullException("O Banco de dados não foi inicializado.");

        await dbContainer.StopAsync();
        await dbContainer.DisposeAsync();
    }
}
