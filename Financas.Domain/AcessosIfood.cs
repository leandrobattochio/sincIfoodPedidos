using Financas.Core;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financas.Domain
{
    public class AcessosIfood : BaseEntity
    {
        public AcessosIfood(string email, string accessToken, DateTime validUntil, string refreshToken)
        {
            Email = email;
            AccessToken = accessToken;
            ValidUntil = validUntil;
            CreatedAt = DateTime.Now;
            RefreshToken = refreshToken;
        }

        [Column(TypeName = "varchar(100)")]
        public string Email { get; private set; }

        [Column(TypeName = "varchar(max)")]
        public string AccessToken { get; private set; }

        public DateTime ValidUntil { get; private set; }

        [Column(TypeName = "varchar(max)")]
        public string RefreshToken { get; private set; }

        public DateTime CreatedAt { get; private set; }

        #region Metodos da Entidade

        public void AtualizarToken(string token, string refreshToken, DateTime validoAte)
        {
            AccessToken = token;
            RefreshToken = refreshToken;
            ValidUntil = validoAte;
        }

        public bool IsAccessTokenExpirado() => DateTime.Now >= ValidUntil;

        #endregion
    }
}
