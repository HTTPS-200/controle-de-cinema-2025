using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TesteFacil.Testes.Interface.ModuloSala;

public class SalaIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public SalaIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
    }

    public SalaIndexPageObject IrPara(string enderecoBase)
    {
        driver.Navigate().GoToUrl($"{enderecoBase.TrimEnd('/')}/salas");
        wait.Until(d => d.Url.Contains("/salas", StringComparison.OrdinalIgnoreCase));
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return this;
    }

    public SalaFormPageObject ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

        return new(driver);
    }

    public SalaFormPageObject ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnEditar']"))).Click();
        return new(driver);
    }

    public SalaFormPageObject ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnExcluir']"))).Click();
        return new(driver);
    }

    public bool ContemSala(string numero)
    {
        return driver.PageSource.Contains($"# {numero}");
    }
}