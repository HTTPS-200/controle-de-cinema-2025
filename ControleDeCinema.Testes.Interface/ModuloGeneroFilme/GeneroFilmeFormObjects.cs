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
        wait.Until(d => d.FindElement(By.CssSelector("form[data-se='form']")).Displayed);
    }

    public GeneroFilmeFormObjects PreencherNome(string nome)
    {
        var campoNome = wait.Until(d => d.FindElement(By.CssSelector("input[data-se='inputDescricao']")));
        campoNome.Clear();
        campoNome.SendKeys(nome);
        return this;
    }

    public GeneroFilmeIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();
        wait.Until(d => d.Url.Contains("/generos", StringComparison.OrdinalIgnoreCase));
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return new(driver);
    }
}
