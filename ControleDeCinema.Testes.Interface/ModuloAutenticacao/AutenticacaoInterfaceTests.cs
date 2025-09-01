using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Testes.Interface.Compartilhado;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloAutenticacao;

[TestClass]
[TestCategory("Testes de Interface de Autenticação")]
public sealed class AutenticacaoInterfaceTests : TestFixture
{
    private AutenticacaoPageObject autenticacaoPage;

    [TestInitialize]
    public void InicializarTeste()
    {
        base.InicializarTeste();
        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
    }


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

        driver.Manage().Cookies.DeleteAllCookies();
        driver.Navigate().GoToUrl("about:blank");

        string emailInvalido = "email_invalido_" + Guid.NewGuid() + "@teste.com";
        string senhaInvalida = "senhaErrada123";

        autenticacaoPage.FazerLoginComCredenciais(emailInvalido, senhaInvalida);

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
        bool paginaAtual = wait.Until(d =>
            d.Url.Contains("/autenticacao/login", StringComparison.OrdinalIgnoreCase) &&
            d.FindElements(By.CssSelector("form[action='/autenticacao/login']")).Count > 0
        );

        Assert.IsTrue(paginaAtual, "O sistema permitiu login com credenciais inválidas.");
    }

    [TestMethod]
    public void CT037_Deve_Realizar_Logout_Corretamente()
    {
        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
        autenticacaoPage.RegistrarContaCliente();

        driver.Manage().Cookies.DeleteAllCookies();
        driver.Navigate().GoToUrl("about:blank");

        autenticacaoPage.FazerLogin("Cliente");

        autenticacaoPage.FazerLogout();
        
        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
        bool paginaAtual = wait.Until(d =>
            d.Url.Contains("/autenticacao/login", StringComparison.OrdinalIgnoreCase) &&
            d.FindElements(By.CssSelector("form[action='/autenticacao/login']")).Count > 0);
        
        Assert.IsTrue(paginaAtual, "O sistema não retornou para a página de login após o logout.");
    }
}
