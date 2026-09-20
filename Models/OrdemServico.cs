namespace ABMTech.Api.Models;

public enum StatusManutencao
{
    Agendado,
    AguardandoAprovacao,
    Aprovado,
    EmAndamento,
    TestesFinalizados,
    ProntoParaRetirada,
    Entregue,
    Cancelado
}

public class OrdemServico
{
    public int Id { get; set; }
    public string CodigoRastreio { get; set; } = string.Empty; // ex: "ABM-8492"
    public string NomeCliente { get; set; } = string.Empty;
    public string WhatsappCliente { get; set; } = string.Empty;
    public string ModeloAparelho { get; set; } = string.Empty; // ex: "Samsung A35"
    public string DefeitoRelatado { get; set; } = string.Empty;
    
    // Dados de Orçamento
    public decimal PrecoPecaFornecedor { get; set; }
    public decimal ValorMaoDeObra { get; set; }
    public decimal MargemLucroPecaPorcentagem { get; set; } = 0.20m; // Ex: 20%
    public decimal ValorTotalCalculado { get; set; }

    public StatusManutencao Status { get; set; } = StatusManutencao.Agendado;
    public DateTime DataCriacao { get; set; } = DateTime.Now;
}