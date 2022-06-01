using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financas.Domain
{
    [Owned]
    public class OrigemPedidoIfood
    {
        protected OrigemPedidoIfood()
        {

        }

        public OrigemPedidoIfood(string plataforma, string appName, string appVersion)
        {
            Plataforma = plataforma;
            AppName = appName;
            AppVersion = appVersion;
        }

        [Column(TypeName = "varchar(50)")]
        public string Plataforma { get; private set; }

        [Column(TypeName = "varchar(50)")]
        public string AppName { get; private set; }

        [Column(TypeName = "varchar(50)")]
        public string AppVersion { get; private set; }
    }
}
