using ControleDeCinema.Testes.Interface.Compartilhado;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

[TestClass]
[TestCategory("Tests de Interface de GeneroFilme")]
public sealed class GeneroFilmeInterfaceTests : TestFixture
{
    private AutenticacaoPageObject autenticacaoPage;

    [TestInitialize]
    public void InicializarTeste()
    {
        base.InicializarTeste();
        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);

        driver!.Manage().Cookies.DeleteAllCookies();
        driver.Navigate().GoToUrl(enderecoBase!);

        ((IJavaScriptExecutor)driver).ExecuteScript("window.localStorage.clear();");
        ((IJavaScriptExecutor)driver).ExecuteScript("window.sessionStorage.clear();");

        if (!autenticacaoPage.EstaLogado())
        {
            try
            {
                autenticacaoPage.RegistrarContaEmpresarial();
            }
            catch (WebDriverTimeoutException)
            {
                autenticacaoPage.FazerLogin("Empresa");
            }
            catch (NoSuchElementException)
            {
                autenticacaoPage.FazerLogin("Empresa");
            }
        }
    }

    [TestMethod]
    public void CT001_Deve_Cadastrar_Genero_Corretamente()
    {
        var generoIndex = new GeneroFilmeIndexPageObject(driver!).IrPara(enderecoBase!);
        var generoForm = generoIndex.ClickCadastrar()
                                    .PreencherDescricao("Ação")
                                    .Confirmar();

        Assert.IsTrue(generoForm.ContemGenero("Ação"));
    }

    [TestMethod]
    public void CT002_Deve_Editar_Genero_Corretamente()
    {
        var generoIndex = new GeneroFilmeIndexPageObject(driver!).IrPara(enderecoBase!);

        generoIndex.ClickCadastrar()
                   .PreencherDescricao("Romance")
                   .Confirmar();

        var formEditar = generoIndex.IrPara(enderecoBase!).ClickEditar();
        formEditar.PreencherDescricao("Comédia").Confirmar();

        Assert.IsTrue(generoIndex.ContemGenero("Comédia"));
    }

    [TestMethod]
    public void CT003_Deve_Excluir_Genero_Corretamente()
    {
        var generoIndex = new GeneroFilmeIndexPageObject(driver!).IrPara(enderecoBase!);

        generoIndex
            .ClickCadastrar()
            .PreencherDescricao("Românce")
            .Confirmar();

        var generoForm = generoIndex
            .IrPara(enderecoBase!)
            .ClickExcluir();

        generoForm.ClickConfirmarExcluir("Românce");

        Assert.IsFalse(generoIndex.ContemGenero("Românce"));
    }

    [TestMethod]
    public void CT004_Deve_Listar_Todos_Os_Generos()
    {
        var generoIndex = new GeneroFilmeIndexPageObject(driver!);

        generoIndex.IrPara(enderecoBase!)
                   .ClickCadastrar()
                   .PreencherDescricao("Ação")
                   .Confirmar();

        generoIndex.IrPara(enderecoBase!)
                   .ClickCadastrar()
                   .PreencherDescricao("Romance")
                   .Confirmar();

        generoIndex.IrPara(enderecoBase!);

        Assert.IsTrue(generoIndex.ContemGenero("Ação"));
        Assert.IsTrue(generoIndex.ContemGenero("Romance"));
    }

    [TestMethod]
    public void CT005_Deve_Validar_Campo_Obrigatorio()
    {
        var generoIndex = new GeneroFilmeIndexPageObject(driver!).IrPara(enderecoBase!);

        var generoForm = generoIndex
            .ClickCadastrar()
            .PreencherDescricao("")
            .ConfirmarComErro();

        Assert.AreEqual("O campo \"Descrição\" é obrigatório.", generoForm.ObterMensagemErro());

        // StringAssert.Contains(generoForm.ObterMensagemErro(), "Descrição");
        // StringAssert.Contains(generoForm.ObterMensagemErro(), "obrigatório");
    }

    [TestMethod]
    public void CT006_Deve_Validar_Genero_Duplicado()
    {
        var generoIndex = new GeneroFilmeIndexPageObject(driver!).IrPara(enderecoBase!);

        generoIndex
            .ClickCadastrar()
            .PreencherDescricao("Aventura")
            .ConfirmarComSucesso();

        var generoForm = generoIndex
            .IrPara(enderecoBase!)
            .ClickCadastrar()
            .PreencherDescricao("Aventura")
            .ConfirmarComErro();

        StringAssert.Contains(
            generoForm.ObterMensagemErro().ToLower(),
            "já existe"
        );
    }

}
