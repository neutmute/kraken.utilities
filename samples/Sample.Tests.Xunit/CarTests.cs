using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Tests.Xunit
{
    public class CarTests : TestBase
    {
        [Fact]
        public void AssertBuilderDemo()
        {
            // Arrange, Act
            var car = new Car();

            // Assert
            AssertBuilder.Generate(car, "car");
        }
    }
}
