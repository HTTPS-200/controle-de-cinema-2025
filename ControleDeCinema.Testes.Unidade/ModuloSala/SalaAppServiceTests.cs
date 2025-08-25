using Moq;
using Microsoft.Extensions.Logging;
using ControleDeCinema.Aplicacao.ModuloSala;
using ControleDeCinema.Dominio.ModuloSala;
using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Dominio.ModuloAutenticacao; 

namespace ControleDeCinema.Testes.Unidade.ModuloSala;

[TestClass]
[TestCategory("Testes de Unidade de Sala")]
public sealed class SalaAppServiceTests
{
    private Mock<IRepositorioSala>? repositorioSalaMock;
    private Mock<IUnitOfWork>? unitOfWorkMock;
    private Mock<ILogger<SalaAppService>>? loggerMock;
    private Mock<ITenantProvider>? tenantProviderMock; 
    private SalaAppService? salaAppService;

    [TestInitialize]
    public void Setup()
    {
        repositorioSalaMock = new Mock<IRepositorioSala>();
        unitOfWorkMock = new Mock<IUnitOfWork>();
        loggerMock = new Mock<ILogger<SalaAppService>>();
        tenantProviderMock = new Mock<ITenantProvider>();

        // simular usuário logado
        tenantProviderMock
            .Setup(t => t.UsuarioId)
            .Returns(Guid.NewGuid());

        salaAppService = new SalaAppService(
            tenantProviderMock.Object,   
            repositorioSalaMock.Object,
            unitOfWorkMock.Object,
            loggerMock.Object
        );
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarOk_QuandoSalaForValida()
    {
        var sala = new Sala(1, 50);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>());

        var resultado = salaAppService?.Cadastrar(sala);

        repositorioSalaMock?.Verify(r => r.Cadastrar(sala), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoSalaDuplicada()
    {
        var sala = new Sala(1, 50);
        var salaExistente = new Sala(1, 60);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>() { salaExistente });

        var resultado = salaAppService?.Cadastrar(sala);

        repositorioSalaMock?.Verify(r => r.Cadastrar(It.IsAny<Sala>()), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_DeveRetornarFalha_QuandoExcecaoForLancada()
    {
        var sala = new Sala(2, 100);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>());

        unitOfWorkMock?
            .Setup(u => u.Commit())
            .Throws(new Exception("Erro inesperado"));

        var resultado = salaAppService?.Cadastrar(sala);

        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);
        Assert.IsNotNull(resultado);
        var mensagemErro = resultado.Errors.First().Message;
        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }
}
