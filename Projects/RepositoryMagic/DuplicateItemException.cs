using System;

namespace RepositoryMagic
{
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message)
            : base(message)
        {
        }
    }
}
