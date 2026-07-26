using ApresentacaoDotNet.CoolPattern.Behaviour;

namespace ApresentacaoDotNet.CoolPattern.Entidades;

// Pluga os comportamentos no construtor.
public sealed class ContaCorrente : Conta
{
    public ContaCorrente(decimal saldoInicial = 0) : base(saldoInicial)
    {
        comportamentoDebito = new DebitoLiberado();
        comportamentoCartao = new CartaoDebitoAtivo();
    }

    public override void Exibir() =>
        Console.WriteLine("Sou uma Conta Corrente");
}
