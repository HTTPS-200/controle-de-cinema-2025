using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

public class GeneroFilmeIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public GeneroFilmeIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
    }

    public GeneroFilmeIndexPageObject IrPara(string enderecoBase)
    {
        try
        {
            driver.Navigate().GoToUrl($"{enderecoBase.TrimEnd('/')}/generos");
            wait.Until(d => d.Url.Contains("/generos", StringComparison.OrdinalIgnoreCase));
            wait.Until(d => d.FindElement(By.TagName("body")).Displayed);
            wait.Until(d => d.FindElements(
                By.CssSelector("a[data-se='btnCadastrar'], .card, h1, h2, h3")
            ).Any(e => e.Displayed));

            return this;
        }
        catch (WebDriverTimeoutException ex)
        {
            Console.WriteLine($"Timeout ao carregar página de gêneros. URL: {enderecoBase}/generos");
            Console.WriteLine($"Página atual: {driver.Url}");
            Console.WriteLine($"Page source: {driver.PageSource}");
            throw;
        }
    }

    public GeneroFilmeFormPageObject ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();
        return new(driver);
    }

    public GeneroFilmeFormPageObject ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnEditar']"))).Click();
        return new(driver);
    }

    public GeneroFilmeFormPageObject ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnExcluir']"))).Click();
        return new(driver);
    }

    public bool ContemGenero(string descricao)
    {
        return driver.PageSource.Contains(descricao);
    }
}
