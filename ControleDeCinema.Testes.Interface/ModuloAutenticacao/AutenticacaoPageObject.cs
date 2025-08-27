using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.Compartilhado;

public class AutenticacaoPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;
    private readonly string enderecoBase;

    private const string emailCliente = "clienteTeste@gmail.com";
    private const string emailEmpresa = "empresaTeste@gmail.com";
    private const string senhaPadrao = "Teste123!";

    public AutenticacaoPageObject(IWebDriver driver, string enderecoBase)
    {
        this.driver = driver;
        this.enderecoBase = enderecoBase;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
    }

    public void RegistrarContaEmpresarial()
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/registro");

        var inputEmail = driver.FindElement(By.CssSelector("input[data-se='inputEmail']"));
        var inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));
        var inputConfirmarSenha = driver.FindElement(By.CssSelector("input[data-se='inputConfirmarSenha']"));
        var selectTipoUsuario = new SelectElement(driver.FindElement(By.CssSelector("select[data-se='selectTipoUsuario']")));

        inputEmail.Clear();
        inputEmail.SendKeys(emailEmpresa);

        inputSenha.Clear();
        inputSenha.SendKeys(senhaPadrao);

        inputConfirmarSenha.Clear();
        inputConfirmarSenha.SendKeys(senhaPadrao);

        selectTipoUsuario.SelectByText("Empresa");

        ConfirmarFormulario();
    }

    public void RegistrarContaCliente()
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/registro");

        var inputEmail = driver.FindElement(By.CssSelector("input[data-se='inputEmail']"));
        var inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));
        var inputConfirmarSenha = driver.FindElement(By.CssSelector("input[data-se='inputConfirmarSenha']"));
        var selectTipoUsuario = new SelectElement(driver.FindElement(By.CssSelector("select[data-se='selectTipoUsuario']")));

        inputEmail.Clear();
        inputEmail.SendKeys(emailCliente);

        inputSenha.Clear();
        inputSenha.SendKeys(senhaPadrao);

        inputConfirmarSenha.Clear();
        inputConfirmarSenha.SendKeys(senhaPadrao);

        selectTipoUsuario.SelectByText("Cliente");

        ConfirmarFormulario();
    }

    public void FazerLogin(string tipoConta)
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/login");

        var inputEmail = driver.FindElement(By.CssSelector("input[data-se='inputEmail']"));
        var inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));

        inputEmail.Clear();
        inputEmail.SendKeys(tipoConta == "Cliente" ? emailCliente : emailEmpresa);

        inputSenha.Clear();
        inputSenha.SendKeys(senhaPadrao);

        ConfirmarFormulario();
    }

    public void FazerLogout()
    {
        driver.Navigate().GoToUrl(enderecoBase);

        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
        wait.Until(d => d.FindElement(By.CssSelector("form[action='/autenticacao/logout']"))).Submit();
    }

    private void ConfirmarFormulario()
    {
        wait.Until(d =>
        {
            var btn = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
            if (!btn.Enabled || !btn.Displayed) return false;
            btn.Click();
            return true;
        });

        wait.Until(d =>
            !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
            !d.Url.Contains("/autenticacao/login", StringComparison.OrdinalIgnoreCase)
        );

        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    }
}
