using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloFilme;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using ControleDeCinema.Testes.Interface.ModuloSessao;
using OpenQA.Selenium;
using TesteFacil.Testes.Interface.ModuloSala;

public class DadosParaTesteHelper
{
    private readonly IWebDriver driver;
    private readonly string enderecoBase;

    public DadosParaTesteHelper(IWebDriver driver, string enderecoBase)
    {
        this.driver = driver;
        this.enderecoBase = enderecoBase;
    }

    public void CadastrarGenero(string descricao)
    {
        var generoIndex = new GeneroFilmeIndexPageObject(driver);
        generoIndex.IrPara(enderecoBase)
                   .ClickCadastrar()
                   .PreencherDescricao(descricao)
                   .ConfirmarComSucesso();
    }

    public void CadastrarFilme(string titulo, int duracao, string genero, bool lancamento = false)
    {
        var filmeIndex = new FilmeIndexPageObject(driver, enderecoBase);
        var form = filmeIndex.IrPara(enderecoBase).ClickCadastrar();
        form.PreencherTitulo(titulo)
            .PreencherDuracao(duracao)
            .SelecionarGenero(genero);

        if (lancamento)
            form.MarcarLancamento();

        form.ConfirmarComSucesso();
    }

    public void CadastrarSala(string numero, int capacidade)
    {
        var salaIndex = new SalaIndexPageObject(driver);
        var form = salaIndex.IrPara(enderecoBase)
                            .ClickCadastrar(); 

        form.PreencherNumero(numero)
            .PreencherCapacidade(capacidade.ToString())
            .Confirmar(); 
    }

    public void CadastrarSessao(string filme, string sala, string inicio, int maxIngressos)
    {
        var sessaoIndex = new SessaoIndexPageObject(driver);
        var form = sessaoIndex.IrPara(enderecoBase).ClickCadastrar();
        form.SelecionarFilme(filme)
            .SelecionarSala(sala)
            .PreencherInicio(inicio)
            .PreencherNumeroMaximoIngressos(maxIngressos)
            .ConfirmarComSucesso();
    }

    public void RegistrarContaCliente(AutenticacaoPageObject auth) => auth.RegistrarContaCliente();
    public void RegistrarContaEmpresa(AutenticacaoPageObject auth) => auth.RegistrarContaEmpresarial();
}
