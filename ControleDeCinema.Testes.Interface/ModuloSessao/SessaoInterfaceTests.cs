using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloFilme;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TesteFacil.Testes.Interface.ModuloSala;

namespace ControleDeCinema.Testes.Interface.ModuloSessao
{
    [TestClass]
    [TestCategory("Testes de Interface de Sessão")]
    public sealed class SessaoInterfaceTests : TestFixture
    {
        private AutenticacaoPageObject autenticacaoPage;

        [TestInitialize]
        public void InicializarTeste()
        {
            base.InicializarTeste();
            autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
            autenticacaoPage.RegistrarContaEmpresarial();
        }

        [TestMethod]
        public void CT001_Deve_Cadastrar_Nova_Sessao()
        {
            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherDescricao("Comédia")
                .ConfirmarComSucesso();

            // Arrange - Cadastrar filme
            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            var filmeForm = filmeIndex.IrPara(enderecoBase)
                                      .ClickCadastrar();
            filmeForm
                .PreencherTitulo("Esposa de Mentirinha")
                .PreencherDuracao(117)
                .MarcarLancamento()
                .SelecionarGenero("Comédia")
                .ConfirmarComSucesso();

            // Arrange - Cadastrar sala
            var salaIndex = new SalaIndexPageObject(driver);
            salaIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherNumero("5")
                .PreencherCapacidade("50")
                .Confirmar();

            // Act - Cadastrar sessão
            var sessaoIndex = new SessaoIndexPageObject(driver);
            var sessaoForm = sessaoIndex
                .IrPara(enderecoBase)
                .ClickCadastrar();

            sessaoForm
                .PreencherInicio("2025-08-12T14:15")
                .PreencherNumeroMaximoIngressos(12)
                .SelecionarFilme("Esposa de Mentirinha")
                .SelecionarSala("5")
                .ConfirmarComSucesso();

            // Assert
            Assert.IsTrue(sessaoIndex.ContemSessao("Esposa de Mentirinha"));
        }

        [TestMethod]
        public void CT002_Deve_Editar_Sessao_Existente()
        {
            var sessaoIndex = new SessaoIndexPageObject(driver!).IrPara(enderecoBase!);
            var sessaoForm = sessaoIndex.ClickCadastrar();

            sessaoForm
                .PreencherInicio("31/08/2025 18:00")
                .PreencherNumeroMaximoIngressos(40)
                .SelecionarFilme("Predador")
                .SelecionarSala("102")
                .ConfirmarComSucesso();

            sessaoForm = sessaoIndex.ClickEditar();
            sessaoForm
                .PreencherInicio("31/08/2025 19:00")
                .PreencherNumeroMaximoIngressos(45)
                .ConfirmarComSucesso();

            Assert.IsTrue(sessaoIndex.ContemSessao("Predador"));
        }

        [TestMethod]
        public void CT003_Deve_Excluir_Sessao()
        {
            var sessaoIndex = new SessaoIndexPageObject(driver!).IrPara(enderecoBase!);
            var sessaoForm = sessaoIndex.ClickCadastrar();

            sessaoForm
                .PreencherInicio("31/08/2025 16:00")
                .PreencherNumeroMaximoIngressos(30)
                .SelecionarFilme("Predador")
                .SelecionarSala("103")
                .ConfirmarComSucesso();

            sessaoForm = sessaoIndex.ClickExcluir();
            sessaoForm.Confirmar();

            Assert.IsFalse(sessaoIndex.ContemSessao("Predador"));
        }

        [TestMethod]
        public void CT004_Deve_Validar_Campos_Obrigatorios()
        {
            var sessaoIndex = new SessaoIndexPageObject(driver!).IrPara(enderecoBase!);
            var sessaoForm = sessaoIndex.ClickCadastrar();
            sessaoForm.ConfirmarComErro();

            string erro = sessaoForm.ObterMensagemErro();
            StringAssert.Contains(erro.ToLower(), "obrigatório");
        }
    }
}
