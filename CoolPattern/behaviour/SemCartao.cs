using ApresentacaoDotNet.CoolPattern.Entidades;

namespace ApresentacaoDotNet.CoolPattern.Behaviour;

public sealed class SemCartao : IComportamentoCartao
{
    public void Pagar(Conta conta, decimal valor)
    {
        Console.WriteLine($"cartão NEGADO (R$ {valor:F2}) — esta conta não tem cartão");
    }
}
