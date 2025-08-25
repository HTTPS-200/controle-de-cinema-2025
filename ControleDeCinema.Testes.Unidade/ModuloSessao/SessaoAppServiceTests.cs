using ControleDeCinema.Aplicacao.ModuloSessao;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControledeCinema.Dominio.Compartilhado;

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
            tenantMock.Setup(t => t.IsInRole(It.IsAny<string>())).Returns(false);

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

        [TestMethod]
        public void Editar_Deve_RetornarOk_QuandoSessaoValida()
        {
            var id = Guid.NewGuid();
            var sessaoEditada = new Sessao(DateTime.Now.AddHours(2), 30, filme, sala);

            repositorioMock.Setup(r => r.SelecionarRegistros()).Returns(new List<Sessao>());
            repositorioMock.Setup(r => r.Editar(id, sessaoEditada)).Returns(true);

            var resultado = appService.Editar(id, sessaoEditada);

            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
            Assert.IsTrue(resultado.IsSuccess);
        }

        [TestMethod]
        public void Editar_Deve_RetornarFalha_QuandoNaoEncontrada()
        {
            var id = Guid.NewGuid();
            var sessaoEditada = new Sessao(DateTime.Now.AddHours(2), 30, filme, sala);

            repositorioMock.Setup(r => r.SelecionarRegistros()).Returns(new List<Sessao>());
            repositorioMock.Setup(r => r.Editar(id, sessaoEditada)).Returns(false);

            var resultado = appService.Editar(id, sessaoEditada);

            Assert.IsTrue(resultado.IsFailed);
        }

        [TestMethod]
        public void Excluir_Deve_RetornarOk_QuandoSucesso()
        {
            var id = Guid.NewGuid();
            repositorioMock.Setup(r => r.Excluir(id)).Returns(true);

            var resultado = appService.Excluir(id);

            unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
            Assert.IsTrue(resultado.IsSuccess);
        }

        [TestMethod]
        public void Excluir_Deve_RetornarFalha_QuandoNaoEncontrada()
        {
            var id = Guid.NewGuid();
            repositorioMock.Setup(r => r.Excluir(id)).Returns(false);

            var resultado = appService.Excluir(id);

            Assert.IsTrue(resultado.IsFailed);
        }

        [TestMethod]
        public void SelecionarPorId_Deve_RetornarSessao_QuandoEncontrada()
        {
            var id = Guid.NewGuid();
            var sessao = new Sessao(DateTime.Now, 20, filme, sala);
            repositorioMock.Setup(r => r.SelecionarRegistroPorId(id)).Returns(sessao);

            var resultado = appService.SelecionarPorId(id);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(sessao, resultado.Value);
        }

        [TestMethod]
        public void SelecionarPorId_Deve_RetornarFalha_QuandoNaoEncontrada()
        {
            var id = Guid.NewGuid();
            repositorioMock.Setup(r => r.SelecionarRegistroPorId(id)).Returns((Sessao)null);

            var resultado = appService.SelecionarPorId(id);

            Assert.IsTrue(resultado.IsFailed);
        }

        [TestMethod]
        public void VenderIngresso_Deve_RetornarOk_QuandoAssentoDisponivel()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 5, filme, sala);
            repositorioMock.Setup(r => r.SelecionarRegistroPorId(It.IsAny<Guid>())).Returns(sessao);

            var resultado = appService.VenderIngresso(Guid.NewGuid(), 1, false);

            Assert.IsTrue(resultado.IsSuccess);
            Assert.AreEqual(1, resultado.Value.NumeroAssento);
            Assert.AreEqual(1, sessao.Ingressos.Count);
        }

        [TestMethod]
        public void VenderIngresso_Deve_RetornarFalha_QuandoAssentoOcupado()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 5, filme, sala);
            sessao.GerarIngresso(1, false);
            repositorioMock.Setup(r => r.SelecionarRegistroPorId(It.IsAny<Guid>())).Returns(sessao);

            var resultado = appService.VenderIngresso(Guid.NewGuid(), 1, false);

            Assert.IsTrue(resultado.IsFailed);
        }

        [TestMethod]
        public void Encerrar_Deve_RetornarOk_QuandoSessaoExiste()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 5, filme, sala);
            repositorioMock.Setup(r => r.SelecionarRegistroPorId(It.IsAny<Guid>())).Returns(sessao);

            var resultado = appService.Encerrar(Guid.NewGuid());

            Assert.IsTrue(resultado.IsSuccess);
            Assert.IsTrue(sessao.Encerrada);
        }
    }
}
