using ApresentacaoDotNet.CoolPattern.Behaviour;
using ApresentacaoDotNet.CoolPattern.Entidades;

/// <summary>
/// Mini-simulador no estilo do Strategy: cria contas, chama perform*, exibe.
/// </summary>
public static class CoolPattern
{
    public static void Run()
    {
        Console.WriteLine("=== CoolPattern: Strategy (Conta TEM comportamentos) ===");
        Console.WriteLine();

        Console.WriteLine("--- 1) ContaCorrente ---");
        Conta corrente = new ContaCorrente(saldoInicial: 500m);
        corrente.Exibir();
        corrente.Depositar(100m);
        corrente.RealizarDebito(40m);
        corrente.PagarComCartao(60m);
        Console.WriteLine($"Saldo: R$ {corrente.Saldo:F2}");
        Console.WriteLine();

        Console.WriteLine("--- 2) ContaPoupanca ---");
        Conta poupanca = new ContaPoupanca(saldoInicial: 1_000m);
        poupanca.Exibir();
        poupanca.Depositar(200m);
        poupanca.RealizarDebito(50m);
        poupanca.PagarComCartao(20m); // SemCartao
        Console.WriteLine($"Saldo: R$ {poupanca.Saldo:F2}");
        Console.WriteLine();

        Console.WriteLine("--- 3) ContaInvestimento ---");
        Conta investimento = new ContaInvestimento("CDB Liquidez", saldoInicial: 1_000m);
        investimento.Exibir();
        investimento.Depositar(200m);
        investimento.PagarComCartao(50m); // SemCartao
        investimento.RealizarDebito(30m); // DebitoNegado
        Console.WriteLine($"Saldo: R$ {investimento.Saldo:F2}");
        Console.WriteLine();

        Console.WriteLine("--- 4) Lote de PagarComCartao (sem typeof) ---");
        Conta[] contas = [corrente, poupanca, investimento];
        foreach (var conta in contas)
        {
            conta.Exibir();
            conta.PagarComCartao(10m);
        }

        Console.WriteLine();
        Console.WriteLine("Conclusão: Conta compõe e delega comportamentos — sem herdar ContaCorrente.");

        // =========================================================================
        // MELHORIA (descomentar com Set* em Conta.cs):
        // =========================================================================
        // Console.WriteLine();
        // Console.WriteLine("--- 5) SetComportamento* em runtime ---");
        // investimento.SetComportamentoDebito(new DebitoLiberado());
        // investimento.RealizarDebito(15m);
        //
        // corrente.SetComportamentoCartao(new SemCartao());
        // corrente.PagarComCartao(10m);
    }
}
