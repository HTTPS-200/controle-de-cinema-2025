using ControleDeCinema.Testes.Interface.Compartilhado;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleDeCinema.Testes.Interface.ModuloFilme
{
    [TestClass]
    [TestCategory("Testes de Interface de Filme")]
    public sealed class FilmeInterfaceTests : TestFixture
    {
        private AutenticacaoPageObject autenticacaoPage;

        [TestInitialize]
        public void InicializarTeste()
        {
            base.InicializarTeste();

            autenticacaoPage = new AutenticacaoPageObject(driver!, enderecoBase!);
            autenticacaoPage.RegistrarContaEmpresarial();
        }

        [TestMethod]
        public void CT001_Deve_Cadastrar_Filme_Corretamente()
        {
            var filmeIndex = new FilmeIndexPageObject(driver!, enderecoBase!).IrPara(enderecoBase!);

            var filmeForm = filmeIndex.ClickCadastrar()
                                      .PreencherTitulo("Titanic")
                                      .PreencherDuracao(195)
                                      .SelecionarGenero("Romance")
                                      .MarcarLancamento(false);

            filmeForm.Confirmar();

            filmeIndex.IrPara(enderecoBase!);

            Assert.IsTrue(filmeIndex.ContemFilme("Titanic"));
        }

        [TestMethod]
        public void CT002_Deve_Editar_Filme_Corretamente()
        {
            var filmeIndex = new FilmeIndexPageObject(driver!, enderecoBase!);

            // Pré-condição: cadastrar filme
            filmeIndex.IrPara(enderecoBase!)
                      .ClickCadastrar()
                      .PreencherTitulo("Avatar")
                      .PreencherDuracao(160)
                      .SelecionarGenero("Ação")
                      .MarcarLancamento(true)
                      .Confirmar();

            var filmeForm = filmeIndex.IrPara(enderecoBase!)
                                      .ClickEditar();

            filmeForm.PreencherTitulo("Avatar 2")
                     .PreencherDuracao(170)
                     .Confirmar();

            filmeIndex.IrPara(enderecoBase!);

            Assert.IsTrue(filmeIndex.ContemFilme("Avatar 2"));
        }

        [TestMethod]
        public void CT003_Deve_Excluir_Filme_Corretamente()
        {
            var filmeIndex = new FilmeIndexPageObject(driver!, enderecoBase!);

            // Pré-condição: cadastrar filme
            filmeIndex.IrPara(enderecoBase!)
                      .ClickCadastrar()
                      .PreencherTitulo("Matrix")
                      .PreencherDuracao(136)
                      .SelecionarGenero("Ação")
                      .MarcarLancamento(false)
                      .Confirmar();

            var filmeForm = filmeIndex.IrPara(enderecoBase!)
                                      .ClickExcluir();

            filmeForm.Confirmar();

            Assert.IsFalse(filmeIndex.ContemFilme("Matrix"));
        }

        [TestMethod]
        public void CT004_Deve_Listar_Todos_Os_Filmes()
        {
            var filmes = new[] { "Interestelar", "Gladiador", "Inception" };
            var filmeIndex = new FilmeIndexPageObject(driver!, enderecoBase!);

            foreach (var f in filmes)
            {
                filmeIndex.IrPara(enderecoBase!)
                          .ClickCadastrar()
                          .PreencherTitulo(f)
                          .PreencherDuracao(120)
                          .SelecionarGenero("Ação")
                          .MarcarLancamento(false)
                          .Confirmar();
            }

            filmeIndex.IrPara(enderecoBase!);

            foreach (var f in filmes)
                Assert.IsTrue(filmeIndex.ContemFilme(f));
        }

        [TestMethod]
        public void CT005_Deve_Validar_Campos_Obrigatorios()
        {
            var filmeIndex = new FilmeIndexPageObject(driver!, enderecoBase!).IrPara(enderecoBase!);

            var filmeForm = filmeIndex.ClickCadastrar()
                                      .PreencherTitulo("")
                                      .PreencherDuracao(0)
                                      .SelecionarGenero("")
                                      .ConfirmarComErro();

            StringAssert.Contains(filmeForm.ObterMensagemErro(), "obrigatório");
        }

        [TestMethod]
        public void CT006_Deve_Validar_Duracao_Positiva()
        {
            var filmeIndex = new FilmeIndexPageObject(driver!, enderecoBase!).IrPara(enderecoBase!);

            var filmeForm = filmeIndex.ClickCadastrar()
                                      .PreencherTitulo("Filme Negativo")
                                      .PreencherDuracao(-50)
                                      .SelecionarGenero("Drama")
                                      .ConfirmarComErro();

            StringAssert.Contains(filmeForm.ObterMensagemErro().ToLower(), "positiva");
        }

        [TestMethod]
        public void CT007_Deve_Validar_Titulo_Duplicado()
        {
            var filmeIndex = new FilmeIndexPageObject(driver!, enderecoBase!);

            filmeIndex.IrPara(enderecoBase!)
                      .ClickCadastrar()
                      .PreencherTitulo("Duplicado")
                      .PreencherDuracao(120)
                      .SelecionarGenero("Comédia")
                      .ConfirmarComSucesso();

            var filmeForm = filmeIndex.IrPara(enderecoBase!)
                                      .ClickCadastrar()
                                      .PreencherTitulo("Duplicado")
                                      .PreencherDuracao(130)
                                      .SelecionarGenero("Comédia")
                                      .ConfirmarComErro();

            StringAssert.Contains(filmeForm.ObterMensagemErro().ToLower(), "já está em uso");
        }
    }
}
