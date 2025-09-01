using ControleDeCinema.Testes.Interface.Compartilhado;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace TesteFacil.Testes.Interface.ModuloSala;

[TestClass]
[TestCategory("Tests de Interface de Sala")]
public sealed class SalaInterfaceTests : TestFixture
{
    private AutenticacaoPageObject autenticacaoPage;

    [TestInitialize]
    public void InicializarTeste()
    {
        base.InicializarTeste();

        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
        driver!.Manage().Cookies.DeleteAllCookies();
        autenticacaoPage.RegistrarContaEmpresarial();
        //autenticacaoPage.FazerLogin("Empresa");
    }

    [TestMethod]
    public void CT007_Deve_Cadastrar_Sala_Corretamente()
    {
        var salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        var salaForm = salaIndex.ClickCadastrar()
                                 .PreencherNumero("101")
                                 .PreencherCapacidade("50")
                                 .Confirmar();

        salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        Assert.IsTrue(salaIndex.ContemSala("101"));
    }

    [TestMethod]
    public void CT008_Deve_Editar_Sala_Corretamente()
    {
        var salaIndex = new SalaIndexPageObject(driver!);

        salaIndex
            .IrPara(enderecoBase!)
            .ClickCadastrar()
            .PreencherNumero("5")
            .PreencherCapacidade("50")
            .Confirmar();

        var salaForm = salaIndex
            .IrPara(enderecoBase!)
            .ClickEditar();

        salaForm
            .PreencherNumero("7")
            .PreencherCapacidade("60")
            .Confirmar();

        Assert.IsTrue(salaIndex.ContemSala("7"));
    }

    [TestMethod]
    public void CT009_Deve_Excluir_Sala_Corretamente()
    {
        var salaIndex = new SalaIndexPageObject(driver!);

        salaIndex
            .IrPara(enderecoBase!)
            .ClickCadastrar()
            .PreencherNumero("9")
            .PreencherCapacidade("100")
            .Confirmar();

        var salaForm = salaIndex
            .IrPara(enderecoBase!)
            .ClickExcluir();

        salaForm.Confirmar();

        Assert.IsFalse(salaIndex.ContemSala("9"));
    }

    [TestMethod]
    public void CT010_Deve_Listar_Todas_As_Salas()
    {
        var salaIndex = new SalaIndexPageObject(driver!);

        salaIndex
            .IrPara(enderecoBase!)
            .ClickCadastrar()
            .PreencherNumero("1")
            .PreencherCapacidade("30")
            .Confirmar();

        salaIndex
            .IrPara(enderecoBase!)
            .ClickCadastrar()
            .PreencherNumero("2")
            .PreencherCapacidade("40")
            .Confirmar();

        salaIndex.IrPara(enderecoBase!);

        Assert.IsTrue(salaIndex.ContemSala("1"));
        Assert.IsTrue(salaIndex.ContemSala("2"));
    }
}
