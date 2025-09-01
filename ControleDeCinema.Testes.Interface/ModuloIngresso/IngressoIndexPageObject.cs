using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloIngresso;

public class IngressoIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public IngressoIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));
    }

    public IngressoIndexPageObject IrPara(string enderecoBase)
    {
        driver.Navigate().GoToUrl($"{enderecoBase.TrimEnd('/')}/sessoes");

        wait.Until(d => d.Url.Contains("/sessoes", StringComparison.OrdinalIgnoreCase));

        wait.Until(d => d.FindElements(By.CssSelector("a[data-se='btnComprarIngresso']")).Any());

        return this;
    }

    public IngressoFormPageObject ClickComprarIngresso()
    {
        var botao = wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnComprarIngresso']")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", botao); 
        botao.Click();
        return new IngressoFormPageObject(driver);
    }

    public bool ContemSessao(string filme, string sala)
    {
        return driver.PageSource.Contains(filme) && driver.PageSource.Contains(sala);
    }

    public bool ContemMensagem(string mensagem)
    {
        return driver.PageSource.IndexOf(mensagem, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
