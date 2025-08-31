using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

public class GeneroFilmeFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public GeneroFilmeFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        try
        {
            wait.Until(d => d.FindElement(By.CssSelector("form[data-se='form']")).Displayed);
        }
        catch (WebDriverTimeoutException)
        {
            DumpOnFailure(driver, "genero-timeout");
            throw;
        }
    }

    public GeneroFilmeFormPageObject PreencherDescricao(string descricao)
    {
        wait.Until(d =>
           d.FindElement(By.CssSelector("input[data-se='inputDescricao']")).Displayed &&
           d.FindElement(By.CssSelector("input[data-se='inputDescricao']")).Enabled
       );

        IWebElement inputDescricao = driver.FindElement(By.CssSelector("input[data-se='inputDescricao']"));
        inputDescricao.Clear();
        inputDescricao.SendKeys(descricao);

        return this;
    }

    public GeneroFilmeIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();
        wait.Until(d => d.Url.Contains("/generos", StringComparison.OrdinalIgnoreCase));
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return new(driver);
    }

    public GeneroFilmeIndexPageObject ClickConfirmarExcluir(string descricao)
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();

        wait.Until(d => d.Url.Contains("/generos", StringComparison.OrdinalIgnoreCase));

        wait.Until(d => !d.PageSource.Contains(descricao));

        return new(driver);
    }

    public GeneroFilmeIndexPageObject ConfirmarComSucesso()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();
        wait.Until(d => d.Url.Contains("/generos", StringComparison.OrdinalIgnoreCase));
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return new(driver);
    }

    public GeneroFilmeFormPageObject ConfirmarComErro()
    {
        wait.Until(d => d.FindElement(By.CssSelector("[data-se='btnConfirmar']"))).Click();

        wait.Until(d => d.FindElement(By.CssSelector(
            ".field-validation-error, .text-danger, .alert-danger"
        )).Displayed);

        return this;
    }

    public string ObterMensagemErro()
    {
        var elemento = driver.FindElement(By.CssSelector(
         ".field-validation-error, .text-danger, .alert-danger"
     ));
        return elemento.Text;
    }

    private static void DumpOnFailure(IWebDriver driver, string prefix)
    {
        try
        {
            Screenshot shot = ((ITakesScreenshot)driver).GetScreenshot();
            string png = Path.Combine(Path.GetTempPath(), $"{prefix}-{DateTime.Now:HHmmss}.png");
            shot.SaveAsFile(png);

            string html = Path.Combine(Path.GetTempPath(), $"{prefix}-{DateTime.Now:HHmmss}.html");
            File.WriteAllText(html, driver.PageSource);
        }
        catch { /* best-effort */ }
    }
}
