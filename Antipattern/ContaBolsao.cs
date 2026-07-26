namespace ApresentacaoDotNet.Antipattern;

/// <summary>
/// Só deveria Depositar e Debitar — sem cartão.
/// "Correção" ingênua: sobrescrever PagarComCartao para negar.
/// Isso é Refused Bequest — herdou um membro só para recusá-lo.
/// Se cada conta "sem cartão" fizer o mesmo (com sua própria mensagem),
/// trocar o canal de saída (ex.: Console.WriteLine -> Objeto.Log) vira
/// Shotgun Surgery: uma intenção de mudança, N classes para editar.
/// </summary>
public class ContaBolsao : ContaCorrente
{
    public ContaBolsao(decimal saldoInicial = 0) : base(saldoInicial) { }

    protected override string Rotulo => "Bolsão";

    public override void PagarComCartao(decimal valor) =>
        Console.WriteLine($"[{Rotulo}] Conta Bolsão não faz pagamento de cartão");
}
