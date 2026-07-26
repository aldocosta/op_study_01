namespace ApresentacaoDotNet.Antipattern;

/// <summary>
/// Conta corrente com saldo, depósito, débito e cartão.
/// Problema: usada como base de herança para "reusar saldo".
/// </summary>
public class ContaCorrente
{
    public decimal Saldo { get; protected set; }

    public ContaCorrente(decimal saldoInicial = 0) => Saldo = saldoInicial;

    protected virtual string Rotulo => "Corrente";

    public virtual void Depositar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "Valor deve ser positivo.");

        Saldo += valor;
        Console.WriteLine($"[{Rotulo}] depósito R$ {valor:F2} — saldo: R$ {Saldo:F2}");
    }

    public virtual void Debitar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "Valor deve ser positivo.");

        if (valor > Saldo)
            throw new InvalidOperationException("Saldo insuficiente.");

        Saldo -= valor;
        Console.WriteLine($"[{Rotulo}] débito R$ {valor:F2} — saldo: R$ {Saldo:F2}");
    }

    // Específico de corrente — qualquer herdeiro ganha isso de graça.
    public virtual void PagarComCartao(decimal valor)
    {
        Debitar(valor);
        Console.WriteLine($"[{Rotulo}] cartão débito R$ {valor:F2}");
    }
}
