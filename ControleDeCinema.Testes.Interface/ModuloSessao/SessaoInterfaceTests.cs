using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloFilme;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
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

            driver!.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl(enderecoBase!);

            ((IJavaScriptExecutor)driver).ExecuteScript("window.localStorage.clear();");
            ((IJavaScriptExecutor)driver).ExecuteScript("window.sessionStorage.clear();");

            if (!autenticacaoPage.EstaLogado())
            {
                try
                {
                    autenticacaoPage.RegistrarContaEmpresarial();
                }
                catch (WebDriverTimeoutException)
                {
                    autenticacaoPage.FazerLogin("Empresa");
                }
                catch (NoSuchElementException)
                {
                    autenticacaoPage.FazerLogin("Empresa");
                }
            }
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

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            var filmeForm = filmeIndex.IrPara(enderecoBase)
                                      .ClickCadastrar();
            filmeForm
                .PreencherTitulo("Esposa de Mentirinha")
                .PreencherDuracao(117)
                .MarcarLancamento()
                .SelecionarGenero("Comédia")
                .ConfirmarComSucesso();

            var salaIndex = new SalaIndexPageObject(driver);
            salaIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherNumero("5")
                .PreencherCapacidade("50")
                .Confirmar();

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

            Assert.IsTrue(sessaoIndex.ContemSessao("Esposa de Mentirinha"));
        }

        [TestMethod]
        public void CT002_Deve_Editar_Sessao_Existente()
        {

            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherDescricao("Ação")
                .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            var filmeForm = filmeIndex.IrPara(enderecoBase)
                                      .ClickCadastrar();
            filmeForm
                .PreencherTitulo("Predador")
                .PreencherDuracao(107)
                .SelecionarGenero("Ação")
                .ConfirmarComSucesso();

            var salaIndex = new SalaIndexPageObject(driver);
            salaIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherNumero("102")
                .PreencherCapacidade("50")
                .Confirmar();

            var sessaoIndex = new SessaoIndexPageObject(driver!).IrPara(enderecoBase!);
            var sessaoForm = sessaoIndex.ClickCadastrar();

            sessaoForm
                .PreencherInicio("2025-08-31T18:00")
                .PreencherNumeroMaximoIngressos(40)
                .SelecionarFilme("Predador")  
                .SelecionarSala("102")
                .ConfirmarComSucesso();

            sessaoForm = sessaoIndex.ClickEditar();
            sessaoForm
                .PreencherInicio("2025-08-31T19:00")
                .PreencherNumeroMaximoIngressos(45)
                .ConfirmarComSucesso();


            Assert.IsTrue(sessaoIndex.ContemSessao("Predador"));
        }

        [TestMethod]
        public void CT003_Deve_Excluir_Sessao()
        {

            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherDescricao("Ação")
                .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            filmeIndex.IrPara(enderecoBase)
                      .ClickCadastrar()
                      .PreencherTitulo("Predador")
                      .PreencherDuracao(107)
                      .SelecionarGenero("Ação")
                      .ConfirmarComSucesso();

            var salaIndex = new SalaIndexPageObject(driver);
            salaIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherNumero("102")
                .PreencherCapacidade("50")
                .Confirmar();

            var sessaoIndex = new SessaoIndexPageObject(driver).IrPara(enderecoBase);
            sessaoIndex.ClickCadastrar()
                .PreencherInicio("2025-08-31T18:00")
                .PreencherNumeroMaximoIngressos(40)
                .SelecionarFilme("Predador")
                .SelecionarSala("102")
                .ConfirmarComSucesso();

            sessaoIndex = new SessaoIndexPageObject(driver).IrPara(enderecoBase);
            var encerramentoForm = sessaoIndex.ClickEncerrar();
            encerramentoForm.ConfirmarEncerramento()
                            .VoltarParaIndex();

            sessaoIndex = new SessaoIndexPageObject(driver).IrPara(enderecoBase);
            var excluirForm = sessaoIndex.ClickExcluir();
            excluirForm.ConfirmarExclusao();

            Assert.IsFalse(sessaoIndex.ContemSessao("Predador"));
        }


        [TestMethod]
        public void CT004_Deve_Validar_Campos_Obrigatorios()
        {
            var sessaoIndex = new SessaoIndexPageObject(driver!).IrPara(enderecoBase!);
            var sessaoForm = sessaoIndex.ClickCadastrar();
            sessaoForm.ConfirmarComErro();

            string erro = sessaoForm.ObterMensagemErro();
            StringAssert.Contains(erro.ToLower(), "ao menos 1");
        }
    }
}
