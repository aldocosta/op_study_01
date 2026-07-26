using ApresentacaoDotNet.CoolPattern.Behaviour;

namespace ApresentacaoDotNet.CoolPattern.Entidades;

// Pode debitar, mas não tem cartão.
public sealed class ContaPoupanca : Conta
{
    public ContaPoupanca(decimal saldoInicial = 0) : base(saldoInicial)
    {
        comportamentoDebito = new DebitoLiberado();
        comportamentoCartao = new SemCartao();
    }

    public override void Exibir() =>
        Console.WriteLine("Sou uma Conta Poupança");
}
