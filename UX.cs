using static System.Console;

public class UX
{
    private readonly Banco _banco;
    private readonly string _titulo;

    public UX(string titulo, Banco banco)
    {
        _titulo = titulo;
        _banco = banco;
    }

    public void Executar()
    {
        CriarTitulo(_titulo);
        WriteLine(" [1] Criar Conta");
        WriteLine(" [2] Listar Contas");
        WriteLine(" [3] Efetuar Saque");
        WriteLine(" [4] Efetuar Depósito");
        WriteLine(" [5] Aumentar Limite");
        WriteLine(" [6] Diminuir Limite");
        ForegroundColor = ConsoleColor.Red;
        WriteLine("\n [9] Sair");
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        Write(" Digite a opção desejada: ");
        var opcao = ReadLine() ?? "";
        ForegroundColor = ConsoleColor.White;

        switch (opcao)
        {
            case "1": CriarConta(); break;
            case "2": MenuListarContas(); break;
            case "3": EfetuarSaque(); break;
            case "4": EfetuarDeposito(); break;
            case "5": AumentarLimite(); break;
            case "6": DiminuirLimite(); break;
        }

        if (opcao != "9")
        {
            Executar();
        }
        else
        {

            _banco.SaveContas();
        }
    }

    private void CriarConta()
    {
        CriarTitulo(_titulo + " - Criar Conta");
        Write(" Numero:  ");
        var numero = Convert.ToInt32(ReadLine());
        Write(" Cliente: ");
        var cliente = ReadLine() ?? "";
        Write(" CPF:     ");
        var cpf = ReadLine() ?? "";
        Write(" Senha:   ");
        var senha = ReadLine() ?? "";
        Write(" Limite:  ");
        var limite = Convert.ToDecimal(ReadLine());

        var conta = new Conta(numero, cliente, cpf, senha, limite);
        _banco.Contas.Add(conta);

        _banco.SaveContas();

        CriarRodape("Conta criada com sucesso!");
    }

    private void MenuListarContas()
    {
        CriarTitulo(_titulo + " - Listar Contas");
        foreach (var conta in _banco.Contas)
        {
            ExibirDadosConta(conta);
        }
        CriarRodape();
    }

    private void EfetuarSaque()
    {
        CriarTitulo(_titulo + " - Saque");
        var conta = BuscarConta();

        if (conta != null)
        {
            Write(" Digite a Senha: ");
            var senha = ReadLine();

            if (conta.Senha == senha)
            {
                Write(" Valor do Saque: ");
                var valor = Convert.ToDecimal(ReadLine());

                if (valor > 0 && valor <= conta.SaldoDisponível)
                {
                    conta.Saldo -= valor;
                    _banco.SaveContas();
                    CriarRodape($"Saque de {valor:C} realizado com sucesso!");
                }
                else
                {
                    CriarRodape("Saldo insuficiente ou valor inválido!");
                }
            }
            else
            {
                CriarRodape("Senha incorreta!");
            }
        }
    }

    private void EfetuarDeposito()
    {
        CriarTitulo(_titulo + " - Depósito");
        var conta = BuscarConta();

        if (conta != null)
        {
            Write(" Valor do Depósito: ");
            var valor = Convert.ToDecimal(ReadLine());

            if (valor > 0)
            {
                conta.Saldo += valor;
                _banco.SaveContas();
                CriarRodape($"Depósito de {valor:C} realizado com sucesso!");
            }
            else
            {
                CriarRodape("Valor de depósito inválido!");
            }
        }
    }

    private void AumentarLimite()
    {
        CriarTitulo(_titulo + " - Aumentar Limite");
        var conta = BuscarConta();

        if (conta != null)
        {
            Write(" Digite a Senha: ");
            var senha = ReadLine();

            if (conta.Senha == senha)
            {
                Write(" Valor para aumentar o limite: ");
                var valor = Convert.ToDecimal(ReadLine());

                if (valor > 0)
                {
                    conta.Limite += valor;
                    _banco.SaveContas();
                    CriarRodape($"Limite aumentado em {valor:C}!");
                }
                else
                {
                    CriarRodape("Valor inválido!");
                }
            }
            else
            {
                CriarRodape("Senha incorreta!");
            }
        }
    }

    private void DiminuirLimite()
    {
        CriarTitulo(_titulo + " - Diminuir Limite");
        var conta = BuscarConta();

        if (conta != null)
        {
            Write(" Digite a Senha: ");
            var senha = ReadLine();

            if (conta.Senha == senha)
            {
                Write(" Valor para diminuir do limite: ");
                var valor = Convert.ToDecimal(ReadLine());

                if (valor > 0)
                {
                    if (conta.Limite - valor < 0)
                    {
                        CriarRodape("Não é possível reduzir o limite abaixo de zero.");
                    }
                    else
                    {
                        conta.Limite -= valor;
                        _banco.SaveContas();
                        CriarRodape($"Limite reduzido em {valor:C}!");
                    }
                }
                else
                {
                    CriarRodape("Valor inválido!");
                }
            }
            else
            {
                CriarRodape("Senha incorreta!");
            }
        }
    }

    private Conta? BuscarConta()
    {
        Write(" Digite o número da conta: ");
       
        int.TryParse(ReadLine(), out int numero);

        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numero);

        if (conta == null)
        {
            CriarRodape("Conta não encontrada!");
            return null;
        }

        WriteLine($" > Conta encontrada: {conta.Cliente} (Saldo: {conta.Saldo:C})");
        return conta;
    }

    private void ExibirDadosConta(Conta conta)
    {
        WriteLine($" Conta: {conta.Numero} - {conta.Cliente}");
        WriteLine($" Saldo: {conta.Saldo:C} | Limite: {conta.Limite:C}");
        WriteLine($" Saldo Disponível: {conta.SaldoDisponível:C}\n");
    }

    private void CriarLinha()
    {
        WriteLine("-------------------------------------------------");
    }

    private void CriarTitulo(string titulo)
    {
        Clear();
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(" " + titulo);
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
    }

    private void CriarRodape(string? mensagem = null)
    {
        CriarLinha();
        ForegroundColor = ConsoleColor.Green;
        if (mensagem != null)
            WriteLine(" " + mensagem);
        Write(" ENTER para continuar");
        ForegroundColor = ConsoleColor.White;
        ReadLine();
    }
}