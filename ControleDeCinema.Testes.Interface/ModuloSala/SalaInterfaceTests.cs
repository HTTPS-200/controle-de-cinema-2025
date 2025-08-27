using ControleDeCinema.Testes.Interface.Compartilhado;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TesteFacil.Testes.Interface.ModuloSala;

[TestClass]
[TestCategory("Tests de Interface de Sala")]
public sealed class SalaInterfaceTests : TestFixture
{
    private AutenticacaoPage autenticacaoPage;
    [TestInitialize]
    //public void InicializarTeste()
    //{
    //    base.InicializarTeste();

    //    autenticacaoPage = new AutenticacaoPage(driver);

    //    autenticacaoPage.RegistrarContaEmpresarial(enderecoBase);
    //}

    [TestMethod]
    public void CT007_Deve_Cadastrar_Sala_Corretamente()
    {
        var indexPage = new SalaIndexPageObject(driver)
            .IrPara(enderecoBase);

        indexPage
            .ClickCadastrar()
            .PreencherNumero("101")
            .PreencherCapacidade("50")
            .Confirmar();

        Assert.IsTrue(indexPage.ContemSala("101"));
    }

    [TestMethod]
    public void CT008_Deve_Editar_Sala_Corretamente()
    {
        var indexPage = new SalaIndexPageObject(driver)
            .IrPara(enderecoBase);

        indexPage
            .ClickCadastrar()
            .PreencherNumero("102")
            .PreencherCapacidade("80")
            .Confirmar();

        indexPage
            .ClickEditar()
            .PreencherNumero("102-A")
            .PreencherCapacidade("90")
            .Confirmar();

        Assert.IsTrue(indexPage.ContemSala("102-A"));
    }

    [TestMethod]
    public void CT009_Deve_Excluir_Sala_Corretamente()
    {
        var indexPage = new SalaIndexPageObject(driver)
            .IrPara(enderecoBase);

        indexPage
            .ClickCadastrar()
            .PreencherNumero("103")
            .PreencherCapacidade("70")
            .Confirmar();

        indexPage
            .ClickExcluir()
            .Confirmar();

        Assert.IsFalse(indexPage.ContemSala("103"));
    }

    [TestMethod]
    public void CT010_Deve_Listar_Todas_As_Salas()
    {
        var indexPage = new SalaIndexPageObject(driver)
            .IrPara(enderecoBase);

        indexPage
            .ClickCadastrar()
            .PreencherNumero("104")
            .PreencherCapacidade("100")
            .Confirmar();

        Assert.IsTrue(indexPage.ContemSala("104"));
    }
}
