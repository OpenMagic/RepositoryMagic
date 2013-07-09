using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryMagic.Tests
{
    [TestClass]
    public class MemoryRepositoryTests : IRepositoryTests
    {
        protected override IRepository<IRepositoryTests.TestModel, int> NewRepository()
        {
            return new MemoryRepository<IRepositoryTests.TestModel, int>();
        }
    }
}
