namespace ApresentacaoDotNet.Antipattern;

/// <summary>
/// Antipattern proposital:
/// a classe base concentra comportamentos que não são comuns a todas as contas.
/// Resultado: classes que não deveriam ter cartão/débito acabam herdando isso.
/// </summary>
public abstract class Conta
{
    public decimal Saldo { get; protected set; }

    public Conta(decimal saldoInicial = 0) => Saldo = saldoInicial;

    protected virtual string Rotulo => "Conta";

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

    // Erro de modelagem no antipattern: método não é universal para todas as contas.
    public virtual void PagarComCartao(decimal valor)
    {
        Debitar(valor);
        Console.WriteLine($"[{Rotulo}] cartão débito R$ {valor:F2}");
    }
}
