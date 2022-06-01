using Financas.Core;
using System.ComponentModel.DataAnnotations;

namespace Financas.Inputs
{
    public class InCompletarLogin : InEmail
    {
        [Required(ErrorMessage = ErrorMessagesConsts.TokenObrigatorio, AllowEmptyStrings = false)]
        public string Token { get; set; }
    }
}
