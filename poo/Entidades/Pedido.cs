namespace PooPedidos.Entidades;

public class Pedido
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public List<ItemPedido> Itens { get; set; } = [];

    public decimal ValorTotal => Itens.Sum(item => item.ValorTotal);

    public override string ToString() =>
        $"{Id} - {Data:dd/MM/yyyy} - {Cliente.Nome} - {ValorTotal:C}";
}
