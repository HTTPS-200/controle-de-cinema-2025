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
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
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
            wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

            return new(driver);
        }

        public FilmeFormPageObject ClickEditar()
        {
            wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnEditar']"))).Click();

            return new(driver);
        }

        public FilmeFormPageObject ClickExcluir()
        {
            wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnExcluir']"))).Click();

            return new(driver);
        }

        public bool ContemFilme(string titulo)
        {
            return driver.PageSource.Contains(titulo);
        }
    }
}