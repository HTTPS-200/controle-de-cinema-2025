using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
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
            GeneroFilmeIndexPageObject generoFilmeIndex = new(driver);
            generoFilmeIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherDescricao("Terror")
                .ConfirmarComSucesso();

            FilmeIndexPageObject filmeIndex = new(driver, enderecoBase);

            FilmeFormPageObject filmeForm = filmeIndex
                .IrPara(enderecoBase)
                .ClickCadastrar();

            filmeForm
                .PreencherTitulo("Alien o 8 Passageiro")
                .PreencherDuracao(117)
                .MarcarLancamento()
                .SelecionarGenero("Terror")
                .ConfirmarComSucesso();

            Assert.IsTrue(filmeIndex.ContemFilme("Alien o 8 Passageiro"));
        }


        [TestMethod]
        public void CT002_Deve_Editar_Filme_Corretamente()
        {
            GeneroFilmeIndexPageObject generoFilmeIndex = new(driver);
            generoFilmeIndex
                .IrPara(enderecoBase)
                .ClickCadastrar()
                .PreencherDescricao("Terror")
                .ConfirmarComSucesso();


            FilmeIndexPageObject filmeIndex = new(driver, enderecoBase);
            FilmeFormPageObject filmeForm = filmeIndex
                .IrPara(enderecoBase)
                .ClickCadastrar();

            filmeForm
                .PreencherTitulo("Alien o 8 Passageiro")
                .PreencherDuracao(117)
                .MarcarLancamento()
                .SelecionarGenero("Terror")
                .ConfirmarComSucesso();

            filmeForm = filmeIndex
                .IrPara(enderecoBase)
                .ClickEditar();

            filmeForm
                .PreencherTitulo("Alien o 8 Passageiro Editada")
                .PreencherDuracao(117)
                .MarcarLancamento()
                .SelecionarGenero("Terror")
                .ConfirmarComSucesso();

            Assert.IsTrue(filmeIndex.ContemFilme("Alien o 8 Passageiro Editada"));
        }


        [TestMethod]
        public void CT003_Deve_Excluir_Filme_Corretamente()
        {
            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex.IrPara(enderecoBase)
                       .ClickCadastrar()
                       .PreencherDescricao("Terror")
                       .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            var filmeForm = filmeIndex.IrPara(enderecoBase)
                                      .ClickCadastrar();

            filmeForm.PreencherTitulo("Alien o 8 Passageiro")
                     .PreencherDuracao(117)
                     .MarcarLancamento()
                     .SelecionarGenero("Terror")
                     .ConfirmarComSucesso();

            filmeForm = filmeIndex.IrPara(enderecoBase)
                                  .ClickExcluir();

            filmeForm.ConfirmarComSucesso();

            Assert.IsFalse(filmeIndex.ContemFilme("Alien o 8 Passageiro"));
        }

        [TestMethod]
        public void CT004_Deve_Listar_Todos_Os_Filmes()
        {
            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex.IrPara(enderecoBase)
                       .ClickCadastrar()
                       .PreencherDescricao("Terror")
                       .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);

            string[] filmes = { "Alien o 8 Passageiro", "Predador" };
            int[] duracoes = { 117, 91 };

            for (int i = 0; i < filmes.Length; i++)
            {
                var form = filmeIndex.IrPara(enderecoBase)
                                     .ClickCadastrar();

                form.PreencherTitulo(filmes[i])
                    .PreencherDuracao(duracoes[i])
                    .MarcarLancamento()
                    .SelecionarGenero("Terror")
                    .ConfirmarComSucesso();
            }

            filmeIndex.IrPara(enderecoBase);

            foreach (var f in filmes)
                Assert.IsTrue(filmeIndex.ContemFilme(f));
        }

        [TestMethod]
        public void CT005_Deve_Validar_Campos_Obrigatorios()
        {
            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex.IrPara(enderecoBase)
                       .ClickCadastrar()
                       .PreencherDescricao("Terror")
                       .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            var filmeForm = filmeIndex.IrPara(enderecoBase)
                                      .ClickCadastrar();

            filmeForm.ConfirmarComErro();

            string erro = filmeForm.ObterMensagemErro();
            StringAssert.Contains(erro, "obrigatório");
        }

        [TestMethod]
        public void CT006_Deve_Validar_Duracao_Positiva()
        {
            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex.IrPara(enderecoBase)
                       .ClickCadastrar()
                       .PreencherDescricao("Terror")
                       .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
            var filmeForm = filmeIndex.IrPara(enderecoBase)
                                      .ClickCadastrar();

            filmeForm.PreencherTitulo("Alien o 8 Passageiro")
                     .PreencherDuracao(0) 
                     .MarcarLancamento()
                     .SelecionarGenero("Terror")
                     .ConfirmarComErro();

            string erro = filmeForm.ObterMensagemErro();

            StringAssert.Contains(erro.ToLower(), "acima de 0");
        }

        [TestMethod]
        public void CT007_Deve_Validar_Titulo_Duplicado()
        {
            var generoIndex = new GeneroFilmeIndexPageObject(driver);
            generoIndex.IrPara(enderecoBase)
                       .ClickCadastrar()
                       .PreencherDescricao("Ação")
                       .ConfirmarComSucesso();

            var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);

            filmeIndex.IrPara(enderecoBase)
                      .ClickCadastrar()
                      .PreencherTitulo("Matrix")
                      .PreencherDuracao(120)
                      .MarcarLancamento()
                      .SelecionarGenero("Ação")
                      .ConfirmarComSucesso();

            var filmeForm = filmeIndex.ClickCadastrar();
            filmeForm.PreencherTitulo("Matrix")
                     .PreencherDuracao(130)
                     .MarcarLancamento()
                     .SelecionarGenero("Ação")
                     .ConfirmarComErro();

            string erro = filmeForm.ObterMensagemErro();

            StringAssert.Contains(erro.ToLower(), "já existe um filme");
        }

    }
}