using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LazyList.Core;
using Moq;
using Xunit;

namespace LazyList.Tests.Core
{
    public class LazyLoadFactoryTests
    {
        private readonly Mock<ILazyLoadResolver> _lazyLoadResolverMock;
        private readonly LazyListFactory _lazyListFactory;

        public LazyLoadFactoryTests()
        {
            _lazyLoadResolverMock = new Mock<ILazyLoadResolver>();
            _lazyListFactory = new LazyListFactory(new[]
            {
                _lazyLoadResolverMock.Object
            });
        }
        
        [Fact]
        public void GivenLazyLoadWhenResolveWithSameIdShouldReturnSameInstance()
        {
            var stub = new Stub();
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(Stub))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.Is<LazyLoadParameter>(x => x.Value.Equals(1))))
                .Returns(new [] { stub })
                .Verifiable();
            
            var stubs = _lazyListFactory.Create<Stub>(1);
            stubs.Should().Contain(stub);
            
            _lazyLoadResolverMock.VerifyAll();
        }
    }
}
