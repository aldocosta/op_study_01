using ApresentacaoDotNet.Antipattern;

/// <summary>
/// Demo do antipattern: herança para reusar código — espelho do CoolPattern.
/// </summary>
public static class BadPattern
{
    public static void Run()
    {
        Console.WriteLine("=== BadPattern: herança forçando comportamento errado ===");
        Console.WriteLine();

        // ---------------------------------------------------------------------------
        // 1) ContaCorrente — domínio fecha
        // ---------------------------------------------------------------------------
        Console.WriteLine("--- 1) ContaCorrente ---");
        var corrente = new ContaCorrente(saldoInicial: 500m);
        corrente.Depositar(100m);
        corrente.Debitar(40m);
        corrente.PagarComCartao(60m);
        Console.WriteLine($"Saldo corrente: R$ {corrente.Saldo:F2}");
        Console.WriteLine();

        // ---------------------------------------------------------------------------
        // 2) ContaPoupanca : ContaCorrente — herança "parece ok"
        // ---------------------------------------------------------------------------
        Console.WriteLine("--- 2) ContaPoupanca : ContaCorrente ---");
        var poupanca = new ContaPoupanca(saldoInicial: 1_000m);
        poupanca.Depositar(200m);
        poupanca.Debitar(50m);
        poupanca.Render(0.5m);

        Console.WriteLine("Herança libera PagarComCartao — e o cartão DEBITA de verdade:");
        poupanca.PagarComCartao(20m);
        Console.WriteLine($"Saldo poupança: R$ {poupanca.Saldo:F2}");
        Console.WriteLine();

        // ---------------------------------------------------------------------------
        // 3) ContaInvestimento : ContaCorrente — mesmo erro, óbvio
        // ---------------------------------------------------------------------------
        Console.WriteLine("--- 3) ContaInvestimento : ContaCorrente ---");
        var investimento = new ContaInvestimento("CDB Liquidez", saldoInicial: 1_000m);
        investimento.Depositar(200m);
        investimento.Render(1.5m);

        Console.WriteLine("Cartão e débito herdados — domínio errado, código aceita:");
        investimento.PagarComCartao(50m);
        investimento.Debitar(30m);
        Console.WriteLine($"Saldo investimento: R$ {investimento.Saldo:F2}");
        Console.WriteLine();

        // ---------------------------------------------------------------------------
        // 4) Gambiarra típica: typeof para "proteger" o método errado
        // ---------------------------------------------------------------------------
        Console.WriteLine("--- 4) Lote de PagarComCartao (investimento NÃO deveria entrar) ---");
        ContaCorrente[] contas = [corrente, poupanca, investimento];

        // A API diz que todas são ContaCorrente → todas "podem" pagar com cartão.
        // Sem design certo, o time inventa filtro por tipo:
        foreach (var conta in contas)
        {
            if (conta.GetType() == typeof(ContaInvestimento))
            {
                Console.WriteLine($"[gambiarra] pulando {conta.GetType().Name} — não deveria ter cartão");
                continue;
            }

            conta.PagarComCartao(10m);
        }

        // E a poupança? Também não deveria ter cartão — mas passou no if.
        // Próximo bug: mais um typeof. Hierarquia podre vira switch de tipos.

        // ---------------------------------------------------------------------------
        // 5) ContaBolsao — "corrigindo" com override que nega (Refused Bequest)
        // ---------------------------------------------------------------------------
        Console.WriteLine();
        Console.WriteLine("--- 5) ContaBolsao : ContaCorrente (tentativa de correção) ---");
        var bolsao = new ContaBolsao(saldoInicial: 300m);
        bolsao.Depositar(100m);
        bolsao.Debitar(50m);
        bolsao.PagarComCartao(20m); // negado — mas exigiu OVERRIDE na subclasse
        Console.WriteLine($"Saldo bolsão: R$ {bolsao.Saldo:F2}");

        Console.WriteLine();
        Console.WriteLine("Se ContaPoupanca e ContaInvestimento também 'corrigirem' PagarComCartao");
        Console.WriteLine("assim (cada uma com seu próprio Console.WriteLine), trocar o canal de saída");
        Console.WriteLine("(ex.: Console.WriteLine -> Objeto.Log) exige editar 3 classes diferentes");
        Console.WriteLine("para uma única intenção de mudança. Isso é Shotgun Surgery.");

        Console.WriteLine();
        Console.WriteLine("Conclusão: herança vazou o método; typeof no foreach é sintoma, não cura.");
        Console.WriteLine("Cada novo tipo 'especial' = mais gambiarra. Veja o CoolPattern.");
    }
}
