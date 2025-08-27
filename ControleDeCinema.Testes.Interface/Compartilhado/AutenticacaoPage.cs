using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.EntityFrameworkCore;
using ControleDeCinema.Infraestrutura.Orm.Compartilhado;
using ControleDeCinema.Dominio.ModuloAutenticacao;

public class AutenticacaoPage
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public AutenticacaoPage(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public void CriarUsuarioEmpresa(string usuario, string senha)
    {
        var options = new DbContextOptionsBuilder<ControleDeCinemaDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=controle-de-cinema-db;Username=postgres;Password=MinhaSenhaFraca")
            .Options;

        using var dbContext = new ControleDeCinemaDbContext(options);

        if (dbContext.Users.Any(u => u.Email == usuario))
            return;

        var novoUsuario = new Usuario
        {
            UserName = usuario,
            Email = usuario,
            EmailConfirmed = true
        };

        var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Usuario>();
        novoUsuario.PasswordHash = passwordHasher.HashPassword(novoUsuario, senha);

        dbContext.Users.Add(novoUsuario);

        var empresaRole = dbContext.Roles.FirstOrDefault(r => r.Name == "Empresa");
        if (empresaRole == null)
        {
            empresaRole = new Cargo
            {
                Name = "Empresa",
                NormalizedName = "EMPRESA"
                // Add other properties as needed
            };

            dbContext.Roles.Add(empresaRole);
            dbContext.SaveChanges();
        }

        dbContext.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>
        {
            UserId = novoUsuario.Id,
            RoleId = empresaRole.Id
        });

        dbContext.SaveChanges();
    }

    public void RealizarLogin(string enderecoBase, string usuario, string senha)
    {
        driver.Navigate().GoToUrl($"{enderecoBase}autentificacao");
        driver.FindElement(By.Id("linkLogin")).Click();

        var inputUsuario = wait.Until(d => d.FindElement(By.Id("button[type='Email']")));
        var inputSenha = driver.FindElement(By.Id("button[type='Senha']"));
        var btnLogin = driver.FindElement(By.CssSelector("button[type='submit']"));

        inputUsuario.Clear();
        inputUsuario.SendKeys(usuario);

        inputSenha.Clear();
        inputSenha.SendKeys(senha);

        btnLogin.Click();

        wait.Until(d => d.FindElement(By.Id("Usuario")).Displayed);
    }
}
