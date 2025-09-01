using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloAutenticacao;
public class AutentificacaoFormObjects
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public AutentificacaoFormObjects(IWebDriver driver, WebDriverWait wait)
    {
        this.driver = driver;
        this.wait = wait;
    }

    public AutentificacaoFormObjects PreencherEmail(string email)
    {
        var input = wait.Until(d => d.FindElement(By.CssSelector("input[data-se='inputEmail']")));
        input.Clear();
        input.SendKeys(email);
        return this;
    }

    public AutentificacaoFormObjects PreencherSenha(string senha)
    {
        var input = wait.Until(d => d.FindElement(By.CssSelector("input[data-se='inputSenha']")));
        input.Clear();
        input.SendKeys(senha);
        return this;
    }

    public void Confirmar()
    {
        var btn = wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']")));
        btn.Click();
    }


}
