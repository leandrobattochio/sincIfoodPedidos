using Financas.Core;
using Financas.Domain.Shared;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Financas.Domain
{
    public class RequestLog : BaseEntity
    {
        public RequestLog(TipoIntegradorEnum tipoIntegrador, string dadosEntrada, string uRL, TipoRequisicaoEnum tipoRequisicao)
        {
            TipoIntegrador = tipoIntegrador;
            DadosEntrada = dadosEntrada;
            DataRequisicao = DateTime.Now;
            URL = uRL;
            TipoRequisicao = tipoRequisicao;
        }

        public DateTime DataRequisicao { get; private set; }
        public TipoIntegradorEnum TipoIntegrador { get; private set; }
        public string DadosEntrada { get; private set; }
        public string DadosSaida { get; private set; }
        public StatusRequestEnum Status { get; private set; }
        public int HttpStatusCode { get; private set; }
        public string ExceptionMessage { get; private set; }

        [Column(TypeName = "varchar(1000)")]
        public string URL { get; private set; }

        public TipoRequisicaoEnum TipoRequisicao { get; private set; }


        public void AtualizarLogSucesso(HttpStatusCode status, string dadosSaida)
        {
            Status = StatusRequestEnum.Sucesso;
            DadosSaida = dadosSaida;
            HttpStatusCode = (int)status;
        }

        public void AtualizarLogErro(HttpStatusCode status, string dadosSaida, string exceptionMessage = "")
        {
            Status = StatusRequestEnum.Falha;
            DadosSaida = dadosSaida;
            HttpStatusCode = (int)status;
            ExceptionMessage = exceptionMessage;
        }
    }
}
