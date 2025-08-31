using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ControleDeCinema.Testes.Interface.ModuloIngresso;

public class IngressoFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public IngressoFormPageObject(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        wait.Until(d => d.FindElements(By.CssSelector("form[data-se='form']")).Any());
    }

    public IngressoFormPageObject SelecionarAssento(string assento)
    {
        try
        {
            var select = wait.Until(d => new SelectElement(d.FindElement(By.CssSelector("select[data-se='selectAssento']"))));
            if (select.Options.Count == 0)
            {
                Console.WriteLine("Nenhum assento disponível no dropdown. Tentando recarregar a página...");
                driver.Navigate().Refresh();
                Thread.Sleep(1000);

                select = new SelectElement(driver.FindElement(By.CssSelector("select[data-se='selectAssento']")));
            }

            if (select.Options.Count > 0)
            {
                var assentoDisponivel = select.Options.FirstOrDefault(op => op.Text.Contains(assento));
                if (assentoDisponivel != null)
                {
                    select.SelectByText(assento);
                }
                else
                {
                    select.SelectByIndex(0);
                }
            }
            else
            {
                Console.WriteLine("Ainda não há assentos disponíveis após recarregar");
            }

            return this;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao selecionar assento: {ex.Message}");
            throw;
        }
    }

    public IngressoFormPageObject MarcarMeiaEntrada(bool marcar = true)
    {
        var checkbox = wait.Until(d => d.FindElement(By.CssSelector("input[data-se='checkboxMeiaEntrada']")));
        if (checkbox.Selected != marcar)
            checkbox.Click();
        return this;
    }

    public IngressoIndexPageObject ConfirmarCompra()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();

        wait.Until(d => d.Url.Contains("/sessoes", StringComparison.OrdinalIgnoreCase) ||
                       d.FindElements(By.CssSelector(".alert-success, .alert-info, .text-success")).Any());

        return new IngressoIndexPageObject(driver);
    }

    public IngressoFormPageObject ConfirmarComErro()
    {
        var waitCurto = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        waitCurto.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

        waitCurto.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();

        waitCurto.Until(d =>
            d.FindElements(By.CssSelector(".text-danger, .alert-danger, .field-validation-error"))
             .Any(e => e.Displayed && !string.IsNullOrEmpty(e.Text)) ||
            d.Url.Contains("/error", StringComparison.OrdinalIgnoreCase) ||
            d.FindElements(By.CssSelector(".input-validation-error")).Any()
        );

        return this;
    }

    public string ObterMensagemErro()
    {
        var elemento = driver.FindElements(By.CssSelector(".text-danger, .alert-danger"))
                            .FirstOrDefault(e => e.Displayed);
        return elemento?.Text ?? string.Empty;
    }
}
