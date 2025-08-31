using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ControleDeCinema.Testes.Interface.ModuloFilme
{
    public class FilmeFormPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public FilmeFormPageObject(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public FilmeFormPageObject PreencherTitulo(string titulo)
        {
            var input = driver.FindElement(By.CssSelector("[data-se='inputTitulo']"));
            input.Clear();
            input.SendKeys(titulo);
            return this;
        }

        public FilmeFormPageObject PreencherDuracao(int duracao)
        {
            var input = driver.FindElement(By.CssSelector("[data-se='inputDuracao']"));
            input.Clear();
            input.SendKeys(duracao.ToString());
            return this;
        }

        public FilmeFormPageObject MarcarLancamento(bool lancamento = true)
        {
            var checkbox = driver.FindElement(By.CssSelector("[data-se='checkboxLancamento']"));
            if (checkbox.Selected != lancamento)
                checkbox.Click();
            return this;
        }

        public FilmeFormPageObject SelecionarGenero(string genero)
        {
            wait.Until(d =>
            d.FindElement(By.CssSelector("select[data-se='selectGenero']")).Displayed &&
            d.FindElement(By.CssSelector("select[data-se='selectGenero']")).Enabled
        );

            SelectElement selectGenero = new(driver.FindElement(By.CssSelector("select[data-se='selectGenero']")));

            wait.Until(_ => selectGenero.Options.Any(o => o.Text == genero));

            selectGenero.SelectByText(genero);

            return this;
        }

        public void Confirmar()
        {
            driver.FindElement(By.CssSelector("[data-se='btnConfirmar']")).Click();
        }

        public string ObterMensagemErro()
        {
            return driver.FindElement(By.CssSelector(".text-danger")).Text;
        }

        public FilmeFormPageObject ConfirmarComErro()
        {
            Confirmar();
            wait.Until(d => d.FindElement(By.CssSelector(".text-danger")).Displayed);
            return this;
        }

        public FilmeFormPageObject ConfirmarComSucesso()
        {
            Confirmar();
            wait.Until(d => d.Url.Contains("/filmes", StringComparison.OrdinalIgnoreCase));
            return this;
        }
    }
}
