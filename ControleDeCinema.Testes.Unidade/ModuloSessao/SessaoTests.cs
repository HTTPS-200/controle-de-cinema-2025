using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleDeCinema.Testes.Unidade.ModuloSessao
{
    [TestClass]
    [TestCategory("Testes de Unidade de Sessão")]
    public sealed class SessaoTests
    {
        private Sala sala;
        private Filme filme;

        [TestInitialize]
        public void Setup()
        {
            sala = new Sala(1, 50);
            var genero = new GeneroFilme("Ação");
            filme = new Filme("Matrix", 120, false, genero);
        }

        [TestMethod]
        public void Deve_Criar_Sessao_Corretamente()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 30, filme, sala);

            Assert.AreEqual(filme, sessao.Filme);
            Assert.AreEqual(sala, sessao.Sala);
            Assert.AreEqual(30, sessao.NumeroMaximoIngressos);
        }

        [TestMethod]
        public void Deve_Impedir_NumeroMaximoIngressos_Maior_Que_Capacidade()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 100, filme, sala);

            Assert.IsTrue(sessao.NumeroMaximoIngressos > sala.Capacidade);
        }

        [TestMethod]
        public void Deve_Impedir_Conflito_Horario()
        {
            var inicio = DateTime.Now.AddHours(1);
            var sessaoExistente = new Sessao(inicio, 30, filme, sala);
            var novaSessao = new Sessao(inicio, 25, filme, sala);

            var conflito = new List<Sessao> { sessaoExistente }
                .Any(s => s.Sala.Id == novaSessao.Sala.Id && s.Inicio == novaSessao.Inicio);

            Assert.IsTrue(conflito);
        }

        [TestMethod]
        public void Deve_Vender_Ingresso_Corretamente()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 2, filme, sala);
            var ingresso = sessao.GerarIngresso(1, false);

            Assert.AreEqual(1, ingresso.NumeroAssento);
            Assert.IsFalse(ingresso.MeiaEntrada);
            Assert.AreEqual(1, sessao.Ingressos.Count);
        }

        [TestMethod]
        public void Nao_Pode_Vender_Assento_Ocupado()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 2, filme, sala);
            sessao.GerarIngresso(1, false);

            var assentoOcupado = sessao.Ingressos.Any(i => i.NumeroAssento == 1);
            Assert.IsTrue(assentoOcupado);
        }

        [TestMethod]
        public void Nao_Pode_Vender_Quando_Sessao_Encerrada()
        {
            var sessao = new Sessao(DateTime.Now.AddHours(1), 2, filme, sala);
            sessao.Encerrar();

            Assert.IsTrue(sessao.Encerrada);
        }
    }
}
