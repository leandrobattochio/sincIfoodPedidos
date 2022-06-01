using Financas.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financas.Domain
{
    public class EstabelecimentoIfood : BaseEntity
    {
        protected EstabelecimentoIfood()
        {

        }

        public EstabelecimentoIfood(Guid idIfood, string nome, string tipo)
        {
            IdIfood = idIfood;
            Nome = nome;
            Tipo = tipo;
        }

        public Guid IdIfood { get; private set; }

        [Column(TypeName = "varchar(100)")]
        public string Nome { get; private set; }

        [Column(TypeName = "varchar(100)")]
        public string Tipo { get; private set; }

    }
}
