using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Infraestrutura.Orm.ModuloSala;
using ControleDeCinema.Infraestrutura.Orm.Compartilhado;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModuloSala
{
    [TestClass]
    [TestCategory("Testes de Integração de Sala")]
    public sealed class RepositorioSalaEmOrmTests : TestFixture
    {
        [TestMethod]
        public void Deve_Cadastrar_Sala_Corretamente()
        {
            // Arrange
            var sala = new Sala(1, 50);

            // Act
            repositorioSala?.Cadastrar(sala);
            dbContext?.SaveChanges();

            // Assert
            var registroSelecionado = repositorioSala?.SelecionarRegistroPorId(sala.Id);
            Assert.AreEqual(sala, registroSelecionado);
        }

        [TestMethod]
        public void Deve_Editar_Sala_Corretamente()
        {
            // Arrange
            var sala = new Sala(1, 50);
            repositorioSala?.Cadastrar(sala);
            dbContext?.SaveChanges();

            var salaEditada = new Sala(2, 100);

            // Act
            var conseguiuEditar = repositorioSala?.Editar(sala.Id, salaEditada);
            dbContext?.SaveChanges();

            // Assert
            var registroSelecionado = repositorioSala?.SelecionarRegistroPorId(sala.Id);
            Assert.IsTrue(conseguiuEditar);
            Assert.AreEqual(sala.Numero, registroSelecionado.Numero);
            Assert.AreEqual(sala.Capacidade, registroSelecionado.Capacidade);
        }

        [TestMethod]
        public void Deve_Excluir_Sala_Corretamente()
        {
            // Arrange
            var sala = new Sala(1, 50);
            repositorioSala?.Cadastrar(sala);
            dbContext?.SaveChanges();

            // Act
            var conseguiuExcluir = repositorioSala?.Excluir(sala.Id);
            dbContext?.SaveChanges();

            // Assert
            var registroSelecionado = repositorioSala?.SelecionarRegistroPorId(sala.Id);
            Assert.IsTrue(conseguiuExcluir);
            Assert.IsNull(registroSelecionado);
        }
        [TestMethod]
        public void Deve_Selecionar_Salas_Corretamente()
        {
            // Arrange
            var sala1 = new Sala(1, 50);
            var sala2 = new Sala(2, 100);
            var sala3 = new Sala(3, 75);

            var salasEsperadas = new List<Sala> { sala1, sala2, sala3 };

            repositorioSala?.CadastrarEntidades(salasEsperadas);
            dbContext?.SaveChanges();

            var salasEsperadasOrdenadas = salasEsperadas.OrderBy(s => s.Numero).ToList();

            // Act
            var salasRecebidas = repositorioSala?.SelecionarRegistros()
                .OrderBy(s => s.Numero)
                .ToList();

            // Assert
            Assert.IsNotNull(salasRecebidas);
            Assert.AreEqual(salasEsperadasOrdenadas.Count, salasRecebidas.Count);

            for (int i = 0; i < salasEsperadasOrdenadas.Count; i++)
            {
                Assert.AreEqual(salasEsperadasOrdenadas[i].Id, salasRecebidas[i].Id, $"Erro no Id da sala no índice {i}");
                Assert.AreEqual(salasEsperadasOrdenadas[i].Numero, salasRecebidas[i].Numero, $"Erro no Número da sala no índice {i}");
                Assert.AreEqual(salasEsperadasOrdenadas[i].Capacidade, salasRecebidas[i].Capacidade, $"Erro na Capacidade da sala no índice {i}");
            }
        }

    }
}