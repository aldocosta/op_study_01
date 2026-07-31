namespace ApresentacaoDotNet.Antipattern;

/// <summary>
/// Está no mesmo nível da corrente (filha de Conta).
/// FALHA gritante: Debitar e PagarComCartao existem e funcionam.
/// Pior: a classe base expôs capacidades que investimento não deveria ter.
/// </summary>
public class ContaInvestimento : Conta
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
