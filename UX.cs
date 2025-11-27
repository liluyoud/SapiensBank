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
            case "3": MenuSacar(); break;
            case "4": MenuDepositar(); break;
            case "5": MenuAumentarLimite(); break;
            case "6": MenuDiminuirLimite(); break;
        }

        if (opcao != "9")
        {
            Executar();
        }

        _banco.SaveContas();
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

        CriarRodape("Conta criada com sucesso!");
    }

    private void MenuListarContas()
    {
        CriarTitulo(_titulo + " - Listar Contas");

        foreach (var conta in _banco.Contas)
        {
            WriteLine($" Conta: {conta.Numero} - {conta.Cliente}");
            WriteLine($" Saldo: {conta.Saldo:C} | Limite: {conta.Limite:C}");
            WriteLine($" Saldo Disponível: {conta.SaldoDisponível:C}\n");
        }

        CriarRodape();
    }

    private void MenuSacar()
    {
        CriarTitulo(_titulo + " - Saque");

        var conta = BuscarContaPorNumeroESenha();
        if (conta == null)
        {
            CriarRodape("Conta não encontrada ou senha inválida.");
            return;
        }

        Write(" Valor do saque: ");
        decimal valor = Convert.ToDecimal(ReadLine());

        if (valor <= 0)
        {
            CriarRodape("Valor inválido.");
            return;
        }

        if (conta.SaldoDisponível < valor)
        {
            CriarRodape("Saldo insuficiente.");
            return;
        }

        conta.Saldo -= valor;
        CriarRodape("Saque realizado com sucesso!");
    }

    private void MenuDepositar()
    {
        CriarTitulo(_titulo + " - Depósito");

        Write(" Número da conta: ");
        int numero = Convert.ToInt32(ReadLine());
        var conta = _banco.Contas.FirstOrDefault(c => c.Numero == numero);

        if (conta == null)
        {
            CriarRodape("Conta não encontrada.");
            return;
        }

        Write(" Valor do depósito: ");
        decimal valor = Convert.ToDecimal(ReadLine());

        if (valor <= 0)
        {
            CriarRodape("Valor inválido.");
            return;
        }

        conta.Saldo += valor;
        CriarRodape("Depósito realizado!");
    }

    private void MenuAumentarLimite()
    {
        CriarTitulo(_titulo + " - Aumentar Limite");

        var conta = BuscarContaPorNumeroESenha();
        if (conta == null)
        {
            CriarRodape("Conta ou senha inválida.");
            return;
        }

        Write(" Valor para aumentar o limite: ");
        decimal valor = Convert.ToDecimal(ReadLine());

        if (valor <= 0)
        {
            CriarRodape("Valor inválido.");
            return;
        }

        conta.Limite += valor;
        CriarRodape("Limite aumentado com sucesso!");
    }

    private void MenuDiminuirLimite()
    {
        CriarTitulo(_titulo + " - Diminuir Limite");

        var conta = BuscarContaPorNumeroESenha();
        if (conta == null)
        {
            CriarRodape("Conta ou senha inválida.");
            return;
        }

        Write(" Valor para diminuir o limite: ");
        decimal valor = Convert.ToDecimal(ReadLine());

        if (valor <= 0 || valor > conta.Limite)
        {
            CriarRodape("Valor inválido.");
            return;
        }

        conta.Limite -= valor;
        CriarRodape("Limite reduzido com sucesso!");
    }

    private Conta? BuscarContaPorNumeroESenha()
    {
        Write(" Número da conta: ");
        int numero = Convert.ToInt32(ReadLine());

        Write(" Senha: ");
        string senha = ReadLine() ?? "";

        return _banco.Contas
            .FirstOrDefault(c => c.Numero == numero && c.Senha == senha);
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
