using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TesteFacil.Testes.Interface.ModuloSala;

public class SalaFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public SalaFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        try
        {
            wait.Until(d =>
                d.FindElement(By.CssSelector(cssSelectorToFind: "form[data-se='form']")).Displayed);
        }
        catch (WebDriverTimeoutException)
        {
            DumpOnFailure(driver, "sala-timeout");
            throw;
        }
    }

    public SalaFormPageObject PreencherNumero(string numero)
    {
        var input = wait.Until(d =>
        {
            var element = d.FindElement(By.CssSelector("input[data-se='inputNumero']"));
            return element.Displayed && element.Enabled ? element : null;
        });

        input.Clear();
        input.SendKeys(numero);
        return this;
    }

    public SalaFormPageObject PreencherCapacidade(string capacidade)
    {
        var input = wait.Until(d =>
        {
            var element = d.FindElement(By.CssSelector("input[data-se='inputCapacidade']"));
            return element.Displayed && element.Enabled ? element : null;
        });

        input.Clear();
        input.SendKeys(capacidade);
        return this;
    }

    public SalaIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();
        wait.Until(d => d.Url.Contains("/salas", StringComparison.OrdinalIgnoreCase));
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return new(driver);
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
