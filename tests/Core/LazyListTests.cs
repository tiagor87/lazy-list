using System.Collections.Generic;
using FluentAssertions;
using LazyList.Core;
using Moq;
using Xunit;

namespace LazyList.Tests.Core
{
    public class LazyListTests
    {
        private readonly Mock<ILazyLoadResolver<IEnumerable<Stub>>> _lazyLoadResolverMock;

        public LazyListTests()
        {
            _lazyLoadResolverMock = new Mock<ILazyLoadResolver<IEnumerable<Stub>>>();
        }
        
        [Fact]
        public void GivenLazyListWhenLoadShouldNotFailIfLazyParameterIsNull()
        {
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object, null);
            lazyList.Should().HaveCount(2);
            
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenAddShouldNotLoad()
        {
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);
            lazyList.Add(new Stub());
            
            _lazyLoadResolverMock.Verify(x => x.Resolve(It.IsAny<LazyLoadParameter>()), Times.Never());
        }
        
        [Fact]
        public void GivenLazyListWhenLoadAfterAddShouldBeAddedLast()
        {
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);
            var stub = new Stub();
            lazyList.Add(stub);

            lazyList.Should().HaveCount(3);
            lazyList.Should().HaveElementAt(2, stub);
            _lazyLoadResolverMock.VerifyAll();
        }

        [Fact]
        public void GivenLazyListWhenGetIndexShouldLoad()
        {
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);

            lazyList[0].Should().NotBeNull();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenSetIndexShouldLoad()
        {
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);
            lazyList[0] = new Stub();
            
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenGetEnumeratorShouldLoad()
        {
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);

            lazyList.GetEnumerator().Should().NotBeNull();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenClearShouldLoad()
        {
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);
            lazyList.Clear();

            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenContainsShouldLoad()
        {
            var stub = new Stub();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { stub })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);

            lazyList.Contains(stub).Should().BeTrue();
            _lazyLoadResolverMock.VerifyAll();
        }

        [Fact]
        public void GivenLazyListWhenIndexOfShouldLoad()
        {
            var stub = new Stub();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { stub })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);

            lazyList.IndexOf(stub).Should().Be(0);
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenCopyToShouldLoad()
        {
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] {new Stub(), new Stub()})
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);
            var list = new Stub[2];
            lazyList.CopyTo(list, 0);

            list.Should().HaveCount(2);
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenRemoveShouldLoad()
        {
            var stub = new Stub();
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { stub })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);
            lazyList.Remove(stub);

            lazyList.Should().BeEmpty();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenInsertShouldLoad()
        {
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { new Stub(), new Stub() })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);
            var stub = new Stub();
            lazyList.Insert(1, stub);

            lazyList[1].Should().Be(stub);
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListWhenRemoveAtShouldLoad()
        {
            _lazyLoadResolverMock.Setup(x => x.Resolve(It.IsAny<LazyLoadParameter>()))
                .Returns(new[] { new Stub(), new Stub() })
                .Verifiable();
            
            var lazyList = new LazyList<Stub>(_lazyLoadResolverMock.Object);
            lazyList.RemoveAt(0);
                
            lazyList.Should().HaveCount(1);
            _lazyLoadResolverMock.VerifyAll();
        }
    }
}