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
}
