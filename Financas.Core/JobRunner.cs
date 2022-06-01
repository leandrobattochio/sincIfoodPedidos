using Hangfire;

namespace Financas.Core
{
    public static class JobRunner
    {
        public static void Enqueue<T, A>(A argumento)
            where T : JobSegundoPlano<A>
        {
            BackgroundJob.Enqueue<T>((c) => c.Execute(argumento));
        }

    }
}
