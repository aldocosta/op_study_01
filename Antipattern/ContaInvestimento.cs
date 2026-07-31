namespace ApresentacaoDotNet.Antipattern;

/// <summary>
/// Mesma decisão da poupança: herdar ContaCorrente para reusar saldo.
/// FALHA gritante: Debitar e PagarComCartao existem e funcionam.
/// Pior: ContaInvestimento É ContaCorrente para o compilador (LSP mentindo).
/// </summary>
public class ContaInvestimento : ContaCorrente
{
    public string Produto { get; }

    public ContaInvestimento(string produto, decimal saldoInicial = 0) : base(saldoInicial)
    {
        Produto = produto;
    }

    protected override string Rotulo => $"Investimento:{Produto}";

    public void Render(decimal percentual)
    {
        if (percentual <= 0)
            throw new ArgumentOutOfRangeException(nameof(percentual));

        var rendimento = Saldo * (percentual / 100m);
        Saldo += rendimento;
        Console.WriteLine($"[{Rotulo}] rendeu R$ {rendimento:F2} — saldo: R$ {Saldo:F2}");
    }

}
