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

        IWebElement inputEmail = driver.FindElement(By.CssSelector("input[data-se='inputEmail']"));
        IWebElement inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));
        IWebElement inputConfirmarSenha = driver.FindElement(By.CssSelector("input[data-se='inputConfirmarSenha']"));
        SelectElement selectTipoUsuario = new(driver.FindElement(By.CssSelector("select[data-se='selectTipoUsuario']")));

        inputEmail.Clear();
        inputEmail.SendKeys(emailEmpresa);

        inputSenha.Clear();
        inputSenha.SendKeys(senhaPadrao);

        inputConfirmarSenha.Clear();
        inputConfirmarSenha.SendKeys(senhaPadrao);

        selectTipoUsuario.SelectByText("Empresa");

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

        wait.Until(d =>
        {
            IWebElement btn = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
            if (!btn.Enabled || !btn.Displayed) return false;
            btn.Click();
            return true;
        });

        wait.Until(d =>
            !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
            d.FindElements(By.CssSelector("form[action='/autenticacao/registro']")).Count == 0
        );

        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    }
    public void RegistrarContaCliente()
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/registro");

        IWebElement inputEmail = driver.FindElement(By.CssSelector("input[data-se='inputEmail']"));
        IWebElement inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));
        IWebElement inputConfirmarSenha = driver.FindElement(By.CssSelector("input[data-se='inputConfirmarSenha']"));
        SelectElement selectTipoUsuario = new(driver.FindElement(By.CssSelector("select[data-se='selectTipoUsuario']")));

        inputEmail.Clear();
        inputEmail.SendKeys(emailCliente);

        inputSenha.Clear();
        inputSenha.SendKeys(senhaPadrao);

        inputConfirmarSenha.Clear();
        inputConfirmarSenha.SendKeys(senhaPadrao);

        selectTipoUsuario.SelectByText("Cliente");

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

        wait.Until(d =>
        {
            IWebElement btn = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
            if (!btn.Enabled || !btn.Displayed) return false;
            btn.Click();
            return true;
        });

        wait.Until(d =>
            !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
            d.FindElements(By.CssSelector("form[action='/autenticacao/registro']")).Count == 0
        );

        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    }

    public void FazerLogin(string tipoConta)
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/login");

        IWebElement inputEmail = driver.FindElement(By.CssSelector("input[data-se='inputEmail']"));
        IWebElement inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));

        inputEmail.Clear();
        if (tipoConta == "Cliente")
            inputEmail.SendKeys(emailCliente);
        else if (tipoConta == "Empresa")
            inputEmail.SendKeys(emailEmpresa);


        inputSenha.Clear();
        inputSenha.SendKeys(senhaPadrao);

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

        wait.Until(d =>
        {
            IWebElement btn = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
            if (!btn.Enabled || !btn.Displayed) return false;
            btn.Click();
            return true;
        });

        wait.Until(d =>
            !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
            d.FindElements(By.CssSelector("form[action='/autenticacao/registro']")).Count == 0
        );

        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    }

    public void FazerLogout()
    {
        driver.Navigate().GoToUrl($"{enderecoBase}");

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(5));

        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
        wait.Until(d => d.FindElement(By.CssSelector("form[action='/autenticacao/logout']"))).Submit();
    }

    //private void ConfirmarFormulario()
    //{
    //    wait.Until(d =>
    //    {
    //        var btn = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
    //        if (!btn.Enabled || !btn.Displayed) return false;
    //        btn.Click();
    //        return true;
    //    });

    //    wait.Until(d =>
    //        !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
    //        !d.Url.Contains("/autenticacao/login", StringComparison.OrdinalIgnoreCase)
    //    );

    //    wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    //}
}
