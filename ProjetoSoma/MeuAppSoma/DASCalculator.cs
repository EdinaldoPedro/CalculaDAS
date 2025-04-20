using System;

public class DASCalculator
{
    // Método para calcular a alíquota proporcional para os anexos
    private decimal CalcularAliquotaProporcional(double rbt, double[] faixasRBT, decimal[] aliquotas)
    {
        // Se o RBT for até 180.000, a alíquota é fixa
        if (rbt <= faixasRBT[0])
        {
            return aliquotas[0];
        }

        // Verificar em qual faixa o RBT se encaixa
        for (int i = 0; i < faixasRBT.Length - 1; i++)
        {
            if (rbt > faixasRBT[i] && rbt <= faixasRBT[i + 1])
            {
                // Cálculo proporcional da alíquota
                decimal aliquotaInferior = aliquotas[i];
                decimal aliquotaSuperior = aliquotas[i + 1];

                double diferencaRBT = rbt - faixasRBT[i];
                double diferencaAliquota = (double)(aliquotaSuperior - aliquotaInferior);

                decimal aliquotaProporcional = aliquotaInferior + 
                    (decimal)(diferencaRBT / (faixasRBT[i + 1] - faixasRBT[i]) * diferencaAliquota);
                return aliquotaProporcional;
            }
        }

        // Caso o RBT seja maior que a última faixa, retornamos a maior alíquota
        return aliquotas[faixasRBT.Length - 1];
    }

    // Método para calcular e detalhar os impostos no DAS
    public (decimal das, decimal aliquotaGeral, string detalhamento) CalcularDAS(int anexoEscolhido, decimal faturamentoMensal, double rbt)
    {
        decimal aliquota = 0;

        // Faixas e alíquotas para cada anexo
        double[] faixasRBT = { 180000, 360000, 720000, 1800000, 3600000 };
        decimal[] aliquotasAnexoIII = { 6m, 7.3m, 8.3m, 10.3m, 11.3m };
        decimal[] aliquotasAnexoIV = { 4.5m, 6m, 7m, 9.5m, 10m };
        decimal[] aliquotasAnexoV = { 15.5m, 16.5m, 17.5m, 19.5m, 20m };

        // Percentuais de distribuição de impostos
        decimal[] impostosAnexoIII = { 32m, 0.65m, 3m, 1.2m, 1m, 62.15m };
        decimal[] impostosAnexoIV = { 33m, 0.5m, 2m, 4m, 1.5m, 59m }; // Total: 100%
        decimal[] impostosAnexoV = { 33m, 0.5m, 2m, 15m, 12.5m, 37m }; // Total: 100%

        decimal[] distribuicaoImpostos = null;

        // Seleciona a alíquota e a distribuição de impostos
        switch (anexoEscolhido)
        {
            case 1: // Anexo III
                aliquota = CalcularAliquotaProporcional(rbt, faixasRBT, aliquotasAnexoIII);
                distribuicaoImpostos = impostosAnexoIII;
                break;
            case 2: // Anexo IV
                aliquota = CalcularAliquotaProporcional(rbt, faixasRBT, aliquotasAnexoIV);
                distribuicaoImpostos = impostosAnexoIV;
                break;
            case 3: // Anexo V
                aliquota = CalcularAliquotaProporcional(rbt, faixasRBT, aliquotasAnexoV);
                distribuicaoImpostos = impostosAnexoV;
                break;
            default:
                throw new ArgumentException("Anexo inválido.");
        }

        // Calcula o valor total do DAS
        decimal das = faturamentoMensal * aliquota / 100;

        // Detalha cada imposto
        string detalhamento = "Detalhamento dos impostos:\n";
        string[] nomesImpostos = { "CPP", "PIS", "COFINS", "IRPJ", "CSLL", "ISS" };

        for (int i = 0; i < distribuicaoImpostos.Length; i++)
        {
            decimal valorImposto = das * distribuicaoImpostos[i] / 100;
            detalhamento += $"{nomesImpostos[i]} ({distribuicaoImpostos[i]}%): R$ {valorImposto:F2}\n";
        }

        return (das, aliquota, detalhamento);
    }
}
