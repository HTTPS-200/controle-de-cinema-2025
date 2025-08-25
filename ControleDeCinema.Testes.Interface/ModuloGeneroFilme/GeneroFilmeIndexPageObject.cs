using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
internal class GeneroFilmeIndexPageObject
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
        driver.Navigate().GoToUrl($"{enderecoBase}/GeneroFilme");
        return this;
    }
}