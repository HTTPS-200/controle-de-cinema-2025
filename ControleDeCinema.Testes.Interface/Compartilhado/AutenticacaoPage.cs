using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.Compartilhado;

public class AutenticacaoPage
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public const string EmailEmpresa = "empresateste@gmail.com";
    public const string SenhaEmpresa = "Teste123!";

    public AutenticacaoPage(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
    }

    public void RegistrarContaEmpresarial(string enderecoBase)
    {
        driver.Navigate().GoToUrl($"{enderecoBase.TrimEnd('/')}/autenticacao/registro");

        IWebElement inputEmail = wait.Until(d => d.FindElement(By.CssSelector("input[data-se='inputEmail']")));
        IWebElement inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));
        IWebElement inputConfirmarSenha = driver.FindElement(By.CssSelector("input[data-se='inputConfirmarSenha']"));
        SelectElement selectTipoUsuario = new(driver.FindElement(By.CssSelector("select[data-se='selectTipoUsuario']")));

        inputEmail.Clear();
        inputEmail.SendKeys(EmailEmpresa);

        inputSenha.Clear();
        inputSenha.SendKeys(SenhaEmpresa);

        inputConfirmarSenha.Clear();
        inputConfirmarSenha.SendKeys(SenhaEmpresa);

        selectTipoUsuario.SelectByText("Empresa");

        IWebElement btnConfirmar = wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']")));
        btnConfirmar.Click();

        wait.Until(d =>
            !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
            d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0
        );
    }

    public void Login(string enderecoBase, string email, string senha)
    {
        driver.Navigate().GoToUrl($"{enderecoBase.TrimEnd('/')}/autenticacao/login");

        IWebElement inputEmail = wait.Until(d => d.FindElement(By.CssSelector("input[name='Email']")));
        IWebElement inputSenha = driver.FindElement(By.CssSelector("input[name='Senha']"));
        IWebElement btnLogin = driver.FindElement(By.CssSelector("button[type='submit']"));

        inputEmail.Clear();
        inputEmail.SendKeys(email);

        inputSenha.Clear();
        inputSenha.SendKeys(senha);

        btnLogin.Click();

        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
    }
}
