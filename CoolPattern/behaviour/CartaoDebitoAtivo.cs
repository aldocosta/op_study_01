using ApresentacaoDotNet.CoolPattern.Entidades;

namespace ApresentacaoDotNet.CoolPattern.Behaviour;

public sealed class CartaoDebitoAtivo : IComportamentoCartao
{
    public void Pagar(Conta conta, decimal valor)
    {
        conta.AplicarSaida(valor);
        Console.WriteLine($"cartão débito R$ {valor:F2} — saldo: R$ {conta.Saldo:F2}");
    }
}
