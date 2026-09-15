using System;
using System.Collections.Generic;
using System.Text;

namespace CadastroAnimaisExercicio.Entidades
{

    public class Animal
    {
        public string Nome { get; set; }
        public string Especie { get; set; }
        public Tutor Tutor { get; set; }
        public int Idade { get; private set; }

        public Animal(string nome, string especie, int idade)
        {
            if (idade < 0)
                throw new ArgumentException("Idade não pode ser negativa.", nameof(idade));

            Nome = nome;
            Especie = especie;
            Idade = idade;
        }

        public void ExibirDados()
        {
            var tutorInfo = Tutor != null ? $", Tutor: {Tutor.Nome} ({Tutor.Telefone})" : ", Tutor: N/A";
            Console.WriteLine($"Nome: {Nome}, Espécie: {Especie}, Idade: {Idade}{tutorInfo}");
        }

        public void EmitirSom()
        {
            var especieLower = (Especie ?? string.Empty).ToLowerInvariant();

            switch (especieLower)
            {
                case "cachorro":
                case "cão":
                case "cao":
                    Console.WriteLine("Au au!");
                    break;
                case "gato":
                    Console.WriteLine("Miau!");
                    break;
                case "vaca":
                    Console.WriteLine("Muu!");
                    break;
                case "leão":
                case "leao":
                    Console.WriteLine("Roar!");
                    break;
                case "pato":
                    Console.WriteLine("Quack!");
                    break;
                default:
                    Console.WriteLine("O animal emite um som.");
                    break;
            }
        }

        public bool AtualizarIdade(int novaIdade)
        {
            if (novaIdade < 0)
                return false;

            Idade = novaIdade;
            return true;
        }
    }
}
