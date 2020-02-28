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
        public void GivenLazyListWhenLoadAfterAddShouldBeAddedLast()
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

        [Fact]
        public void GivenLazyListWhenGetIndexShouldLoad()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);

            lazyList[0].Should().NotBeNull();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenSetIndexShouldLoad()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);
            lazyList[0] = new Stub();
            
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenGetEnumeratorShouldLoad()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);

            lazyList.GetEnumerator().Should().NotBeNull();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenClearShouldLoad()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);
            lazyList.Clear();

            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenContainsShouldLoad()
        {
            var stub = new Stub();
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { stub })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);

            lazyList.Contains(stub).Should().BeTrue();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenIndexOfShouldLoad()
        {
            var stub = new Stub();
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { stub })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);

            lazyList.IndexOf(stub).Should().Be(0);
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenCopyToShouldLoad()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);
            var list = new Stub[2];
            lazyList.CopyTo(list, 0);

            list.Should().HaveCount(2);
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenRemoveShouldLoad()
        {
            var stub = new Stub();
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { stub })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);
            lazyList.Remove(stub);

            lazyList.Should().BeEmpty();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenInsertShouldLoad()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { new Stub(), new Stub() })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);
            var stub = new Stub();
            lazyList.Insert(1, stub);

            lazyList[1].Should().Be(stub);
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenRemoveAtShouldLoad()
        {
            _lazyLoadResolverMock.SetupGet(x => x.ResolveType)
                .Returns(typeof(IEnumerable<Stub>))
                .Verifiable();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { new Stub(), new Stub() })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadFactory);
            lazyList.RemoveAt(0);
                
            lazyList.Should().HaveCount(1);
            _lazyLoadResolverMock.VerifyAll();
        }
    }
}