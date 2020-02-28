using System;
using FluentAssertions;
using LazyList.Core;
using Xunit;

namespace LazyList.Tests.Core
{
    public class LazyLoadParameterTests
    {
        [Fact]
        public void GivenLazyParameterWhenNullValueShouldNotThrow()
        {
            var nullParameter = LazyLoadParameter.Null;
            Func<bool> action = () => nullParameter.Equals(new LazyLoadParameter(1));
            action.Should().NotThrow();
        }

        [Fact]
        public void GivenLazyParameterWhenSameValueShouldBeEqual()
        {
            var parameter1 = new LazyLoadParameter(1);
            var parameter2 = new LazyLoadParameter(1);
            parameter1.Should().Be(parameter2);
        }

        [Fact]
        public void GivenLazyParameterWhenIntValueShouldImplicitConvert()
        {
            int value = 1;
            LazyLoadParameter parameter = value;
            parameter.Value.Should().Be(value);
        }
        
        [Fact]
        public void GivenLazyParameterWhenLongValueShouldImplicitConvert()
        {
            long value = 1;
            LazyLoadParameter parameter = value;
            parameter.Value.Should().Be(value);
        }
        
        [Fact]
        public void GivenLazyParameterWhenStringValueShouldImplicitConvert()
        {
            string value = "1";
            LazyLoadParameter parameter = value;
            parameter.Value.Should().Be(value);
        }
    }
}