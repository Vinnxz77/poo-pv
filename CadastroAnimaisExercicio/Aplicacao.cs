using CadastroAnimaisExercicio.Entidades;
using System;
using System.Collections.Generic;

namespace CadastroAnimaisExercicio;

public class Aplicacao
{
    private static readonly List<Animal> animais = new List<Animal>();

    public void Executar()
    {
        while (true)
        {
            ExibirMenu();
            var opcao = LerInteiro("Escolha uma opção: ");

            switch (opcao)
            {
                case 1:
                    CadastrarAnimal();
                    break;
                case 2:
                    ListarAnimais();
                    break;
                case 3:
                    BuscarAnimal();
                    break;
                case 4:
                    AlterarIdade();
                    break;
                case 5:
                    FazerAnimalEmitirSom();
                    break;
                case 6:
                    ExemploListaAnimais();
                    break;
                case 0:
                    Console.WriteLine("Programa encerrado.");
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            Pausar();
        }
    }

    private static void ExibirMenu()
    {
        LimparConsole();
        Console.WriteLine("=== CADASTRO DE ANIMAIS ===");
        Console.WriteLine("1 - Cadastrar animal");
        Console.WriteLine("2 - Listar animais");
        Console.WriteLine("3 - Buscar animal");
        Console.WriteLine("4 - Alterar idade");
        Console.WriteLine("5 - Emitir som");
        Console.WriteLine("6 - Exemplo: criar e listar 3 animais");
        Console.WriteLine("0 - Sair");
        Console.WriteLine();
    }

    private static void ExemploListaAnimais()
    {
        var lista = new List<Animal>();

        var a1 = new Animal("Rex", "Cachorro", 3) { Tutor = new Tutor("Carlos", "(11) 99999-0001") };
        var a2 = new Animal("Mimi", "Gato", 2) { Tutor = new Tutor("Ana", "(11) 99999-0002") };
        var a3 = new Animal("Bela", "Pato", 1) { Tutor = new Tutor("João", "(11) 99999-0003") };

        lista.Add(a1);
        lista.Add(a2);
        lista.Add(a3);

        Console.WriteLine("Lista de animais criada (foreach):");
        foreach (var animal in lista)
        {
            animal.ExibirDados();
        }
    }

    private static void CadastrarAnimal()
    {
        var Nome = LerTexto("Digite o nome do animal: ");
        var Especie = LerTexto("Digite a espécie do animal: ");
        var Idade = LerInteiro("Digite a idade do animal: ");
        var nomeTutor = LerTexto("Digite o nome do tutor: ");
        var telefoneTutor = LerTexto("Digite o telefone do tutor: ");
        try
        {
            var animal = new Animal(Nome, Especie, Idade) { Tutor = new Tutor(nomeTutor, telefoneTutor) };
            animal.ExibirDados();
            animais.Add(animal);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Erro ao cadastrar animal: {ex.Message}");
        }
    }

    private static void ListarAnimais()
    {
        if (animais.Count == 0)
        {
            Console.WriteLine("Nenhum animal cadastrado.");
            return;
        }

        Console.WriteLine("Animais cadastrados:");
        for (int i = 0; i < animais.Count; i++)
        {
            Console.Write($"{i + 1} - ");
            animais[i].ExibirDados();
        }
    }

    private static void BuscarAnimal()
    {
        if (animais.Count == 0)
        {
            Console.WriteLine("Animal não encontrado.");
            return;
        }

        var nome = LerTexto("Digite o nome do animal para buscar: ");
        var encontrado = animais.Find(a => !string.IsNullOrWhiteSpace(a.Nome) && string.Equals(a.Nome.Trim(), nome.Trim(), StringComparison.OrdinalIgnoreCase));

        if (encontrado == null)
        {
            Console.WriteLine("Animal não encontrado.");
            return;
        }

        encontrado.ExibirDados();
    }

    private static void AlterarIdade()
    {
        if (animais.Count == 0)
        {
            Console.WriteLine("Nenhum animal cadastrado.");
            return;
        }

        Console.WriteLine("Selecione o animal para alterar a idade:");
        for (int i = 0; i < animais.Count; i++)
        {
            Console.Write($"{i + 1} - ");
            animais[i].ExibirDados();
        }

        int escolha;
        while (true)
        {
            escolha = LerInteiro("Digite o número do animal: ");
            if (escolha >= 1 && escolha <= animais.Count)
                break;

            Console.WriteLine("Escolha inválida. Tente novamente.");
        }

        var novaIdade = LerInteiro("Digite a nova idade: ");

        if (novaIdade < 0)
        {
            Console.WriteLine("Idade inválida. A idade não pode ser negativa.");
            return;
        }

        // Atualiza e confirma
        var atualizado = animais[escolha - 1].AtualizarIdade(novaIdade);
        if (atualizado)
        {
            Console.WriteLine("Idade atualizada:");
            animais[escolha - 1].ExibirDados();
        }
        else
        {
            Console.WriteLine("Erro ao atualizar a idade.");
        }
    }

    private static void FazerAnimalEmitirSom()
    {
        if (animais.Count == 0)
        {
            Console.WriteLine("Nenhum animal cadastrado.");
            return;
        }

        Console.WriteLine("Selecione o animal que deve emitir som:");
        for (int i = 0; i < animais.Count; i++)
        {
            Console.Write($"{i + 1} - ");
            animais[i].ExibirDados();
        }

        int escolha;
        while (true)
        {
            escolha = LerInteiro("Digite o número do animal: ");
            if (escolha >= 1 && escolha <= animais.Count)
                break;

            Console.WriteLine("Escolha inválida. Tente novamente.");
        }

        Console.WriteLine("Som do animal:");
        animais[escolha - 1].EmitirSom();
    }

    private static string LerTexto(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            var texto = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                return texto;
            }

            Console.WriteLine("O texto não pode ficar vazio.");
        }
    }

    private static int LerInteiro(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);

            if (int.TryParse(Console.ReadLine(), out var numero))
            {
                return numero;
            }

            Console.WriteLine("Digite um número inteiro válido.");
        }
    }

    private static void Pausar()
    {
        Console.WriteLine("\nPressione Enter para continuar...");
        Console.ReadLine();
    }

    private static void LimparConsole()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // Permite executar o projeto com entrada redirecionada em testes.
        }
    }
}
