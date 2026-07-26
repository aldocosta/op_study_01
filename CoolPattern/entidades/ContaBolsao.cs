using ApresentacaoDotNet.CoolPattern.Behaviour;

namespace ApresentacaoDotNet.CoolPattern.Entidades;

// Só deposita e debita — sem cartão.
// Nenhuma classe nova de comportamento: reaproveita DebitoLiberado e SemCartao.
public sealed class ContaBolsao : Conta
{
    public ContaBolsao(decimal saldoInicial = 0) : base(saldoInicial)
    {
        comportamentoDebito = new DebitoLiberado();
        comportamentoCartao = new SemCartao();
    }

    public override void Exibir() =>
        Console.WriteLine("Sou uma Conta Bolsão");
}
