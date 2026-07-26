using ApresentacaoDotNet.CoolPattern.Entidades;

namespace ApresentacaoDotNet.CoolPattern;

// Família de algoritmos de débito (Strategy).
public interface IComportamentoDebito
{
    void Debitar(Conta conta, decimal valor);
}
