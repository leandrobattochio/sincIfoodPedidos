using System;

namespace Financas.Core
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; }
    }
}
