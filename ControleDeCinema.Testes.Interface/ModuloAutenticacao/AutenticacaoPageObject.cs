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
        try
        {
            // Navega para a página inicial onde a navbar com logout está disponível
            driver.Navigate().GoToUrl($"{enderecoBase}");

            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));

            // Espera a navbar carregar
            wait.Until(d => d.FindElements(By.CssSelector(".navbar")).Count > 0);

            // Procura pelo dropdown de usuário - busca por ícone ou texto genérico
            var dropdownButton = driver.FindElements(By.CssSelector(".dropdown-toggle"))
                .FirstOrDefault(btn => btn.Text.Contains("person-circle", StringComparison.OrdinalIgnoreCase) ||
                                      btn.FindElements(By.CssSelector(".bi-person-circle")).Count > 0);

            if (dropdownButton != null)
            {
                // Clica no dropdown para abrir o menu
                dropdownButton.Click();

                // Pequeno delay para o menu abrir
                Thread.Sleep(500);

                // Procura pelo formulário de logout dentro do dropdown
                var logoutForm = driver.FindElements(By.CssSelector(".dropdown-menu form[action*='logout']"))
                    .FirstOrDefault();

                if (logoutForm != null)
                {
                    // Submete o formulário de logout
                    logoutForm.Submit();

                    // Espera o redirecionamento para a página de login
                    wait.Until(d => d.Url.Contains("/autenticacao/login", StringComparison.OrdinalIgnoreCase) ||
                                   d.FindElements(By.CssSelector("form[data-se='form']")).Count > 0);
                }
                else
                {
                    // Fallback: navega diretamente para a URL de logout
                    driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/logout");
                }
            }
            else
            {
                // Fallback: navega diretamente para a URL de logout
                driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/logout");
            }

            // Espera garantir que o logout foi efetuado
            wait.Until(d => d.FindElements(By.CssSelector("form[action*='/autenticacao/logout']")).Count == 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro durante logout: {ex.Message}");
            // Fallback final: limpa cookies para garantir logout
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/login");
        }
    }

        public bool EstaLogado()
    {
        try
        {
            driver.Navigate().GoToUrl($"{enderecoBase}");
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(5));

            // Verifica se existe o dropdown de usuário (indicando que está logado)
            return driver.FindElements(By.CssSelector(".dropdown-toggle"))
                .Any(btn => btn.FindElements(By.CssSelector(".bi-person-circle")).Count > 0 ||
                           (btn.Text.Contains("@") && btn.Text.Contains(".")));
        }
        catch
        {
            return false;
        }
    }
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

