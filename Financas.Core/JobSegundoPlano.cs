using System.Threading.Tasks;

namespace Financas.Core
{
    public abstract class JobSegundoPlano<A>
    {
        public abstract Task Execute(A argumento);
    }
}
