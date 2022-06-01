using Financas.Core;
using System.ComponentModel.DataAnnotations;

namespace Financas.Inputs
{
    public class InInformarCodigoDeAcesso
    {
        [Required(ErrorMessage = ErrorMessagesConsts.KeyObrigatorio, AllowEmptyStrings = false)]
        public string Key { get; set; }

        [Required(ErrorMessage = ErrorMessagesConsts.CodigoEmailObrigatorio, AllowEmptyStrings = false)]
        public string Codigo { get; set; }
    }
}
