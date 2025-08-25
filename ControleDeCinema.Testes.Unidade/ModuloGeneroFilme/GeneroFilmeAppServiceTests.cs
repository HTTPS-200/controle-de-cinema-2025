using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes - Unidade/Aplicação - GeneroFilme")]
public sealed class GeneroFilmeAppServiceTests
{
    private Mock<IRepositorioGeneroFilme>? repositorioGeneroFilmeMock;
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<GeneroFilmeAppService>>? loggerMock;
    private GeneroFilmeAppService? generoFilmeAppService;

    [TestInitialize]
    public void Setup()
    {
        repositorioGeneroFilmeMock = new Mock<IRepositorioGeneroFilme>();
        tenantProviderMock = new Mock<ITenantProvider>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<GeneroFilmeAppService>>();
        generoFilmeAppService = new GeneroFilmeAppService(
            tenantProviderMock.Object,
            repositorioGeneroFilmeMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object
        );
    }

    [TestMethod]
    public void Deve_Inserir_GeneroFilme_Quando_Valido()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        
        repositorioGeneroFilmeMock!
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>());
        
        tenantProviderMock!
            .Setup(t => t.UsuarioId)
            .Returns(Guid.NewGuid());
        
        // Act
        var resultado = generoFilmeAppService!.Cadastrar(generoFilme);
        
        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Cadastrar(generoFilme), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Editar_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        var generoFilmeEditado = new GeneroFilme("Comédia") { Id = generoFilme.Id };
        
        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistroPorId(generoFilme.Id))
            .Returns(generoFilme);
        
        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme> { generoFilme });
        
        tenantProviderMock!
            .Setup(t => t.UsuarioId)
            .Returns(Guid.NewGuid());
        
        // Act
        var resultado = generoFilmeAppService?.Editar(generoFilme.Id, generoFilmeEditado);
        
        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Editar(generoFilme.Id, generoFilmeEditado), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Excluir_GeneroFilme_Corretamente()
    {
        // Arrange
        var generoFilme = new GeneroFilme("Ação");
        
        repositorioGeneroFilmeMock!
            .Setup(r => r.SelecionarRegistroPorId(generoFilme.Id))
            .Returns(generoFilme);
        
        tenantProviderMock!
            .Setup(t => t.UsuarioId)
            .Returns(Guid.NewGuid());
        
        // Act
        var resultado = generoFilmeAppService?.Excluir(generoFilme.Id);
        
        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Excluir(generoFilme.Id), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);
        
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Visualizar_GeneroFilme_Corretamente()
    {
        // Arrange
        var generofilmes = new List<GeneroFilme>
        {
            new GeneroFilme("Ação"),
            new GeneroFilme("Comédia"),
            new GeneroFilme("Drama")
        };

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(generofilmes);

        // Act
        var resultado = generoFilmeAppService?.SelecionarTodos();
        var QteFilmes = generofilmes.Count;

        // Assert
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
        Assert.AreEqual(QteFilmes, resultado.Value.Count);
    }
}
