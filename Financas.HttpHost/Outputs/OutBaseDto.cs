namespace Financas.Outputs
{
    public enum StatusExecucaoEnum
    {
        Falha = 0,
        Sucesso = 1,
    }

    public class OutBaseDto
    {
        public StatusExecucaoEnum Status { get; set; }
        public string Mensagem { get; set; }

        public void PreencherSucesso(string mensagem = "")
        {
            Status = StatusExecucaoEnum.Sucesso;
            Mensagem = mensagem;
        }

        public void PreencherErro(string mensagem)
        {
            Status = StatusExecucaoEnum.Falha;
            Mensagem = mensagem;
        }
    }
}
