using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ControleDeCinema.Testes.Interface.Helpers; 

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

        TimeHelper.EsperarElemento(By.XPath("//*[contains(text(), 'Registro')]"), driver, 10);

        var inputEmail = TimeHelper.EsperarElemento(By.CssSelector("input[data-se='inputEmail']"), driver, 10);
        inputEmail?.Clear();
        inputEmail?.SendKeys(emailEmpresa);

        var inputSenha = TimeHelper.EsperarElemento(By.CssSelector("input[data-se='inputSenha']"), driver, 10);
        inputSenha?.Clear();
        inputSenha?.SendKeys(senhaPadrao);

        var inputConfirmarSenha = TimeHelper.EsperarElemento(By.CssSelector("input[data-se='inputConfirmarSenha']"), driver, 10);
        inputConfirmarSenha?.Clear();
        inputConfirmarSenha?.SendKeys(senhaPadrao);

        var selectElement = TimeHelper.EsperarElemento(By.CssSelector("select[data-se='selectTipoUsuario']"), driver, 10);
        var selectTipoUsuario = new SelectElement(selectElement);
        selectTipoUsuario.SelectByText("Empresa");

        ConfirmarFormulario();
    }
    public void RegistrarContaCliente()
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/registro");

        var inputEmail = TimeHelper.EsperarElemento(By.CssSelector("input[data-se='inputEmail']"), driver, 10);
        inputEmail?.Clear();
        inputEmail?.SendKeys(emailCliente);

        var inputSenha = TimeHelper.EsperarElemento(By.CssSelector("input[data-se='inputSenha']"), driver, 10);
        inputSenha?.Clear();
        inputSenha?.SendKeys(senhaPadrao);

        var inputConfirmarSenha = TimeHelper.EsperarElemento(By.CssSelector("input[data-se='inputConfirmarSenha']"), driver, 10);
        inputConfirmarSenha?.Clear();
        inputConfirmarSenha?.SendKeys(senhaPadrao);

        var selectElement = TimeHelper.EsperarElemento(By.CssSelector("select[data-se='selectTipoUsuario']"), driver, 10);
        var selectTipoUsuario = new SelectElement(selectElement);
        selectTipoUsuario.SelectByText("Cliente");

        ConfirmarFormulario();
    }

    public void FazerLogin(string tipoConta)
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/login");

        var inputEmail = TimeHelper.EsperarElemento(By.CssSelector("input[data-se='inputEmail']"), driver, 10);
        inputEmail?.Clear();
        inputEmail?.SendKeys(tipoConta == "Cliente" ? emailCliente : emailEmpresa);

        var inputSenha = TimeHelper.EsperarElemento(By.CssSelector("input[data-se='inputSenha']"), driver, 10);
        inputSenha?.Clear();
        inputSenha?.SendKeys(senhaPadrao);

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
