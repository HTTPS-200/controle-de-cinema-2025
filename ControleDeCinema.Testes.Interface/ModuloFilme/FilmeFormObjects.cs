using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;
public class FilmeFormObjects
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public FilmeFormObjects(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.CssSelector("form[data-se='formPrincipal']")).Displayed);
    }

    public FilmeFormObjects PreencherNome(string nome)
    {
        var campoNome = wait.Until(d => d.FindElement(By.CssSelector("input[data-se='txtNome']")));
        campoNome.Clear();
        campoNome.SendKeys(nome);
        return this;
    }

    public FilmeIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();
        return new FilmeIndexPageObject(driver);
    }

}
