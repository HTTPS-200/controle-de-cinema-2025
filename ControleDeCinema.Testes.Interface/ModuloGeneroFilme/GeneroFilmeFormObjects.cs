using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
public class GeneroFilmeFormObjects
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public GeneroFilmeFormObjects(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.CssSelector("form")).Displayed);
    }

    public GeneroFilmeFormObjects PreencherNome(string nome)
    {
        var inputNome = driver.FindElement(By.Id("nome"));
        inputNome.Clear();
        inputNome.SendKeys(nome);
        return this;
    }

    public GeneroFilmeIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);
        return new GeneroFilmeIndexPageObject(driver);

    }
}
