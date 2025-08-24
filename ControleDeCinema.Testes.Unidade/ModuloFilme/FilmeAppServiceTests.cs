using Microsoft.Extensions.Logging;
using Moq;
using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloFilme;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace ControleDeCinema.Testes.Unidade.ModuloFilme;

[TestClass]
[TestCategory("Testes - Unidade/Aplicação - Filme")]
public sealed class FilmeAppServiceTests
{
    private Mock<IRepositorioFilme>? repositorioFilmeMock;
    private Mock<ITenantProvider>? tenantProviderMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<FilmeAppService>>? loggerMock;

    private FilmeAppService? filmeAppService;

    [TestInitialize]
    public void Setup()
    { 
        repositorioFilmeMock = new Mock<IRepositorioFilme>();
        tenantProviderMock = new Mock<ITenantProvider>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<FilmeAppService>>();

        filmeAppService = new FilmeAppService(
            tenantProviderMock.Object,
            repositorioFilmeMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object
        );
    }

    [TestMethod]
    public void Deve_Retornar_OK_Quanto_Filme_For_Valido()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Contra o Tempo", 148, false, genero);
        
        var filmeTeste = new Filme("Iterception", 148, false, genero);

        repositorioFilmeMock!
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filmeTeste });

        // Act
        var resultado = filmeAppService!.Cadastrar(filme);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Deve_Retornar_Fail_Quanto_Filme_For_Duplicado()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Contra o Tempo", 148, false, genero);
        
        var filmeTeste = new Filme("Contra o Tempo", 148, false, genero);
        
        repositorioFilmeMock!
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filmeTeste });
        
        // Act
        var resultado = filmeAppService!.Cadastrar(filme);
        
        // Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);
       
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(1, resultado.Errors.Count);
    }

    [TestMethod]
    public void Deve_Retornar_Fail_Quando_Excessao_For_Lancada()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("Contra o Tempo", 148, false, genero);
        
        repositorioFilmeMock!
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>());

        repositorioFilmeMock!
            .Setup(r => r.Cadastrar(It.IsAny<Filme>()))
            .Throws(new Exception("Erro ao cadastrar filme"));

        // Act
        var resultado = filmeAppService!.Cadastrar(filme);
        
        // Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);
        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);
        
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual(1, resultado.Errors.Count);
    }

    // Testes adicionais para validação de dados que não foram implementados na classe FilmeAppService

    ////!!! Atualmente, o método Cadastrar da classe FilmeAppService não faz validação para duração negativa do filme. !!!
    //[TestMethod]
    //public void Deve_Retornar_Fail_Quando_Duração_For_Negativa()  
    //{
    //    // Arrange
    //    var genero = new GeneroFilme("Ação");
    //    var filme = new Filme("Contra o Tempo", -148, false, genero);

    //    var filmeTeste = new Filme("Iterception", 148, false, genero);

    //    repositorioFilmeMock!
    //        .Setup(r => r.SelecionarRegistros())
    //        .Returns(new List<Filme>() { filmeTeste });

    //    // Act
    //    var resultado = filmeAppService!.Cadastrar(filme);

    //    // Assert
    //    repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Never);
    //    unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

    //    Assert.IsNotNull(resultado);
    //    Assert.IsTrue(resultado.IsFailed);
    //    Assert.AreEqual(1, resultado.Errors.Count);
    //}

    ////!!! Atualmente, o método Cadastrar da classe FilmeAppService não faz validação para campos obrigatórios vazios. !!!
    //[TestMethod]
    //public void Deve_Retornar_Fail_Quando_Campos_Obrigatrios_Estiverem_Vazios()
    //{
    //    // Arrange
    //    var genero = new GeneroFilme("Ação");
    //    var filme = new Filme("", 0, false, genero);

    //    var filmeTeste = new Filme("Iterception", 148, false, genero);

    //    repositorioFilmeMock!
    //        .Setup(r => r.SelecionarRegistros())
    //        .Returns(new List<Filme>() { filmeTeste });

    //    // Act
    //    var resultado = filmeAppService!.Cadastrar(filme);

    //    // Assert
    //    repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Never);
    //    unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

    //    Assert.IsNotNull(resultado);
    //    Assert.IsTrue(resultado.IsFailed);
    //    Assert.AreEqual(1, resultado.Errors.Count);
    //}
}
