using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LazyList.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace LazyList.Tests.Core
{
    public class LazyListFactoryTests
    {
        private readonly Mock<ILazyLoadResolver> _lazyLoadResolverMock;
        private readonly LazyListFactory _lazyListFactory;

        public LazyListFactoryTests()
        {
            _lazyLoadResolverMock = new Mock<ILazyLoadResolver>();
            _lazyListFactory = new LazyListFactory(new[]
            {
                _lazyLoadResolverMock.Object
            });
        }
        
        [Fact]
        public void GivenLazyListFactoryWhenCreateAndResolverRegisteredShouldReturnInstance()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(Stub))
                .Verifiable();
            
            var list = _lazyListFactory.Create<Stub>(1);

            list.Should().NotBeNull();
            list.Should().BeOfType<LazyList<Stub>>();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListFactoryWhenCreateAndResolverNotRegisteredShouldReturnInstance()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(String))
                .Verifiable();
            
            var list = _lazyListFactory.Create<Stub>(1);

            list.Should().NotBeNull();
            list.Should().BeOfType<LazyList<Stub>>();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListFactoryWhenStaticCreateShouldReturnInstance()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider.Setup(x => x.GetService(typeof(ILazyListFactory)))
                .Returns(_lazyListFactory)
                .Verifiable();
            
            LazyListFactory.Init(serviceProvider.Object);
            var list = LazyListFactory.CreateList<Stub>(1);

            list.Should().NotBeNull();
            list.Should().BeOfType<LazyList<Stub>>();
            serviceProvider.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListFactoryWhenStaticCreateAndNotInitShouldThrow()
        {
            Func<IList<Stub>> action = () => LazyListFactory.CreateList<Stub>(1);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}
