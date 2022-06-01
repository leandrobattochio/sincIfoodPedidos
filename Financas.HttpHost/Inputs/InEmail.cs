using Financas.Core;
using System.ComponentModel.DataAnnotations;

namespace Financas.Inputs
{
    public class InEmail
    {
        [Required(ErrorMessage = ErrorMessagesConsts.EmailObrigatorio, AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = ErrorMessagesConsts.EmailInvalido)]
        public string Email { get; set; }
    }
}
