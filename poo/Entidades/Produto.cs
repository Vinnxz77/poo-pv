namespace PooPedidos.Entidades;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }

    public override string ToString() => $"{Id} - {Nome} - {Preco:C}";
}
