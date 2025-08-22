using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Integracao.Compatilhado;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ControleDeCinema.Testes.Integracao.ModuloSessao
{
    [TestClass]
    [TestCategory("Testes de Integração de Sessão")]
    public sealed class RepositorioSessaoEmOrmTests : TestFixture
    {
        [TestMethod]
        public void Deve_Cadastrar_Sessao_Corretamente()
        {
            // Arrange
            var sala = new Sala(1, 50);
            repositorioSala?.Cadastrar(sala);

            var genero = new GeneroFilme("Ação");
            repositorioGeneroFilme?.Cadastrar(genero);

            var filme = new Filme("Matrix", 120, false, genero);
            repositorioFilme?.Cadastrar(filme);

            dbContext?.SaveChanges();

            var sessao = new Sessao(DateTime.UtcNow.AddHours(1), 30, filme, sala);

            // Act
            repositorioSessao?.Cadastrar(sessao);
            dbContext?.SaveChanges();

            // Assert
            var registroSelecionado = repositorioSessao?.SelecionarRegistroPorId(sessao.Id);
            Assert.AreEqual(sessao, registroSelecionado);
        }

        [TestMethod]
        public void Deve_Editar_Sessao_Corretamente()
        {
            // Arrange
            var sala = new Sala(1, 50);
            repositorioSala?.Cadastrar(sala);

            var genero = new GeneroFilme("Ação");
            repositorioGeneroFilme?.Cadastrar(genero);

            var filme = new Filme("Matrix", 120, false, genero);
            repositorioFilme?.Cadastrar(filme);

            dbContext?.SaveChanges();

            var sessao = new Sessao(DateTime.UtcNow.AddHours(1), 30, filme, sala);
            repositorioSessao?.Cadastrar(sessao);
            dbContext?.SaveChanges();

            var sessaoEditada = new Sessao(DateTime.UtcNow.AddHours(2), 40, filme, sala);

            // Act
            var conseguiuEditar = repositorioSessao?.Editar(sessao.Id, sessaoEditada);
            dbContext?.SaveChanges();

            // Assert
            var registroSelecionado = repositorioSessao?.SelecionarRegistroPorId(sessao.Id);
            Assert.IsTrue(conseguiuEditar);
            Assert.AreEqual(sessao.NumeroMaximoIngressos, registroSelecionado?.NumeroMaximoIngressos);
            Assert.AreEqual(sessao.Inicio, registroSelecionado?.Inicio);
        }

        [TestMethod]
        public void Deve_Excluir_Sessao_Corretamente()
        {
            // Arrange
            var sala = new Sala(1, 50);
            repositorioSala?.Cadastrar(sala);

            var genero = new GeneroFilme("Ação");
            repositorioGeneroFilme?.Cadastrar(genero);

            var filme = new Filme("Matrix", 120, false, genero);
            repositorioFilme?.Cadastrar(filme);

            dbContext?.SaveChanges();

            var sessao = new Sessao(DateTime.UtcNow.AddHours(1), 30, filme, sala);
            repositorioSessao?.Cadastrar(sessao);
            dbContext?.SaveChanges();

            // Act
            var conseguiuExcluir = repositorioSessao?.Excluir(sessao.Id);
            dbContext?.SaveChanges();

            // Assert
            var registroSelecionado = repositorioSessao?.SelecionarRegistroPorId(sessao.Id);
            Assert.IsTrue(conseguiuExcluir);
            Assert.IsNull(registroSelecionado);
        }

        [TestMethod]
        public void Deve_Selecionar_Sessoes_Corretamente()
        {
            // Arrange
            var sala1 = new Sala(1, 50);
            var sala2 = new Sala(2, 30);
            repositorioSala?.CadastrarEntidades(new List<Sala> { sala1, sala2 });

            var genero1 = new GeneroFilme("Ação");
            var genero2 = new GeneroFilme("Ficção");
            repositorioGeneroFilme?.CadastrarEntidades(new List<GeneroFilme> { genero1, genero2 });

            var filme1 = new Filme("Matrix", 120, false, genero1);
            var filme2 = new Filme("Avatar", 150, false, genero2);
            repositorioFilme?.CadastrarEntidades(new List<Filme> { filme1, filme2 });

            dbContext?.SaveChanges();

            var sessao1 = new Sessao(DateTime.UtcNow.AddHours(1), 30, filme1, sala1);
            var sessao2 = new Sessao(DateTime.UtcNow.AddHours(2), 25, filme2, sala1);
            var sessao3 = new Sessao(DateTime.UtcNow.AddHours(3), 40, filme1, sala2);

            var sessoesEsperadas = new List<Sessao> { sessao1, sessao2, sessao3 };
            repositorioSessao?.CadastrarEntidades(sessoesEsperadas);
            dbContext?.SaveChanges();

            // Act
            var sessoesRecebidas = repositorioSessao?.SelecionarRegistros();

            // Assert
            CollectionAssert.AreEquivalent(sessoesEsperadas, sessoesRecebidas);
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Conflito_De_Horario()
        {
            // Arrange
            var sala = new Sala(1, 50);
            repositorioSala?.Cadastrar(sala);

            var genero = new GeneroFilme("Ação");
            repositorioGeneroFilme?.Cadastrar(genero);

            var filme = new Filme("Matrix", 120, false, genero);
            repositorioFilme?.Cadastrar(filme);

            dbContext?.SaveChanges();

            var inicio = DateTime.UtcNow.AddHours(1);
            var sessao1 = new Sessao(inicio, 30, filme, sala);
            var sessao2 = new Sessao(inicio, 25, filme, sala);

            repositorioSessao?.Cadastrar(sessao1);
            dbContext?.SaveChanges();

            // Act
            var duplicada = repositorioSessao?.SelecionarRegistros()
                .Any(s => s.Sala.Id == sala.Id && s.Inicio == inicio);

            // Assert
            Assert.IsTrue(duplicada);
        }

        [TestMethod]
        public void Nao_Deve_Cadastrar_Maior_Capacidade()
        {
            // Arrange
            var sala = new Sala(1, 50);
            repositorioSala?.Cadastrar(sala);

            var genero = new GeneroFilme("Ação");
            repositorioGeneroFilme?.Cadastrar(genero);

            var filme = new Filme("Matrix", 120, false, genero);
            repositorioFilme?.Cadastrar(filme);

            dbContext?.SaveChanges();

            var sessao = new Sessao(DateTime.UtcNow.AddHours(1), 100, filme, sala);

            // Act
            var excedeCapacidade = sessao.NumeroMaximoIngressos > sala.Capacidade;

            // Assert
            Assert.IsTrue(excedeCapacidade);
        }
    }
}
