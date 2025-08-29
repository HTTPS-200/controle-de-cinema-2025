using ControleDeCinema.Testes.Interface.Compartilhado;
using OpenQA.Selenium;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

[TestClass]
[TestCategory("Tests de Interface de GeneroFilme")]
public sealed class GeneroFilmeIntefaceTests : TestFixture
{
    protected AutenticacaoPageObject autenticacaoPage;

    [TestInitialize]
    public void InicializarTeste()
    {
        base.InicializarTeste();

        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);

        autenticacaoPage.RegistrarContaEmpresarial();
    }


    [TestMethod]
    public void Deve_Cadastrar_GeneroFilme_Corretamente()
    {
        var nome = "Ação";
        var indexPage = new GeneroFilmeIndexPageObject(driver!);
        var formPage = indexPage
            .IrPara(enderecoBase!)
            .ClickCadastrar();
       
        indexPage = formPage
            .PreencherNome(nome)
            .Confirmar();
        
        Assert.IsTrue(indexPage.ContemGenero(nome));
    }

    [TestMethod]
    public void Deve_Editar_GeneroFilme_Corretamente()
    {
        var nome = "Ação";
        var nomeEditado = "Comédia";
        var indexPage = new GeneroFilmeIndexPageObject(driver!);
        
        var formPage = indexPage
            .IrPara(enderecoBase!)
            .ClickCadastrar();
        
        indexPage = formPage
            .PreencherNome(nome)
            .Confirmar();
        
        formPage = indexPage
            .ClickEditar();
        
        indexPage = formPage
            .PreencherNome(nomeEditado)
            .Confirmar();

        Assert.AreEqual(true, indexPage.ContemGenero(nomeEditado));
    }

    [TestMethod]
    public void Deve_Excluir_GeneroFilme_Corretamente()
    {
        var nome = "Ação";
        var indexPage = new GeneroFilmeIndexPageObject(driver!);
        
        var formPage = indexPage
            .IrPara(enderecoBase!)
            .ClickCadastrar();
        
        indexPage = formPage
            .PreencherNome(nome)
            .Confirmar();
        
        formPage = indexPage
            .ClickExcluir();
        
        indexPage = formPage
            .Confirmar();
        
        Assert.AreEqual(false, indexPage.ContemGenero(nome));
    }
}
