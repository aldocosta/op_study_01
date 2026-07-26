namespace ApresentacaoDotNet.Antipattern;

/// <summary>
/// Herda ContaCorrente só para reusar saldo/depósito/saque.
/// GANHO aparente: menos código.
/// FALHA: PagarComCartao vem junto e executa de verdade.
/// </summary>
public class ContaPoupanca : ContaCorrente
{
    public ContaPoupanca(decimal saldoInicial = 0) : base(saldoInicial) { }

    protected override string Rotulo => "Poupança";

    public void Render(decimal percentual)
    {
        if (percentual <= 0)
            throw new ArgumentOutOfRangeException(nameof(percentual));

        var juros = Saldo * (percentual / 100m);
        Saldo += juros;
        Console.WriteLine($"[{Rotulo}] juros R$ {juros:F2} — saldo: R$ {Saldo:F2}");
    }
}
