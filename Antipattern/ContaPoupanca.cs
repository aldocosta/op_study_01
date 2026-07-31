namespace ApresentacaoDotNet.Antipattern;

/// <summary>
/// Está no mesmo nível da corrente (filha de Conta).
/// GANHO aparente: menos código.
/// FALHA: PagarComCartao vem junto e executa de verdade.
/// </summary>
public class ContaPoupanca : Conta
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
