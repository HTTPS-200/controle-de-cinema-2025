using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloFilme
{
    public class FilmeIndexPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private readonly string baseUrl;

        public FilmeIndexPageObject(IWebDriver driver, string baseUrl)
        {
            this.driver = driver;
            this.baseUrl = baseUrl;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        public FilmeIndexPageObject IrPara(string enderecoBase)
        {
            driver.Navigate().GoToUrl($"{enderecoBase.TrimEnd('/')}/filmes");
            wait.Until(d => d.Url.Contains("/filmes", StringComparison.OrdinalIgnoreCase));
            wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);
            return this;
        }

        public FilmeFormPageObject ClickCadastrar()
        {
            driver.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Click();
            return new FilmeFormPageObject(driver);
        }

        public FilmeFormPageObject ClickEditar()
        {
            driver.FindElement(By.CssSelector("a[data-se='btnEditar']")).Click();
            return new FilmeFormPageObject(driver);
        }

        public FilmeFormPageObject ClickExcluir()
        {
            driver.FindElement(By.CssSelector("a[data-se='btnExcluir']")).Click();
            return new FilmeFormPageObject(driver);
        }

        public bool ContemFilme(string titulo)
        {
            return driver.PageSource.Contains(titulo);
        }
    }
}
