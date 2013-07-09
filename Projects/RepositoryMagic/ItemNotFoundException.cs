using System;

namespace RepositoryMagic
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message)
            : base(message)
        {
        }
    }
}
