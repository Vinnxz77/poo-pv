namespace PooPedidos.Entidades;

public class ItemPedido
{
    public Produto Produto { get; set; } = null!;
    public int Qtd { get; set; }
    public decimal Valor { get; set; }

    public decimal ValorTotal => Qtd * Valor;

    public override string ToString() =>
        $"{Produto.Nome} | Qtd: {Qtd} | Unitário: {Valor:C} | Total: {ValorTotal:C}";
}
