using System.Globalization;
using PooPedidos.Entidades;

namespace PooPedidos;

public class Aplicacao
{
    private readonly List<Cliente> _clientes = [];
    private readonly List<Produto> _produtos = [];
    private readonly List<Pedido> _pedidos = [];

    private int _proximoClienteId = 1;
    private int _proximoProdutoId = 1;
    private int _proximoPedidoId = 1;

    public void Executar()
    {
        while (true)
        {
            LimparConsole();
            Console.WriteLine("=== SISTEMA DE PEDIDOS ===");
            Console.WriteLine("1 - Clientes");
            Console.WriteLine("2 - Produtos");
            Console.WriteLine("3 - Pedidos");
            Console.WriteLine("0 - Sair");

            switch (LerInteiro("Opção: "))
            {
                case 1: MenuClientes(); break;
                case 2: MenuProdutos(); break;
                case 3: MenuPedidos(); break;
                case 0: return;
                default: Mensagem("Opção inválida."); break;
            }
        }
    }

    private void MenuClientes()
    {
        while (true)
        {
            ExibirTitulo("CLIENTES");
            Console.WriteLine("1 - Cadastrar");
            Console.WriteLine("2 - Listar");
            Console.WriteLine("3 - Alterar");
            Console.WriteLine("4 - Excluir");
            Console.WriteLine("0 - Voltar");

            switch (LerInteiro("Opção: "))
            {
                case 1: CadastrarCliente(); break;
                case 2: ListarClientes(true); break;
                case 3: AlterarCliente(); break;
                case 4: ExcluirCliente(); break;
                case 0: return;
                default: Mensagem("Opção inválida."); break;
            }
        }
    }

    private void CadastrarCliente()
    {
        ExibirTitulo("NOVO CLIENTE");
        _clientes.Add(new Cliente
        {
            Id = _proximoClienteId++,
            Nome = LerTexto("Nome: "),
            Email = LerTexto("E-mail: ")
        });
        Mensagem("Cliente cadastrado.");
    }

    private void AlterarCliente()
    {
        ExibirTitulo("ALTERAR CLIENTE");
        ListarClientes();
        var cliente = BuscarCliente(LerInteiro("Id: "));
        if (cliente is null) { Mensagem("Cliente não encontrado."); return; }

        cliente.Nome = LerTexto($"Nome ({cliente.Nome}): ", cliente.Nome);
        cliente.Email = LerTexto($"E-mail ({cliente.Email}): ", cliente.Email);
        Mensagem("Cliente alterado.");
    }

    private void ExcluirCliente()
    {
        ExibirTitulo("EXCLUIR CLIENTE");
        ListarClientes();
        var cliente = BuscarCliente(LerInteiro("Id: "));
        if (cliente is null) { Mensagem("Cliente não encontrado."); return; }
        if (_pedidos.Any(p => p.Cliente.Id == cliente.Id))
        {
            Mensagem("O cliente possui pedidos e não pode ser excluído.");
            return;
        }

        _clientes.Remove(cliente);
        Mensagem("Cliente excluído.");
    }

    private void ListarClientes(bool pausar = false)
    {
        Console.WriteLine("\n--- Lista de clientes ---");
        if (_clientes.Count == 0) Console.WriteLine("Nenhum cliente cadastrado.");
        foreach (var cliente in _clientes) Console.WriteLine(cliente);
        if (pausar) Pausar();
    }

    private void MenuProdutos()
    {
        while (true)
        {
            ExibirTitulo("PRODUTOS");
            Console.WriteLine("1 - Cadastrar");
            Console.WriteLine("2 - Listar");
            Console.WriteLine("3 - Alterar");
            Console.WriteLine("4 - Excluir");
            Console.WriteLine("0 - Voltar");

            switch (LerInteiro("Opção: "))
            {
                case 1: CadastrarProduto(); break;
                case 2: ListarProdutos(true); break;
                case 3: AlterarProduto(); break;
                case 4: ExcluirProduto(); break;
                case 0: return;
                default: Mensagem("Opção inválida."); break;
            }
        }
    }

    private void CadastrarProduto()
    {
        ExibirTitulo("NOVO PRODUTO");
        _produtos.Add(new Produto
        {
            Id = _proximoProdutoId++,
            Nome = LerTexto("Nome: "),
            Preco = LerDecimal("Preço: ")
        });
        Mensagem("Produto cadastrado.");
    }

    private void AlterarProduto()
    {
        ExibirTitulo("ALTERAR PRODUTO");
        ListarProdutos();
        var produto = BuscarProduto(LerInteiro("Id: "));
        if (produto is null) { Mensagem("Produto não encontrado."); return; }

        produto.Nome = LerTexto($"Nome ({produto.Nome}): ", produto.Nome);
        produto.Preco = LerDecimal($"Preço ({produto.Preco:C}): ", produto.Preco);
        Mensagem("Produto alterado.");
    }

    private void ExcluirProduto()
    {
        ExibirTitulo("EXCLUIR PRODUTO");
        ListarProdutos();
        var produto = BuscarProduto(LerInteiro("Id: "));
        if (produto is null) { Mensagem("Produto não encontrado."); return; }
        if (_pedidos.SelectMany(p => p.Itens).Any(i => i.Produto.Id == produto.Id))
        {
            Mensagem("O produto pertence a um pedido e não pode ser excluído.");
            return;
        }

        _produtos.Remove(produto);
        Mensagem("Produto excluído.");
    }

    private void ListarProdutos(bool pausar = false)
    {
        Console.WriteLine("\n--- Lista de produtos ---");
        if (_produtos.Count == 0) Console.WriteLine("Nenhum produto cadastrado.");
        foreach (var produto in _produtos) Console.WriteLine(produto);
        if (pausar) Pausar();
    }

    private void MenuPedidos()
    {
        while (true)
        {
            ExibirTitulo("PEDIDOS");
            Console.WriteLine("1 - Cadastrar");
            Console.WriteLine("2 - Listar");
            Console.WriteLine("3 - Consultar detalhes");
            Console.WriteLine("4 - Alterar");
            Console.WriteLine("5 - Excluir");
            Console.WriteLine("0 - Voltar");

            switch (LerInteiro("Opção: "))
            {
                case 1: CadastrarPedido(); break;
                case 2: ListarPedidos(true); break;
                case 3: ConsultarPedido(); break;
                case 4: AlterarPedido(); break;
                case 5: ExcluirPedido(); break;
                case 0: return;
                default: Mensagem("Opção inválida."); break;
            }
        }
    }

    private void CadastrarPedido()
    {
        ExibirTitulo("NOVO PEDIDO");
        if (_clientes.Count == 0 || _produtos.Count == 0)
        {
            Mensagem("Cadastre pelo menos um cliente e um produto primeiro.");
            return;
        }

        ListarClientes();
        var cliente = BuscarCliente(LerInteiro("Id do cliente: "));
        if (cliente is null) { Mensagem("Cliente não encontrado."); return; }

        var pedido = new Pedido
        {
            Id = _proximoPedidoId++,
            Data = LerData("Data (dd/MM/aaaa, Enter = hoje): ", DateTime.Today),
            Cliente = cliente
        };

        EditarItens(pedido);
        if (pedido.Itens.Count == 0)
        {
            Mensagem("O pedido foi cancelado porque não possui itens.");
            return;
        }

        _pedidos.Add(pedido);
        Mensagem($"Pedido cadastrado. Total: {pedido.ValorTotal:C}");
    }

    private void AlterarPedido()
    {
        ExibirTitulo("ALTERAR PEDIDO");
        ListarPedidos();
        var pedido = BuscarPedido(LerInteiro("Id: "));
        if (pedido is null) { Mensagem("Pedido não encontrado."); return; }

        pedido.Data = LerData($"Data ({pedido.Data:dd/MM/yyyy}): ", pedido.Data);
        ListarClientes();
        var idCliente = LerInteiro($"Id do cliente ({pedido.Cliente.Id}): ", pedido.Cliente.Id);
        var cliente = BuscarCliente(idCliente);
        if (cliente is not null) pedido.Cliente = cliente;
        else Console.WriteLine("Cliente inválido; o cliente atual foi mantido.");

        EditarItens(pedido);
        Mensagem("Pedido alterado.");
    }

    private void EditarItens(Pedido pedido)
    {
        while (true)
        {
            LimparConsole();
            Console.WriteLine($"--- ITENS DO PEDIDO {pedido.Id} ---");
            ExibirItens(pedido);
            Console.WriteLine("\n1 - Adicionar item");
            Console.WriteLine("2 - Alterar quantidade");
            Console.WriteLine("3 - Remover item");
            Console.WriteLine("0 - Concluir");

            switch (LerInteiro("Opção: "))
            {
                case 1: AdicionarItem(pedido); break;
                case 2: AlterarItem(pedido); break;
                case 3: RemoverItem(pedido); break;
                case 0: return;
                default: Mensagem("Opção inválida."); break;
            }
        }
    }

    private void AdicionarItem(Pedido pedido)
    {
        ListarProdutos();
        var produto = BuscarProduto(LerInteiro("Id do produto: "));
        if (produto is null) { Mensagem("Produto não encontrado."); return; }

        var itemExistente = pedido.Itens.FirstOrDefault(i => i.Produto.Id == produto.Id);
        var qtd = LerInteiro("Quantidade: ", minimo: 1);
        if (itemExistente is not null)
        {
            itemExistente.Qtd += qtd;
        }
        else
        {
            pedido.Itens.Add(new ItemPedido
            {
                Produto = produto,
                Qtd = qtd,
                Valor = produto.Preco
            });
        }
    }

    private static void AlterarItem(Pedido pedido)
    {
        if (pedido.Itens.Count == 0) { Mensagem("O pedido não possui itens."); return; }
        ExibirItens(pedido, numerar: true);
        var posicao = LerInteiro("Número do item: ", minimo: 1);
        if (posicao > pedido.Itens.Count) { Mensagem("Item inválido."); return; }
        pedido.Itens[posicao - 1].Qtd = LerInteiro("Nova quantidade: ", minimo: 1);
    }

    private static void RemoverItem(Pedido pedido)
    {
        if (pedido.Itens.Count == 0) { Mensagem("O pedido não possui itens."); return; }
        ExibirItens(pedido, numerar: true);
        var posicao = LerInteiro("Número do item: ", minimo: 1);
        if (posicao > pedido.Itens.Count) { Mensagem("Item inválido."); return; }
        pedido.Itens.RemoveAt(posicao - 1);
    }

    private void ConsultarPedido()
    {
        ExibirTitulo("DETALHES DO PEDIDO");
        ListarPedidos();
        var pedido = BuscarPedido(LerInteiro("Id: "));
        if (pedido is null) { Mensagem("Pedido não encontrado."); return; }

        Console.WriteLine($"\nPedido: {pedido.Id}");
        Console.WriteLine($"Data: {pedido.Data:dd/MM/yyyy}");
        Console.WriteLine($"Cliente: {pedido.Cliente.Nome}");
        ExibirItens(pedido);
        Console.WriteLine($"TOTAL DO PEDIDO: {pedido.ValorTotal:C}");
        Pausar();
    }

    private void ExcluirPedido()
    {
        ExibirTitulo("EXCLUIR PEDIDO");
        ListarPedidos();
        var pedido = BuscarPedido(LerInteiro("Id: "));
        if (pedido is null) { Mensagem("Pedido não encontrado."); return; }
        _pedidos.Remove(pedido);
        Mensagem("Pedido excluído.");
    }

    private void ListarPedidos(bool pausar = false)
    {
        Console.WriteLine("\n--- Lista de pedidos ---");
        if (_pedidos.Count == 0) Console.WriteLine("Nenhum pedido cadastrado.");
        foreach (var pedido in _pedidos) Console.WriteLine(pedido);
        if (pausar) Pausar();
    }

    private static void ExibirItens(Pedido pedido, bool numerar = false)
    {
        Console.WriteLine("\n--- Itens ---");
        if (pedido.Itens.Count == 0) Console.WriteLine("Nenhum item adicionado.");
        for (var i = 0; i < pedido.Itens.Count; i++)
        {
            var prefixo = numerar ? $"{i + 1} - " : string.Empty;
            Console.WriteLine(prefixo + pedido.Itens[i]);
        }
    }

    private Cliente? BuscarCliente(int id) => _clientes.FirstOrDefault(c => c.Id == id);
    private Produto? BuscarProduto(int id) => _produtos.FirstOrDefault(p => p.Id == id);
    private Pedido? BuscarPedido(int id) => _pedidos.FirstOrDefault(p => p.Id == id);

    private static string LerTexto(string mensagem, string? padrao = null)
    {
        while (true)
        {
            Console.Write(mensagem);
            var texto = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(texto)) return texto;
            if (padrao is not null) return padrao;
            Console.WriteLine("O texto não pode ficar vazio.");
        }
    }

    private static int LerInteiro(string mensagem, int? padrao = null, int minimo = 0)
    {
        while (true)
        {
            Console.Write(mensagem);
            var texto = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(texto) && padrao.HasValue) return padrao.Value;
            if (int.TryParse(texto, out var valor) && valor >= minimo) return valor;
            Console.WriteLine($"Digite um número inteiro maior ou igual a {minimo}.");
        }
    }

    private static decimal LerDecimal(string mensagem, decimal? padrao = null)
    {
        while (true)
        {
            Console.Write(mensagem);
            var texto = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(texto) && padrao.HasValue) return padrao.Value;
            if (decimal.TryParse(texto, NumberStyles.Number, CultureInfo.CurrentCulture,
                    out var valor) && valor >= 0) return valor;
            Console.WriteLine("Digite um valor monetário válido.");
        }
    }

    private static DateTime LerData(string mensagem, DateTime padrao)
    {
        while (true)
        {
            Console.Write(mensagem);
            var texto = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(texto)) return padrao;
            if (DateTime.TryParseExact(texto, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var data)) return data;
            Console.WriteLine("Use o formato dd/MM/aaaa.");
        }
    }

    private static void ExibirTitulo(string titulo)
    {
        LimparConsole();
        Console.WriteLine($"=== {titulo} ===");
    }

    private static void LimparConsole()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // A saída pode estar redirecionada durante testes automatizados.
        }
    }

    private static void Mensagem(string texto)
    {
        Console.WriteLine($"\n{texto}");
        Pausar();
    }

    private static void Pausar()
    {
        Console.WriteLine("\nPressione Enter para continuar...");
        Console.ReadLine();
    }
}
