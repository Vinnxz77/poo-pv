using System;
using System.Collections.Generic;
using System.Text;

namespace CadastroAnimaisExercicio.Entidades
{
    public class Animal
    {
        public string Nome;
        public string Especie;
        public int Idade;

        public Animal(string nome, string especie, int idade)
        {
            Nome = nome;
            Especie = especie;
            Idade = idade;
        }

        public void ExibirDados()
        {
            Console.WriteLine($"Nome: {Nome}, Espécie: {Especie}, Idade: {Idade}");
        }
    }
}
