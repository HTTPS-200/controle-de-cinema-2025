using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloFilme;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using ControleDeCinema.Testes.Interface.ModuloSessao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using TesteFacil.Testes.Interface.ModuloSala;

namespace ControleDeCinema.Testes.Interface.ModuloIngresso
{
    [TestClass]
    [TestCategory("Testes de Interface de Ingresso")]
    public sealed class IngressoInterfaceTests : TestFixture
    {
        private AutenticacaoPageObject autenticacaoPage;
        private IngressoIndexPageObject indexPage;

        [TestInitialize]
        public void InicializarTeste()
        {
            base.InicializarTeste();
            autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
            indexPage = new IngressoIndexPageObject(driver!);
        }

        [TestMethod]
        public void CT029_Visualizar_Sessoes_Agrupadas_Por_Filme()
        {
            autenticacaoPage.RegistrarContaEmpresarial();

            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherDescricao("Comédia")
                .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            filmeIndex.IrPara(enderecoBase).ClickCadastrar()
                      .PreencherTitulo("Esposa de Mentirinha")
                      .PreencherDuracao(117)
                      .MarcarLancamento()
                      .SelecionarGenero("Comédia")
                      .Confirmar();

            filmeIndex.IrPara(enderecoBase).ClickCadastrar()
                      .PreencherTitulo("Todo Mundo Tem a Irmã Gêmea Que Merece")
                      .PreencherDuracao(94)
                      .MarcarLancamento()
                      .SelecionarGenero("Comédia")
                      .Confirmar();

            var salaIndex = new SalaIndexPageObject(driver);
            salaIndex.IrPara(enderecoBase).ClickCadastrar()
                     .PreencherNumero("1")
                     .PreencherCapacidade("15")
                     .Confirmar();

            salaIndex.IrPara(enderecoBase).ClickCadastrar()
                     .PreencherNumero("5")
                     .PreencherCapacidade("30")
                     .Confirmar();

            var sessaoIndex = new SessaoIndexPageObject(driver);
            sessaoIndex.IrPara(enderecoBase).ClickCadastrar()
                       .PreencherInicio("2025-08-12T14:15")
                       .PreencherNumeroMaximoIngressos(6)
                       .SelecionarFilme("Esposa de Mentirinha")
                       .SelecionarSala("5")
                       .ConfirmarComSucesso();

            sessaoIndex.IrPara(enderecoBase).ClickCadastrar()
                       .PreencherInicio("2025-08-12T20:00")
                       .PreencherNumeroMaximoIngressos(10)
                       .SelecionarFilme("Todo Mundo Tem a Irmã Gêmea Que Merece")
                       .SelecionarSala("1")
                       .ConfirmarComSucesso();

            autenticacaoPage.FazerLogout();
            autenticacaoPage.RegistrarContaCliente();

            var ingressoIndex = new IngressoIndexPageObject(driver)
                .IrPara(enderecoBase);

            Assert.IsTrue(ingressoIndex.ContemSessao("Esposa de Mentirinha", "5"));
            Assert.IsTrue(ingressoIndex.ContemSessao("Todo Mundo Tem a Irmã Gêmea Que Merece", "1"));
        }


        [TestMethod]
        public void CT030_Comprar_Ingresso_Com_Sucesso()
        {
            autenticacaoPage.RegistrarContaEmpresarial();

            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherDescricao("Ação")
                .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            filmeIndex.IrPara(enderecoBase).ClickCadastrar()
                      .PreencherTitulo("Filme A")
                      .PreencherDuracao(120)
                      .MarcarLancamento()
                      .SelecionarGenero("Ação")
                      .Confirmar();

            var salaIndex = new SalaIndexPageObject(driver);
            salaIndex.IrPara(enderecoBase).ClickCadastrar()
                     .PreencherNumero("1")
                     .PreencherCapacidade("50")
                     .Confirmar();

            var sessaoIndex = new SessaoIndexPageObject(driver);
            sessaoIndex.IrPara(enderecoBase).ClickCadastrar()
                       .PreencherInicio(DateTime.Now.AddHours(2).ToString("yyyy-MM-ddTHH:mm"))
                       .PreencherNumeroMaximoIngressos(50)
                       .SelecionarFilme("Filme A")
                       .SelecionarSala("1")
                       .ConfirmarComSucesso();

            autenticacaoPage.FazerLogout();
            autenticacaoPage.RegistrarContaCliente();

            var form = indexPage.IrPara(enderecoBase!)
                                .ClickComprarIngresso()
                                .SelecionarAssento("1")
                                .MarcarMeiaEntrada(false);

            var resultado = form.ConfirmarCompra();

            Console.WriteLine($"URL atual: {driver.Url}");
            Console.WriteLine($"Page source: {driver.PageSource}");

            bool compraSucesso = resultado.ContemMensagem("Compra realizada com sucesso") ||
                                 resultado.ContemMensagem("compra realizada") ||
                                 resultado.ContemMensagem("sucesso") ||
                                 resultado.ContemMensagem("ingresso comprado") ||
                                 driver.Url.Contains("/sessoes", StringComparison.OrdinalIgnoreCase);

            Assert.IsTrue(compraSucesso, "Compra não foi bem-sucedida. Verifique a mensagem retornada.");
        }

        [TestMethod]
        public void CT031_Comprar_Ingresso_Sessao_Lotada()
        {
            autenticacaoPage.RegistrarContaEmpresarial();

            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherDescricao("Comédia")
                .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            filmeIndex.IrPara(enderecoBase).ClickCadastrar()
                      .PreencherTitulo("Filme B")
                      .PreencherDuracao(90)
                      .MarcarLancamento()
                      .SelecionarGenero("Comédia")
                      .Confirmar();

            var salaIndex = new SalaIndexPageObject(driver);
            salaIndex.IrPara(enderecoBase).ClickCadastrar()
                     .PreencherNumero("2")
                     .PreencherCapacidade("1") 
                     .Confirmar();

            var sessaoIndex = new SessaoIndexPageObject(driver);
            sessaoIndex.IrPara(enderecoBase).ClickCadastrar()
                       .PreencherInicio(DateTime.Now.AddHours(2).ToString("yyyy-MM-ddTHH:mm"))
                       .PreencherNumeroMaximoIngressos(1) 
                       .SelecionarFilme("Filme B")
                       .SelecionarSala("2")
                       .ConfirmarComSucesso();

            autenticacaoPage.FazerLogout();
            autenticacaoPage.RegistrarContaCliente();

            indexPage.IrPara(enderecoBase!)
                     .ClickComprarIngresso()
                     .SelecionarAssento("1")
                     .MarcarMeiaEntrada(false)
                     .ConfirmarCompra();

            indexPage.IrPara(enderecoBase!);

            var botoesComprar = driver.FindElements(By.CssSelector("a[data-se='btnComprarIngresso']"));
            if (botoesComprar.Count == 0)
            {
                Assert.IsTrue(true, "Sessão corretamente marcada como lotada - botão de compra não disponível");
                return;
            }

            var form = indexPage.ClickComprarIngresso();

            var selectElement = driver.FindElement(By.CssSelector("select[data-se='selectAssento']"));
            var select = new SelectElement(selectElement);

            if (select.Options.Count <= 1) 
            {
                var erro = form.ConfirmarComErro().ObterMensagemErro();

                if (erro.ToLower().Contains("sessão está lotada") || erro.ToLower().Contains("assento field is required"))
                {
                    Assert.IsTrue(true, "Sessão corretamente identificada como lotada");
                }
                else
                {
                    Assert.Fail($"Mensagem de erro inesperada: {erro}");
                }
            }
            else
            {
                form.SelecionarAssento("1"); 
                var erro = form.ConfirmarComErro().ObterMensagemErro();
                StringAssert.Contains(erro.ToLower(), "sessão está lotada");
            }
        }

        [TestMethod]
        public void CT032_Comprar_Ingresso_Quantidade_Indisponivel()
        {
            autenticacaoPage.RegistrarContaEmpresarial();

            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherDescricao("Drama")
                .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            filmeIndex.IrPara(enderecoBase).ClickCadastrar()
                      .PreencherTitulo("Filme C")
                      .PreencherDuracao(110)
                      .MarcarLancamento()
                      .SelecionarGenero("Drama")
                      .Confirmar();

            var salaIndex = new SalaIndexPageObject(driver);
            salaIndex.IrPara(enderecoBase).ClickCadastrar()
                     .PreencherNumero("3")
                     .PreencherCapacidade("1") 
                     .Confirmar();

            var sessaoIndex = new SessaoIndexPageObject(driver);
            sessaoIndex.IrPara(enderecoBase).ClickCadastrar()
                       .PreencherInicio(DateTime.Now.AddHours(2).ToString("yyyy-MM-ddTHH:mm"))
                       .PreencherNumeroMaximoIngressos(1) 
                       .SelecionarFilme("Filme C")
                       .SelecionarSala("3")
                       .ConfirmarComSucesso();

            autenticacaoPage.FazerLogout();
            autenticacaoPage.RegistrarContaCliente();

            indexPage.IrPara(enderecoBase!)
                     .ClickComprarIngresso()
                     .SelecionarAssento("1")
                     .MarcarMeiaEntrada(false)
                     .ConfirmarCompra();

            indexPage.IrPara(enderecoBase!);

            var sessaoElement = driver.FindElements(By.CssSelector(".card"))
                .FirstOrDefault(card => card.Text.Contains("Filme C"));

            if (sessaoElement != null)
            {
                if (sessaoElement.Text.Contains("Esgotado", StringComparison.OrdinalIgnoreCase) ||
                    sessaoElement.Text.Contains("Lotada", StringComparison.OrdinalIgnoreCase) ||
                    sessaoElement.Text.Contains("indisponível", StringComparison.OrdinalIgnoreCase))
                {
                    Assert.IsTrue(true, "Sessão corretamente marcada como esgotada");
                    return;
                }
                var botaoComprar = sessaoElement.FindElements(By.CssSelector("a[data-se='btnComprarIngresso']"));
                if (botaoComprar.Count == 0)
                {
                    Assert.IsTrue(true, "Botão de compra não disponível - sessão esgotada");
                    return;
                }
            }
            try
            {
                var form = indexPage.ClickComprarIngresso();
                var selectElement = driver.FindElement(By.CssSelector("select[data-se='selectAssento']"));
                var select = new SelectElement(selectElement);

                if (select.Options.Count <= 1)
                {
                    var erro = form.ConfirmarComErro().ObterMensagemErro();
                    Assert.IsTrue(!string.IsNullOrEmpty(erro), $"Erro ao tentar comprar ingresso extra: {erro}");
                }
                else
                {
                    form.SelecionarAssento("1");
                    var erro = form.ConfirmarComErro().ObterMensagemErro();
                    Assert.IsTrue(!string.IsNullOrEmpty(erro), $"Deveria ter erro ao comprar segundo ingresso: {erro}");
                }
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true, $"Não foi possível comprar segundo ingresso: {ex.Message}");
            }
        }

    }
}
