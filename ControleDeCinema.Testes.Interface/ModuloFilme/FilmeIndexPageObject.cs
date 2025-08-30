using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;

public class FilmeIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public FilmeIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public FilmeIndexPageObject IrPara(string enderecoBase)
    {
        driver.Navigate().GoToUrl($"{enderecoBase}/filmes");
        return this;
    }

    public FilmeFormObjects ClickCadastrar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();
        return new FilmeFormObjects(driver);
    }

    public FilmeFormObjects ClickEditar()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Edição']"))).Click();
        return new FilmeFormObjects(driver);
    }

    public FilmeFormObjects ClickExcluir()
    {
        wait.Until(d => d.FindElement(By.CssSelector(".card a[title='Exclusão']"))).Click();
        return new FilmeFormObjects(driver);
    }

    public bool ContemFilme(string nome)
    {
        wait.Until(d => d.FindElement(By.CssSelector("[data-se='contemFilme']")).Displayed);
        return driver.PageSource.Contains(nome);
    }
}