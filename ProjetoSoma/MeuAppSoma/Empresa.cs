using System;

public class Empresa
{
    public DateTime DataAbertura { get; set; }
    public int MesesDeAbertura { get; set; }
    public double[] RBT12 { get; set; }

    public Empresa(DateTime dataAbertura)
    {
        DataAbertura = dataAbertura;
        RBT12 = new double[12];
    }

    // Método para calcular o RBT (Receita Bruta Total)
    public void CalcularRBT()
    {
        MesesDeAbertura = CalcularMesesDeAbertura();

        if (MesesDeAbertura >= 12)
        {
            Console.WriteLine("A empresa está aberta há mais de 12 meses.");
            EscolherFaturamento();
        }
        else
        {
            Console.WriteLine("A empresa está aberta há menos de 12 meses.");
            PreencherRBT12PorMeses();
        }
    }

    // Método para calcular a diferença em meses entre a data de abertura e a data atual
    private int CalcularMesesDeAbertura()
    {
        DateTime dataAtual = DateTime.Now;
        return (dataAtual.Year - DataAbertura.Year) * 12 + dataAtual.Month - DataAbertura.Month;
    }

    // Método para solicitar ao usuário se ele deseja informar faturamento mensal ou total
    private void EscolherFaturamento()
    {
        Console.Write("Você deseja digitar o faturamento mensal dos últimos 12 meses (1) ou o valor total (2)? ");
        string escolha = Console.ReadLine();

        if (escolha == "1")
        {
            PreencherRBT12PorMeses();
        }
        else if (escolha == "2")
        {
            PreencherRBT12ComFaturamentoTotal();
        }
        else
        {
            Console.WriteLine("Opção inválida.");
        }
    }

    // Método para preencher o RBT12 quando o usuário opta por faturamento mensal
    private void PreencherRBT12PorMeses()
    {
        Console.Write("Digite o faturamento do primeiro mês: ");
        double faturamentoMes1 = ObterFaturamento();

        // Preenche os 12 meses com o valor do primeiro mês
        for (int i = 0; i < 12; i++)
        {
            RBT12[i] = faturamentoMes1;
        }

        PreencherFaturamentoRestante();
    }

    // Método para preencher o RBT12 com faturamento total
    private void PreencherRBT12ComFaturamentoTotal()
    {
        Console.Write("Digite o valor total do faturamento dos últimos 12 meses: ");
        double totalFaturamento = ObterFaturamento();
        double valorMensal = totalFaturamento / 12;

        for (int i = 0; i < 12; i++)
        {
            RBT12[i] = valorMensal;
        }

        Console.WriteLine($"\nTratativa normal. O valor do RBT (Total faturado) é: R$ {totalFaturamento:F2}");
    }

    // Método para perguntar os faturamentos dos meses restantes
    private void PreencherFaturamentoRestante()
    {
        for (int i = 1; i < MesesDeAbertura; i++)
        {
            Console.Write($"Digite o faturamento do mês {i + 1}: ");
            double faturamento = ObterFaturamento();
            RBT12[i] = faturamento;
        }

        ExibirTabelaRBT();
    }

    // Método para exibir a tabela RBT12
    private void ExibirTabelaRBT()
    {
        Console.WriteLine("\nTabela RBT12 (mês a mês):");
        for (int i = 0; i < 12; i++)
        {
            Console.WriteLine($"Mês {i + 1}: R$ {RBT12[i]:F2}");
        }
    }

    // Método para obter o faturamento, garantindo que seja um valor válido
    private double ObterFaturamento()
    {
        double faturamento;
        while (!double.TryParse(Console.ReadLine(), out faturamento) || faturamento < 0)
        {
            Console.Write("Valor inválido. Digite um valor válido para o faturamento: ");
        }
        return faturamento;
    }

    // Método para calcular o total de RBT (Soma dos faturamentos)
    public double CalcularTotalRBT()
    {
        double totalFaturamento = 0;
        foreach (var faturamento in RBT12)
        {
            totalFaturamento += faturamento;
        }
        return totalFaturamento;
    }
}
