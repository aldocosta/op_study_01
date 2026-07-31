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

        Conta bolsao = new ContaBolsao(saldoInicial: 300m);

        Console.WriteLine("--- 4) Lote de PagarComCartao (sem typeof) ---");
        Conta[] contas = [corrente, poupanca, investimento, bolsao];
        foreach (var conta in contas)
        {
            conta.Exibir();
            conta.PagarComCartao(10m);
        }

        Console.WriteLine();


bolsao.SetComportamentoDebito(new DebitoLiberado());

bolsao.Exibir();
bolsao.Depositar(100m);
bolsao.RealizarDebito(50m);
bolsao.PagarComCartao(20m);
Console.WriteLine($"Saldo: R$ {bolsao.Saldo:F2}");
Console.WriteLine();

        // ---------------------------------------------------------------------------
        // 5) ContaBolsao — reaproveitando comportamentos já existentes
        // ---------------------------------------------------------------------------
        Console.WriteLine("--- 5) ContaBolsao (reaproveita DebitoLiberado + SemCartao) ---");
        bolsao.Exibir();
        bolsao.Depositar(100m);
        bolsao.RealizarDebito(50m);
        bolsao.PagarComCartao(20m); // SemCartao — mesma classe usada por poupança/investimento
        Console.WriteLine($"Saldo: R$ {bolsao.Saldo:F2}");

        Console.WriteLine();
        Console.WriteLine("Nenhuma classe de comportamento nova, nenhum override para negar.");
        Console.WriteLine("Trocar o canal de saída (ex.: Console.WriteLine -> Objeto.Log) exige");
        Console.WriteLine("editar 1 arquivo só: SemCartao.cs. Todo mundo que a usa ganha de graça.");
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
