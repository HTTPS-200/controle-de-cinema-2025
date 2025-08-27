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
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        wait.Until(d => d.FindElement(By.CssSelector("form[data-se='form']")).Displayed);
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
        var button = wait.Until(d =>
        {
            var element = d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"));
            return element.Displayed && element.Enabled ? element : null;
        });

        button.Click();

        // Wait for redirect to index page
        wait.Until(d => d.Url.Contains("/salas", StringComparison.OrdinalIgnoreCase));
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return new SalaIndexPageObject(driver);
    }
}