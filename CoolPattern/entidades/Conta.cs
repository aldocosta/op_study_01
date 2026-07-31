namespace ApresentacaoDotNet.CoolPattern.Entidades;

/// <summary>
/// Strategy:
///   - Conta TEM comportamentos (composição)
///   - RealizarDebito / PagarComCartao só delegam
///   - Depositar é comum a todas
///   - Exibir é de cada subtipo
/// </summary>
public abstract class Conta
{
    protected IComportamentoDebito comportamentoDebito = null!;
    protected IComportamentoCartao comportamentoCartao = null!;

    public decimal Saldo { get; private set; }

    protected Conta(decimal saldoInicial = 0) => Saldo = saldoInicial;

    // Cada tipo de conta se apresenta.
    public abstract void Exibir();

    // Comum a todas as contas.
    public void Depositar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(valor));

        Saldo += valor;
        Console.WriteLine($"depósito R$ {valor:F2} — saldo: R$ {Saldo:F2}");
    }

    // Só delega — a Conta não implementa o "como".
    public void RealizarDebito(decimal valor) =>
        comportamentoDebito.Debitar(this, valor);

    public void PagarComCartao(decimal valor) =>
        comportamentoCartao.Pagar(this, valor);

    // =========================================================================
    // MELHORIA (descomentar na apresentação): trocar comportamento em runtime.
    // =========================================================================
     public void SetComportamentoDebito(IComportamentoDebito c) => comportamentoDebito = c;
     public void SetComportamentoCartao(IComportamentoCartao c) => comportamentoCartao = c;

    // Usado pelas strategies para alterar o saldo.
    internal void AplicarSaida(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(valor));
        if (valor > Saldo)
            throw new InvalidOperationException("Saldo insuficiente.");

        Saldo -= valor;
    }

    internal void AplicarEntrada(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(valor));

        Saldo += valor;
    }
}
