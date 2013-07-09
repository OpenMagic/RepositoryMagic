using System;
using FluentAssertions;

namespace RepositoryMagic.Tests.TestHelpers
{
    public abstract class TestBase
    {
        protected void ShouldThrowArgumentNullException(Action action, string paramName)
        {
            action.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: "+paramName);
        }
    }
}
