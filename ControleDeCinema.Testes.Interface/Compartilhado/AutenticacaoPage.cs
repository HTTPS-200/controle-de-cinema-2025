//using OpenQA.Selenium;
//using OpenQA.Selenium.Support.UI;

//namespace ControleDeCinema.Testes.Interface.Compartilhado;

//public class AutenticacaoPage
//{
//    private readonly IWebDriver driver;
//    private readonly WebDriverWait wait;

//    protected static string enderecoBase;
//    protected const string emailCliente = "clienteTeste@gmail.com";
//    protected const string emailEmpresa = "empresaTeste@gmail.com";
//    protected const string senhaPadrao = "Teste123!";

//    public AutenticacaoPage(IWebDriver driver)
//    {
//        this.driver = driver;
//        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
//    }

//    protected static void RegistrarContaEmpresarial()
//    {
//        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/registro");

//        IWebElement inputEmail = driver.FindElement(By.CssSelector("input[data-se='inputEmail']"));
//        IWebElement inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));
//        IWebElement inputConfirmarSenha = driver.FindElement(By.CssSelector("input[data-se='inputConfirmarSenha']"));
//        SelectElement selectTipoUsuario = new(driver.FindElement(By.CssSelector("select[data-se='selectTipoUsuario']")));

//        inputEmail.Clear();
//        inputEmail.SendKeys(emailEmpresa);

//        inputSenha.Clear();
//        inputSenha.SendKeys(senhaPadrao);

//        inputConfirmarSenha.Clear();
//        inputConfirmarSenha.SendKeys(senhaPadrao);

//        selectTipoUsuario.SelectByText("Empresa");

//        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

//        wait.Until(d =>
//        {
//            IWebElement btn = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
//            if (!btn.Enabled || !btn.Displayed) return false;
//            btn.Click();
//            return true;
//        });

//        wait.Until(d =>
//            !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
//            d.FindElements(By.CssSelector("form[action='/autenticacao/registro']")).Count == 0
//        );

//        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
//    }



//    protected static void RegistrarContaCliente()
//    {
//        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/registro");

//        IWebElement inputEmail = driver.FindElement(By.CssSelector("input[data-se='inputEmail']"));
//        IWebElement inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));
//        IWebElement inputConfirmarSenha = driver.FindElement(By.CssSelector("input[data-se='inputConfirmarSenha']"));
//        SelectElement selectTipoUsuario = new(driver.FindElement(By.CssSelector("select[data-se='selectTipoUsuario']")));

//        inputEmail.Clear();
//        inputEmail.SendKeys(emailCliente);

//        inputSenha.Clear();
//        inputSenha.SendKeys(senhaPadrao);

//        inputConfirmarSenha.Clear();
//        inputConfirmarSenha.SendKeys(senhaPadrao);

//        selectTipoUsuario.SelectByText("Cliente");

//        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

//        wait.Until(d =>
//        {
//            IWebElement btn = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
//            if (!btn.Enabled || !btn.Displayed) return false;
//            btn.Click();
//            return true;
//        });

//        wait.Until(d =>
//            !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
//            d.FindElements(By.CssSelector("form[action='/autenticacao/registro']")).Count == 0
//        );

//        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
//    }

//    protected static void FazerLogin(string tipoConta)
//    {
//        driver.Navigate().GoToUrl($"{enderecoBase}/autenticacao/login");

//        IWebElement inputEmail = driver.FindElement(By.CssSelector("input[data-se='inputEmail']"));
//        IWebElement inputSenha = driver.FindElement(By.CssSelector("input[data-se='inputSenha']"));

//        inputEmail.Clear();
//        if (tipoConta == "Cliente")
//            inputEmail.SendKeys(emailCliente);
//        else if (tipoConta == "Empresa")
//            inputEmail.SendKeys(emailEmpresa);


//        inputSenha.Clear();
//        inputSenha.SendKeys(senhaPadrao);

//        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(20));

//        wait.Until(d =>
//        {
//            IWebElement btn = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
//            if (!btn.Enabled || !btn.Displayed) return false;
//            btn.Click();
//            return true;
//        });

//        wait.Until(d =>
//            !d.Url.Contains("/autenticacao/registro", StringComparison.OrdinalIgnoreCase) &&
//            d.FindElements(By.CssSelector("form[action='/autenticacao/registro']")).Count == 0
//        );

//        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
//    }
//    protected static void FazerLogout()
//    {
//        driver.Navigate().GoToUrl($"{enderecoBase}");

//        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(5));

//        wait.Until(d => d.FindElements(By.CssSelector("form[action='/autenticacao/logout']")).Count > 0);
//        wait.Until(d => d.FindElement(By.CssSelector("form[action='/autenticacao/logout']"))).Submit();
//    }
//}