﻿using Xunit;
using Moq;

namespace TimeRegistration.WebTests
{
    public class MainControllerTests
    {
        // TODO: Add some actual tests!

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
