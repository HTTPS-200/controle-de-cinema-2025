using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Interface.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;

[TestClass]
[TestCategory("Testes de Interface de Filme")]
public sealed class FilmeInterfaceTests : TestFixture
{
    protected AutenticacaoPageObject autenticacaoPage;
    protected GeneroFilme generoFilme;

    [TestInitialize]
    public void InicializarTeste()
    {
        base.InicializarTeste();

        autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);

        autenticacaoPage.RegistrarContaEmpresarial();
    }

    [TestMethod]
    public void Deve_Cadastrar_Filme_Corretamente()
    {


    }
}
