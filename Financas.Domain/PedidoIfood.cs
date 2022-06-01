using Financas.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financas.Domain
{
    public class PedidoIfood : BaseEntity
    {
        protected PedidoIfood()
        {

        }

        public PedidoIfood(Guid idIfood, string ultimoStatus, DateTime dataCriacao, DateTime dataFinalizacao, OrigemPedidoIfood origem, decimal totalPedido)
        {
            IdIfood = idIfood;
            UltimoStatus = ultimoStatus;
            DataCriacao = dataCriacao;
            DataFinalizacao = dataFinalizacao;
            Origem = origem;
            TotalPedido = totalPedido;
        }

        public Guid IdIfood { get; private set; }

        [Column(TypeName = "varchar(50)")]
        public string UltimoStatus { get; private set; }

        public DateTime DataCriacao { get; private set; }
        public DateTime DataFinalizacao { get; private set; }
        public OrigemPedidoIfood Origem { get; private set; }
        public decimal TotalPedido { get; private set; }

        public EstabelecimentoIfood Estabelecimento { get; private set; }
        public Guid EstabelecimentoId { get; private set; }

        public AcessosIfood AcessoIfood { get; private set; }
        public Guid AcessoIfoodId { get; private set; }

        #region Metodos Entidade

        public void AssociarEstabelecimento(EstabelecimentoIfood idEstabelecimento)
        {
            Estabelecimento = idEstabelecimento;
        }

        public void AssociarAcesso(AcessosIfood acesso)
        {
            AcessoIfood = acesso;
        }

        #endregion
    }
}
