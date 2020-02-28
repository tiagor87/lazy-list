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
        private readonly LazyLoadFactory _lazyLoadFactory;

        public LazyLoadFactoryTests()
        {
            _lazyLoadResolverMock = new Mock<ILazyLoadResolver>();
            _lazyLoadFactory = new LazyLoadFactory(new[]
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
                .Returns(stub)
                .Verifiable();
            
            var teste = _lazyLoadFactory.Resolve<Stub>(1);
            var teste2 = _lazyLoadFactory.Resolve<Stub>(1);
            
            teste.Should().Be(teste2);
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyLoadWhenResolveWithDifferentParametersShouldReturnDifferentInstances()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(Stub))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.Is<LazyLoadParameter>(x => x.Value.Equals(1))))
                .Returns(new Stub())
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.Is<LazyLoadParameter>(x => x.Value.Equals(2))))
                .Returns(new Stub())
                .Verifiable();
            
            var teste = _lazyLoadFactory.Resolve<Stub>(1);
            var teste2 = _lazyLoadFactory.Resolve<Stub>(2);
            
            teste.Should().NotBe(teste2);
            
            _lazyLoadResolverMock.VerifyAll();
        }

        [Fact]
        public async Task GivenLazyLoadWhenResolveAsyncShouldReturnValue()
        {
            var stub = new Stub();
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(Stub))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.ResolveAsync(It.Is<LazyLoadParameter>(x => x.Value.Equals(1))))
                .ReturnsAsync(stub)
                .Verifiable();
            
            var teste = await _lazyLoadFactory.ResolveAsync<Stub>(1);

            teste.Should().Be(stub);
            _lazyLoadResolverMock.VerifyAll();
        }
    }
}
