using ApresentacaoDotNet.CoolPattern.Entidades;

namespace ApresentacaoDotNet.CoolPattern.Behaviour;

public sealed class DebitoNegado : IComportamentoDebito
{
    public void Debitar(Conta conta, decimal valor)
    {
        Console.WriteLine($"débito NEGADO (R$ {valor:F2}) — esta conta não debita");
    }
}
