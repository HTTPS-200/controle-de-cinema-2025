using ControleDeCinema.Dominio.ModuloSala;

namespace ControleDeCinema.Testes.Unidade.ModuloSala;

[TestClass]
[TestCategory("Testes de Unidade de Sala")]
public sealed class SalaTests
{
    private Sala? sala;

    [TestMethod]
    public void Deve_Criar_Sala_Corretamente()
    {
        // Arrange
        sala = new Sala(1, 50);

        // Assert
        Assert.AreEqual(1, sala.Numero);
        Assert.AreEqual(50, sala.Capacidade);
    }

    [TestMethod]
    public void Deve_Impedir_Cadastro_Campos_Invalidos()
    {
        // Arrange & Act
        sala = new Sala(0, -10);

        var numeroInvalido = sala.Numero <= 0;
        var capacidadeInvalida = sala.Capacidade <= 0;

        // Assert
        Assert.IsTrue(numeroInvalido, "O número da sala deve ser positivo.");
        Assert.IsTrue(capacidadeInvalida, "A capacidade da sala deve ser maior que zero.");
    }

    // Teste duplicidade de numero da sala
    [TestMethod]
    public void Deve_Impedir_Cadastro_Numero_Duplicado()
    {
        // Arrange
        var listaSalas = new List<Sala>
        {
            new Sala(1, 50),
            new Sala(2, 100),
            new Sala(3, 80)
        };

        var novaSala = new Sala(1, 60);

        // Act
        var numeroDuplicado = listaSalas.Any(s => s.Numero == novaSala.Numero);

        // Assert
        Assert.IsTrue(numeroDuplicado, "Já existe uma sala registrada com este número.");
    }
}