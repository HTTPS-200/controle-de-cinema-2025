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
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20)); // aumentei para 20s
        }

        public FilmeFormPageObject PreencherTitulo(string titulo)
        {
            var input = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-se='inputTitulo']")));
            input.Clear();
            input.SendKeys(titulo);
            return this;
        }

        public FilmeFormPageObject PreencherDuracao(int duracao)
        {
            var input = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-se='inputDuracao']")));
            input.Clear();
            input.SendKeys(duracao.ToString());
            return this;
        }

        public FilmeFormPageObject MarcarLancamento(bool lancamento = true)
        {
            var checkbox = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-se='checkboxLancamento']")));
            if (checkbox.Selected != lancamento)
                checkbox.Click();
            return this;
        }

        public FilmeFormPageObject SelecionarGenero(string descricao)
        {
            IWebElement selectElement = wait.Until(d =>
            {
                var elem = d.FindElement(By.CssSelector("select[data-se='selectGenero']"));
                if (!elem.Displayed || !elem.Enabled) return null;
                var select = new SelectElement(elem);
                return select.Options.Any(o => o.Text == descricao) ? elem : null;
            });

            var selectFinal = new SelectElement(selectElement);
            selectFinal.SelectByText(descricao);

            return this;
        }

        public FilmeFormPageObject Confirmar()
        {
            var btn = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-se='btnConfirmar']")));
            btn.Click();
            return this;
        }

        public FilmeFormPageObject ConfirmarComSucesso()
        {
            Confirmar();
            wait.Until(d => d.Url.Contains("/filmes", StringComparison.OrdinalIgnoreCase));
            return this;
        }

        public FilmeFormPageObject ConfirmarComErro()
        {
            Confirmar();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(40));
            wait.Until(d =>
                d.FindElements(By.CssSelector(".text-danger, .alert-danger, .toast-error"))
                 .Any(e => e.Displayed)
            );

            return this;
        }

        public string ObterMensagemErro()
        {
            var elemento = driver.FindElements(By.CssSelector(".text-danger, .alert-danger, .toast-error"))
                                .FirstOrDefault(e => e.Displayed);
            return elemento?.Text ?? string.Empty;
        }
    }
}
