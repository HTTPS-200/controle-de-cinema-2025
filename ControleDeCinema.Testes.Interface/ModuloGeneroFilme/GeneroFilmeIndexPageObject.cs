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
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public GeneroFilmeIndexPageObject IrPara(string enderecoBase)
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/generos");
        return this;
    }

    public GeneroFilmeFormObjects ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();
        return new GeneroFilmeFormObjects(driver);
    }

    public GeneroFilmeFormObjects ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Edição']"))).Click();
        return new GeneroFilmeFormObjects(driver);
    }

    public GeneroFilmeFormObjects ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Exclusão']"))).Click();
        return new GeneroFilmeFormObjects(driver);
    }

    public bool ContemGenero(string nome)
    {
        wait.Until(d => d.FindElement(By.CssSelector("[data-se='contemGenero']")).Displayed);
        return driver.PageSource.Contains(nome);
    }
}