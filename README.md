# Herança nem sempre é a solução

Projeto de apoio para uma apresentação sobre um problema clássico de orientação a objetos:
usar **herança para reusar código** pode fazer uma subclasse herdar comportamentos da
superclasse que **não são aderentes ao seu domínio**, quebrando o [Princípio da Substituição
de Liskov (LSP)](https://en.wikipedia.org/wiki/Liskov_substitution_principle) e gerando alta
manutenção (gambiarras do tipo `typeof`/`switch` para "esconder" o que vazou).

O repositório contém **dois exemplos lado a lado**, com o mesmo cenário de domínio (contas
bancárias), implementados de duas formas:

| | Abordagem | Pasta/arquivo |
|---|---|---|
| ❌ Antipattern | Herança usada só para reusar implementação | `Antipattern/` + `BadPattern.cs` |
| ✅ Padrão recomendado | Composição via **Strategy** | `CoolPattern/` + `CoolPattern.cs` |

## O cenário

Existem três tipos de conta:

- **ContaCorrente** — pode debitar e pagar com cartão de débito.
- **ContaPoupança** — pode debitar, **mas não tem cartão**.
- **ContaInvestimento** — **não debita** e **não tem cartão** (só permite depósito e resgate/rendimento).

## ❌ `Antipattern` / `BadPattern.cs` — herança forçando comportamento errado

`ContaPoupanca` e `ContaInvestimento` **herdam** de `ContaCorrente` só para reaproveitar
saldo, depósito e débito:

```csharp
public class ContaInvestimento : ContaCorrente
```

O problema: `PagarComCartao` (e, no caso do investimento, também `Debitar`) são
comportamentos **específicos de conta corrente**, mas como são `public virtual` na
superclasse, **qualquer herdeiro os ganha de graça** — mesmo quando isso viola a regra de
negócio.

Para o compilador, `ContaInvestimento` **é** uma `ContaCorrente` (relação `is-a`), então o
código passa a aceitar chamadas que não deveriam existir:

```csharp
investimento.PagarComCartao(50m); // não deveria existir, mas compila e executa
investimento.Debitar(30m);        // idem
```

Isso força o time a "proteger" o código na unha, verificando o tipo em tempo de execução
(`typeof`), o que é sintoma de um problema de design, não uma correção real — e ainda assim
`ContaPoupanca` continua passando despercebida no filtro:

```csharp
foreach (var conta in contas)
{
    if (conta.GetType() == typeof(ContaInvestimento))
    {
        continue; // gambiarra: cada novo tipo "especial" exige mais um if
    }

    conta.PagarComCartao(10m);
}
```

**Conclusão do exemplo:** herança vazou o método; `typeof` no `foreach` é sintoma, não cura.

## ✅ `CoolPattern` / `CoolPattern.cs` — composição com Strategy

Aqui a herança só é usada para o que é **genuinamente comum** a todas as contas: saldo e
depósito, na classe abstrata `Conta`. O que **varia** por tipo de conta (débito e cartão)
deixa de ser herdado e passa a ser **composto**:

```csharp
public abstract class Conta
{
    protected IComportamentoDebito comportamentoDebito;
    protected IComportamentoCartao comportamentoCartao;

    public void RealizarDebito(decimal valor) =>
        comportamentoDebito.Debitar(this, valor);

    public void PagarComCartao(decimal valor) =>
        comportamentoCartao.Pagar(this, valor);
}
```

A `Conta` **não sabe** *como* debitar ou pagar — ela só sabe **que** existe algo (uma
`IComportamentoDebito` / `IComportamentoCartao`) que sabe fazer isso. Cada subtipo escolhe,
no construtor, quais comportamentos possui:

```csharp
public sealed class ContaCorrente : Conta
{
    public ContaCorrente(decimal saldoInicial = 0) : base(saldoInicial)
    {
        comportamentoDebito = new DebitoLiberado();
        comportamentoCartao = new CartaoDebitoAtivo();
    }
}

public sealed class ContaPoupanca : Conta
{
    public ContaPoupanca(decimal saldoInicial = 0) : base(saldoInicial)
    {
        comportamentoDebito = new DebitoLiberado();
        comportamentoCartao = new SemCartao();
    }
}

public sealed class ContaInvestimento : Conta
{
    public ContaInvestimento(string produto, decimal saldoInicial = 0) : base(saldoInicial)
    {
        comportamentoDebito = new DebitoNegado();
        comportamentoCartao = new SemCartao();
    }
}
```

Com isso, o `foreach` do lote não precisa de nenhum `typeof`: cada conta simplesmente
responde de acordo com o comportamento que possui.

```csharp
foreach (var conta in contas)
{
    conta.PagarComCartao(10m); // ContaInvestimento e ContaPoupanca respondem "NEGADO" sozinhas
}
```

**Conclusão do exemplo:** a `Conta` compõe e delega comportamentos — sem herdar de
`ContaCorrente`.

### Comportamentos disponíveis (`CoolPattern/behaviour/`)

| Interface | Implementações |
|---|---|
| `IComportamentoDebito` | `DebitoLiberado` (debita de verdade) / `DebitoNegado` (recusa) |
| `IComportamentoCartao` | `CartaoDebitoAtivo` (paga de verdade) / `SemCartao` (recusa) |

Como bônus, `Conta.cs` já deixa comentado um `SetComportamento*`, mostrando que a
composição também permite **trocar o comportamento em runtime** — algo impossível de fazer
de forma limpa com herança estática.

## Extensão: adicionando `ContaBolsao`

Para deixar ainda mais concreto o custo de manutenção da herança, o projeto inclui uma
quarta conta — `ContaBolsao` — que só deve **depositar e debitar**, sem cartão. Ela aparece
nos dois lados (seção 5 de `BadPattern.Run()` e `CoolPattern.Run()`) para mostrar dois
problemas adicionais que aparecem quando um novo tipo de conta chega:

### ❌ Refused Bequest (herança recusada)

Em `Antipattern/ContaBolsao.cs`, para "corrigir" o vazamento de `PagarComCartao`, a única
saída é **sobrescrever o método herdado só para recusá-lo**:

```csharp
public class ContaBolsao : ContaCorrente
{
    public override void PagarComCartao(decimal valor) =>
        Console.WriteLine($"[{Rotulo}] Conta Bolsão não faz pagamento de cartão");
}
```

Isso é o code smell **Refused Bequest** (catálogo de refactoring de Martin Fowler): a
subclasse herda um membro só para negá-lo — sinal de que a hierarquia está errada.

### ❌ Shotgun Surgery (uma mudança, N classes)

Se `ContaPoupanca` e `ContaInvestimento` também precisassem "corrigir" `PagarComCartao` da
mesma forma, cada uma teria sua **própria** mensagem duplicada. Trocar o canal de saída
(por exemplo, `Console.WriteLine` → `Objeto.Log`) exigiria editar **3 classes diferentes**
para uma única intenção de mudança — o smell **Shotgun Surgery**.

### ✅ Reaproveitamento via Strategy

Em `CoolPattern/entidades/ContaBolsao.cs`, a mesma regra de negócio não exige nenhuma
classe ou mensagem nova — só reaproveita comportamentos que já existem:

```csharp
public sealed class ContaBolsao : Conta
{
    public ContaBolsao(decimal saldoInicial = 0) : base(saldoInicial)
    {
        comportamentoDebito = new DebitoLiberado(); // já existe
        comportamentoCartao = new SemCartao();      // já existe
    }
}
```

Trocar o canal de saída da recusa de cartão exige editar **um único arquivo**
(`SemCartao.cs`) — `ContaPoupanca`, `ContaInvestimento` e `ContaBolsao` ganham a mudança de
graça, sem tocar em nenhuma delas.

| | Antipattern (herança) | CoolPattern (composição) |
|---|---|---|
| Novo tipo com subconjunto de comportamentos | Override para negar (Refused Bequest) | Só escolhe as `Strategy`s existentes no construtor |
| Mudar como a recusa é reportada (log, exceção, evento) | Editar N classes que fizeram override | Editar 1 classe de comportamento (`SemCartao`) |
| Risco | Duplicação e esquecimento de algum ponto | Regra centralizada, reaproveitada por quem precisar |

> **Herança duplica a regra em cada classe filha que precisa negá-la; composição centraliza
> a regra numa classe só, reaproveitada por quem precisar dela.**

## Estrutura do projeto

```
.
├── Antipattern/                     # Exemplo ❌ com herança "para reusar"
│   ├── ContaCorrente.cs
│   ├── ContaPoupanca.cs             # : ContaCorrente
│   ├── ContaInvestimento.cs         # : ContaCorrente
│   └── ContaBolsao.cs               # : ContaCorrente — Refused Bequest
├── BadPattern.cs                    # Demo do antipattern
│
├── CoolPattern/                     # Exemplo ✅ com composição (Strategy)
│   ├── entidades/
│   │   ├── Conta.cs                 # Classe base abstrata (saldo/depósito)
│   │   ├── ContaCorrente.cs
│   │   ├── ContaPoupanca.cs
│   │   ├── ContaInvestimento.cs
│   │   └── ContaBolsao.cs           # Reaproveita DebitoLiberado + SemCartao
│   ├── behaviour/
│   │   ├── DebitoLiberado.cs
│   │   ├── DebitoNegado.cs
│   │   ├── CartaoDebitoAtivo.cs
│   │   └── SemCartao.cs
│   ├── IComportamentoDebito.cs
│   └── IComportamentoCartao.cs
├── CoolPattern.cs                   # Demo do padrão recomendado
│
└── Program.cs                       # Ponto de entrada
```

## Como rodar

Requer o [.NET SDK](https://dotnet.microsoft.com/download) instalado.

```bash
dotnet run
```

Por padrão, `Program.cs` executa apenas `BadPattern.Run()`. Para ver o exemplo com
composição, descomente a chamada a `CoolPattern.Run()` em `Program.cs`:

```csharp
// Entrada da apresentação: antipattern primeiro, pattern depois.
BadPattern.Run();
/*
Console.WriteLine();
Console.WriteLine(new string('=', 60));
Console.WriteLine();
*/
CoolPattern.Run(); // <- descomentar
```

## Mensagem principal para a apresentação

> Não herde para reusar implementação variável; componha o comportamento (Strategy) para
> que cada classe declare, explicitamente, o que ela realmente pode fazer.

Em termos de princípios de design:

- **"Herda de" (`is-a`) vs. "tem um" (`has-a`)** — prefira composição quando o `is-a` for
  falso ou obrigar a classe filha a aceitar comportamento que não faz sentido no seu
  domínio.
- **Programe para uma interface, não para uma implementação** — aqui a interface representa
  um *comportamento* (`IComportamentoDebito`, `IComportamentoCartao`), não apenas um objeto
  de domínio.
- **Liskov Substitution Principle** — se a subclasse precisa "desligar" ou "negar" um método
  herdado para se comportar corretamente, a hierarquia provavelmente está errada.

## Ponto central do nosso exemplo

No cenário das contas deste repositório, o critério para decidir quando um comportamento deve
virar interface/Strategy é:

> **Identifique o que muda, separe do que permanece estável e encapsule a variação.**

Aplicando ao nosso domínio:

- **Permanece estável**: conta tem saldo e depósito.
- **Varia por tipo**: débito e pagamento com cartão.
- **Decisão de design**: débito/cartão viram comportamentos (`IComportamentoDebito`,
  `IComportamentoCartao`) compostos na `Conta`, e não métodos herdados de `ContaCorrente`.

Checklist prático aplicado às contas:

- Se o mesmo método vive sendo sobrescrito de formas diferentes, ele é candidato a Strategy.
- Se uma subclasse precisa negar comportamento herdado (`throw`, no-op, mensagem de recusa),
  a hierarquia está pedindo composição.
- Se uma regra muda e você precisa editar várias subclasses, a variação está no lugar errado.
- Se o comportamento pode mudar em runtime, prefira composição com interface a herança fixa.

## Referência

Baseado no exemplo do pato (SimUDuck) do livro **Head First Design Patterns** (Eric
Freeman, Elisabeth Robson, Bert Bates e Kathy Sierra — O'Reilly Media).
