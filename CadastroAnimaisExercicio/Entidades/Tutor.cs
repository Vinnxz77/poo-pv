using System;

namespace CadastroAnimaisExercicio.Entidades
{
    public class Tutor
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }

        public Tutor(string nome, string telefone)
        {
            Nome = nome;
            Telefone = telefone;
        }
    }
}
