namespace ApresentacaoDotNet.Antipattern;

/// <summary>
/// Conta corrente no mesmo nível das demais contas.
/// No antipattern, ela herda de uma base que já traz cartão/débito para todos.
/// </summary>
public class ContaCorrente : Conta
{
    public ContaCorrente(decimal saldoInicial = 0) : base(saldoInicial) { }

    protected override string Rotulo => "Corrente";
}
