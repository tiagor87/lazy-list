using System.Collections.Generic;
using FluentAssertions;
using LazyList.Core;
using Moq;
using Xunit;

namespace LazyList.Tests.Core
{
    public class LazyListTests
    {
        private readonly Mock<ILazyLoadResolver> _lazyLoadResolverMock;
        private readonly LazyLoadFactory _lazyLoadFactory;

        public LazyListTests()
        {
            _lazyLoadResolverMock = new Mock<ILazyLoadResolver>();
            _lazyLoadFactory = new LazyLoadFactory(new[]
            {
                _lazyLoadResolverMock.Object
            }); 
        }
        
        [Fact]
        public void GivenLazyListWhenLoadShouldNotFailIfLazyParameterIsNull()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory, null);
            lazyList.Should().HaveCount(2);
            
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenAddShouldNotLoad()
        {
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);
            lazyList.Add(new Stub());
            
            _lazyLoadResolverMock.Verify(x => x.Resolve(It.IsAny<LazyLoadParameter>()), Times.Never());
        }
        
        [Fact]
        public void GivenLazyListWhenLoadAfterAddShouldSetAddedLast()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);
            var stub = new Stub();
            lazyList.Add(stub);

            lazyList.Should().HaveCount(3);
            lazyList.Should().HaveElementAt(2, stub);
            _lazyLoadResolverMock.VerifyAll();
            
        }
    }
}