using ApresentacaoDotNet.CoolPattern.Behaviour;

namespace ApresentacaoDotNet.CoolPattern.Entidades;

// Não debita e não tem cartão.
public sealed class ContaInvestimento : Conta
{
    public string Produto { get; }

    public ContaInvestimento(string produto, decimal saldoInicial = 0) : base(saldoInicial)
    {
        Produto = produto;
        comportamentoDebito = new DebitoNegado();
        comportamentoCartao = new SemCartao();
    }

    public override void Exibir() =>
        Console.WriteLine($"Sou uma Conta Investimento ({Produto})");
}
