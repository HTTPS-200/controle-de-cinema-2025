using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;

namespace ControleDeCinema.Testes.Interface.ModuloSessao
{
    public class SessaoFormPageObject
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        public SessaoFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        try
        {
            wait.Until(d =>
                d.FindElement(By.CssSelector(cssSelectorToFind: "form[data-se='form']")).Displayed);
        }
        catch (WebDriverTimeoutException)
        {
            DumpOnFailure(driver, "sessao-timeout");
            throw;
        }
    }

    public SessaoFormPageObject PreencherInicio(string inicio)
    {
        wait.Until(d =>
            d.FindElement(By.CssSelector("input[data-se='inputInicio']")).Displayed &&
        d.FindElement(By.CssSelector("input[data-se='inputInicio']")).Enabled
        );

        IWebElement input = driver.FindElement(By.CssSelector("input[data-se='inputInicio']"));

        DateTime dt = DateTime.ParseExact(
            inicio, "yyyy-MM-dd'T'HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);

        string ano = dt.ToString("yyyy", CultureInfo.InvariantCulture);
        string mes = dt.ToString("MM", CultureInfo.InvariantCulture);
        string dia = dt.ToString("dd", CultureInfo.InvariantCulture);
        string hora = dt.ToString("HH", CultureInfo.InvariantCulture);
        string min = dt.ToString("mm", CultureInfo.InvariantCulture);

        input.Click();

        input.SendKeys(dia);

        input.SendKeys(mes);

        input.SendKeys(ano);
        input.SendKeys(Keys.ArrowRight);

        input.SendKeys(hora);

        input.SendKeys(min);

        input.SendKeys(Keys.Tab);

        return this;
    }

    public SessaoFormPageObject PreencherNumeroMaximoIngressos(int quantidade)
        {
            var input = wait.Until(d => d.FindElement(By.CssSelector("input[data-se='inputNumeroMaximoIngressos']")));
            input.Clear();
            input.SendKeys(quantidade.ToString());
            return this;
        }

        public SessaoFormPageObject SelecionarFilme(string filme)
        {
            var selectElem = wait.Until(d => new SelectElement(d.FindElement(By.CssSelector("select[data-se='selectFilme']"))));
            selectElem.SelectByText(filme);
            return this;
        }

        public SessaoFormPageObject SelecionarSala(string sala)
        {
            var selectElem = wait.Until(d => new SelectElement(d.FindElement(By.CssSelector("select[data-se='selectSala']"))));
            selectElem.SelectByText(sala);
            return this;
        }

        public SessaoFormPageObject Confirmar()
        {
            wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();
            wait.Until(d => d.Url.Contains("/sessoes", StringComparison.OrdinalIgnoreCase));
            wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

            return new(driver);
        }

        public SessaoFormPageObject ConfirmarComSucesso()
        {
            Confirmar();
            wait.Until(d => d.Url.Contains("/sessoes", StringComparison.OrdinalIgnoreCase));
            return this;
        }

        public SessaoFormPageObject ConfirmarComErro()
        {
            Confirmar();
            wait.Until(d => d.FindElements(By.CssSelector(".text-danger, .alert-danger")).Any(e => e.Displayed));
            return this;
        }

        public string ObterMensagemErro()
        {
            var elemento = driver.FindElements(By.CssSelector(".text-danger, .alert-danger")).FirstOrDefault(e => e.Displayed);
            return elemento?.Text ?? string.Empty;
        }
        private static void DumpOnFailure(IWebDriver driver, string prefix)
        {
            try
            {
                Screenshot shot = ((ITakesScreenshot)driver).GetScreenshot();
                string png = Path.Combine(Path.GetTempPath(), $"{prefix}-{DateTime.Now:HHmmss}.png");
                shot.SaveAsFile(png);

                string html = Path.Combine(Path.GetTempPath(), $"{prefix}-{DateTime.Now:HHmmss}.html");
                File.WriteAllText(html, driver.PageSource);
            }
            catch { /* best-effort */ }
        }


    }
}
