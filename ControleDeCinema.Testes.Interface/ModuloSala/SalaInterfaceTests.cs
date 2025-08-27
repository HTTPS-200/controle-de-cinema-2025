using ControleDeCinema.Testes.Interface.Compartilhado;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        autenticacaoPage.RegistrarContaEmpresarial();
        autenticacaoPage.FazerLogin("Empresa");
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
        var salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        var salaForm = salaIndex.ClickCadastrar()
                                 .PreencherNumero("102")
                                 .PreencherCapacidade("80")
                                 .Confirmar();

        salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        salaForm = salaIndex.ClickEditar()
                            .PreencherNumero("102-A")
                            .PreencherCapacidade("90")
                            .Confirmar();

        salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        Assert.IsTrue(salaIndex.ContemSala("102-A"));
    }

    [TestMethod]
    public void CT009_Deve_Excluir_Sala_Corretamente()
    {
        var salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        var salaForm = salaIndex.ClickCadastrar()
                                 .PreencherNumero("103")
                                 .PreencherCapacidade("70")
                                 .Confirmar();

        salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        salaForm = salaIndex.ClickExcluir().Confirmar();

        salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        Assert.IsFalse(salaIndex.ContemSala("103"));
    }

    [TestMethod]
    public void CT010_Deve_Listar_Todas_As_Salas()
    {
        var salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        var salaForm = salaIndex.ClickCadastrar()
                                 .PreencherNumero("104")
                                 .PreencherCapacidade("100")
                                 .Confirmar();

        salaIndex = new SalaIndexPageObject(driver!).IrPara(enderecoBase!);
        Assert.IsTrue(salaIndex.ContemSala("104"));
    }
}
