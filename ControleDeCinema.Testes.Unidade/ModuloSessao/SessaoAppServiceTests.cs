using ControleDeCinema.Aplicacao.ModuloSessao;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Dominio.ModuloAutenticacao;

namespace ControleDeCinema.Testes.Unidade.ModuloSessao
{
    [TestClass]
    [TestCategory("Testes de Unidade de Sessão")]
    public sealed class SessaoAppServiceTests
    {
        private Mock<IRepositorioSessao> repositorioMock;
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<ITenantProvider> tenantMock;
        private Mock<ILogger<SessaoAppService>> loggerMock;
        private SessaoAppService appService;
        private Sala sala;
        private Filme filme;

        [TestInitialize]
        public void Setup()
        {
            repositorioMock = new Mock<IRepositorioSessao>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            tenantMock = new Mock<ITenantProvider>();
            loggerMock = new Mock<ILogger<SessaoAppService>>();

            tenantMock.Setup(t => t.UsuarioId).Returns(Guid.NewGuid());

            appService = new SessaoAppService(
                tenantMock.Object,
                repositorioMock.Object,
                unitOfWorkMock.Object,
                loggerMock.Object
            );

            sala = new Sala(1, 50);
            var genero = new GeneroFilme("Ação");
            filme = new Filme("Matrix", 120, false, genero);
        }

        [TestMethod]
        public void Cadastrar_Deve_RetornarOk_QuandoSessaoValida()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 30, filme, sala);
            repositorioMock.Setup(r => r.SelecionarRegistros()).Returns(new List<Sessao>());

            var resultado = appService.Cadastrar(sessao);

            repositorioMock.Verify(r => r.Cadastrar(sessao), Times.Once);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
            Assert.IsTrue(resultado.IsSuccess);
        }

        [TestMethod]
        public void Cadastrar_Deve_RetornarFalha_QuandoSessaoDuplicada()
        {
            var inicio = DateTime.Now.AddHours(1);
            var sessaoExistente = new Sessao(inicio, 30, filme, sala);
            var novaSessao = new Sessao(inicio, 25, filme, sala);

            repositorioMock.Setup(r => r.SelecionarRegistros())
                .Returns(new List<Sessao> { sessaoExistente });

            var resultado = appService.Cadastrar(novaSessao);

            repositorioMock.Verify(r => r.Cadastrar(It.IsAny<Sessao>()), Times.Never);
            unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
            Assert.IsTrue(resultado.IsFailed);
        }

        [TestMethod]
        public void Cadastrar_Deve_RetornarFalha_QuandoNumeroMaximoIngressosExcede()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 100, filme, sala);

            repositorioMock.Setup(r => r.SelecionarRegistros()).Returns(new List<Sessao>());

            var resultado = appService.Cadastrar(sessao);

            Assert.IsTrue(resultado.IsFailed);
        }

        [TestMethod]
        public void Cadastrar_Deve_RetornarFalha_QuandoExcecaoForLancada()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 30, filme, sala);
            repositorioMock.Setup(r => r.SelecionarRegistros()).Returns(new List<Sessao>());
            unitOfWorkMock.Setup(u => u.Commit()).Throws(new Exception("Erro inesperado"));

            var resultado = appService.Cadastrar(sessao);

            unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            Assert.IsTrue(resultado.IsFailed);
        }
    }
}
