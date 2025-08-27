using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace TesteFacil.Testes.Interface.ModuloSala;

[TestClass]
[TestCategory("Tests de Interface de Sala")]
public sealed class SalaInterfaceTests : TestFixture
{
    private AutenticacaoPageObject autenticacaoPage;

    [TestInitialize]
    public void InicializarTeste()
    {
        base.InicializarTeste();

        autenticacaoPage = new AutenticacaoPageObject(driver, enderecoBase!);

        autenticacaoPage.RegistrarContaEmpresarial();
    }

    [TestMethod]
    public void CT007_Deve_Cadastrar_Sala_Corretamente()
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/Sala/Cadastrar");

        var numeroInput = TimeHelper.EsperarElemento(By.Id("Numero"), driver, 10);
        numeroInput?.SendKeys("101");

        var capacidadeInput = TimeHelper.EsperarElemento(By.Id("Capacidade"), driver, 10);
        capacidadeInput?.SendKeys("50");

        var confirmarBtn = TimeHelper.EsperarElemento(By.CssSelector("button[type='submit']"), driver, 10);
        confirmarBtn?.Click();

        Assert.IsTrue(TimeHelper.EsperarElementoVisivel(By.XPath("//*[contains(text(),'101')]"), driver, 10));
    }

    [TestMethod]
    public void CT008_Deve_Editar_Sala_Corretamente()
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/Sala/Cadastrar");

        var numeroInput = TimeHelper.EsperarElemento(By.Id("Numero"), driver, 10);
        numeroInput?.SendKeys("102");

        var capacidadeInput = TimeHelper.EsperarElemento(By.Id("Capacidade"), driver, 10);
        capacidadeInput?.SendKeys("80");

        var confirmarBtn = TimeHelper.EsperarElemento(By.CssSelector("button[type='submit']"), driver, 10);
        confirmarBtn?.Click();

        var editarBtn = TimeHelper.EsperarElemento(By.CssSelector("a[data-se='editar-sala']"), driver, 10);
        editarBtn?.Click();

        var numeroEdit = TimeHelper.EsperarElemento(By.Id("Numero"), driver, 10);
        numeroEdit?.Clear();
        numeroEdit?.SendKeys("102-A");

        var capacidadeEdit = TimeHelper.EsperarElemento(By.Id("Capacidade"), driver, 10);
        capacidadeEdit?.Clear();
        capacidadeEdit?.SendKeys("90");

        var confirmarEdit = TimeHelper.EsperarElemento(By.CssSelector("button[type='submit']"), driver, 10);
        confirmarEdit?.Click();

        Assert.IsTrue(TimeHelper.EsperarElementoVisivel(By.XPath("//*[contains(text(),'102-A')]"), driver, 10));
    }

    [TestMethod]
    public void CT009_Deve_Excluir_Sala_Corretamente()
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/Sala/Cadastrar");

        var numeroInput = TimeHelper.EsperarElemento(By.Id("Numero"), driver, 10);
        numeroInput?.SendKeys("103");

        var capacidadeInput = TimeHelper.EsperarElemento(By.Id("Capacidade"), driver, 10);
        capacidadeInput?.SendKeys("70");

        var confirmarBtn = TimeHelper.EsperarElemento(By.CssSelector("button[type='submit']"), driver, 10);
        confirmarBtn?.Click();

        var excluirBtn = TimeHelper.EsperarElemento(By.CssSelector("a[data-se='excluir-sala']"), driver, 10);
        excluirBtn?.Click();

        var confirmarExclusao = TimeHelper.EsperarElemento(By.CssSelector("button[type='submit']"), driver, 10);
        confirmarExclusao?.Click();

        Assert.IsFalse(TimeHelper.EsperarElementoVisivel(By.XPath("//*[contains(text(),'103')]"), driver, 10));
    }

    [TestMethod]
    public void CT010_Deve_Listar_Todas_As_Salas()
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/Sala/Cadastrar");

        var numeroInput = TimeHelper.EsperarElemento(By.Id("Numero"), driver, 10);
        numeroInput?.SendKeys("104");

        var capacidadeInput = TimeHelper.EsperarElemento(By.Id("Capacidade"), driver, 10);
        capacidadeInput?.SendKeys("100");

        var confirmarBtn = TimeHelper.EsperarElemento(By.CssSelector("button[type='submit']"), driver, 10);
        confirmarBtn?.Click();

        // checa listagem
        Assert.IsTrue(TimeHelper.EsperarElementoVisivel(By.XPath("//*[contains(text(),'104')]"), driver, 10));
    }
}