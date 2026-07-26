using ApresentacaoDotNet.CoolPattern.Entidades;

namespace ApresentacaoDotNet.CoolPattern;

// Família de algoritmos de cartão (Strategy).
public interface IComportamentoCartao
{
    void Pagar(Conta conta, decimal valor);
}
