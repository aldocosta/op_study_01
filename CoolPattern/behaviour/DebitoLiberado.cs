using ApresentacaoDotNet.CoolPattern.Entidades;

namespace ApresentacaoDotNet.CoolPattern.Behaviour;

public sealed class DebitoLiberado : IComportamentoDebito
{
    public void Debitar(Conta conta, decimal valor)
    {
        conta.AplicarSaida(valor);
        Console.WriteLine($"débito R$ {valor:F2} — saldo: R$ {conta.Saldo:F2}");
    }
}
