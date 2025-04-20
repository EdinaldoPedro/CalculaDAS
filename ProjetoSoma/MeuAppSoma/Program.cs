using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Pergunta a data de abertura
        Console.Write("Digite a data de abertura da PJ (DD/MM/YYYY, DD.MM.YYYY, DDMMYYYY, DD-MM-YYYY ou DD,MM,YYYY): ");
        string inputData = Console.ReadLine();

        DateTime dataAbertura = ObterDataValida(inputData);

        if (dataAbertura != DateTime.MinValue)
        {
            // Criação da instância de Empresa
            Empresa empresa = new Empresa(dataAbertura);
            empresa.CalcularRBT();

            double totalFaturamento = empresa.CalcularTotalRBT();

            Console.WriteLine($"\nTratativa proporcional. O valor do RBT (Soma ajustada) é: R$ {totalFaturamento:F2}");

            // Perguntar o anexo para o cálculo do DAS
            int anexoEscolhido = ObterAnexoDAS();

            // Solicitar faturamento mensal
            double faturamentoMensal = ObterFaturamentoMensal();

            // Calcular o DAS
            DASCalculator dasCalculator = new DASCalculator();
            var resultado = dasCalculator.CalcularDAS(anexoEscolhido, (decimal)faturamentoMensal, totalFaturamento);

            Console.WriteLine($"O valor do DAS a pagar é: R$ {resultado.das:F2}");
            Console.WriteLine($"Alíquota Geral: {resultado.aliquotaGeral:F2}%");

            // Exibir detalhamento dos impostos
            Console.WriteLine("\n=== Detalhamento dos Impostos ===");
            Console.WriteLine(resultado.detalhamento);
        }
        else
        {
            Console.WriteLine("Data inválida. Certifique-se de usar um dos formatos válidos.");
        }
    }

    // Método para obter a data válida
    static DateTime ObterDataValida(string inputData)
    {
        string[] formatos = { "dd/MM/yyyy", "dd.MM.yyyy", "ddMMyyyy", "dd-MM-yyyy", "dd,MM,yyyy" };
        DateTime dataAbertura = DateTime.MinValue;

        foreach (var formato in formatos)
        {
            if (DateTime.TryParseExact(inputData, formato, null, System.Globalization.DateTimeStyles.None, out dataAbertura))
            {
                return dataAbertura;
            }
        }

        return DateTime.MinValue; // Retorna um valor inválido se a data não for reconhecida
    }

    // Método para obter o anexo escolhido para o cálculo do DAS
    static int ObterAnexoDAS()
    {
        Console.WriteLine("Escolha o anexo para o cálculo do DAS:");
        Console.WriteLine("1 - Anexo III");
        Console.WriteLine("2 - Anexo IV");
        Console.WriteLine("3 - Anexo V");

        int anexoEscolhido;
        while (!int.TryParse(Console.ReadLine(), out anexoEscolhido) || anexoEscolhido < 1 || anexoEscolhido > 3)
        {
            Console.WriteLine("Opção inválida. Por favor, escolha 1, 2 ou 3.");
        }

        return anexoEscolhido;
    }

    // Método para obter o faturamento mensal
    static double ObterFaturamentoMensal()
    {
        Console.Write("Digite o faturamento mensal: ");
        double faturamentoMensal;

        while (!double.TryParse(Console.ReadLine(), out faturamentoMensal) || faturamentoMensal < 0)
        {
            Console.Write("Valor inválido. Por favor, digite um valor válido para o faturamento mensal: ");
        }

        return faturamentoMensal;
    }
}
