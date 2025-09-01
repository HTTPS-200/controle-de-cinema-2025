using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Testes.Interface.Compartilhado;
using OpenQA.Selenium;

namespace ControleDeCinema.Testes.Interface.ModuloAutenticacao;

[TestClass]
[TestCategory("Testes de Interface de Autenticação")]
public sealed class AutenticacaoInterfaceTests : TestFixture
{
    private AutenticacaoPageObject autenticacaoPage;

    [TestMethod]
    public void CT033_Deve_Cadastrar_Empresa_Com_Sucesso()
    {
        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
        autenticacaoPage.RegistrarContaEmpresarial();
        Assert.IsTrue(driver!.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    }

    [TestMethod]
    public void CT034_Deve_Cadastrar_Cliente_Com_Sucesso()
    {
        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
        autenticacaoPage.RegistrarContaCliente();
        Assert.IsTrue(driver!.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    }

    [TestMethod]
    public void CT035_Deve_Realizar_Login_Com_Credenciais_Validas()
    {
        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
        autenticacaoPage.RegistrarContaEmpresarial();

        driver.Manage().Cookies.DeleteAllCookies();
        driver.Navigate().GoToUrl("about:blank");

        autenticacaoPage.FazerLogin("Empresa");     
        Assert.IsTrue(driver!.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    }

    [TestMethod]
    public void CT036_Nao_Deve_Realizar_Login_Com_Credenciais_Invalidas()
    {   
        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
        autenticacaoPage.RegistrarContaCliente();


        autenticacaoPage.FazerLogin("Empresa");
        Assert.IsTrue(driver!.FindElements(By.CssSelector("div[class*='alert alert-danger']")).Count > 0);
    }

    [TestMethod]
    public void CT037_Deve_Realizar_Logout_Corretamente()
    {
        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
        autenticacaoPage.RegistrarContaCliente();
        autenticacaoPage.FazerLogin("Empresa");
        autenticacaoPage.FazerLogout();
        Assert.IsTrue(driver!.FindElements(By.CssSelector("form[action='/autenticacao/login']")).Count > 0);
    }
}
