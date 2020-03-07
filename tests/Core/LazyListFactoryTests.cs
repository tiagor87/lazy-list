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
    [CollectionDefinition("LazyListFactory", DisableParallelization = false)]
    public class LazyListFactoryTests : IDisposable
    {
        private readonly Mock<ILazyLoadResolver<IEnumerable<Stub>>> _lazyLoadResolverMock;
        private readonly LazyListFactory _lazyListFactory;
        private readonly Mock<IDisposable> _disposableScopeMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;

        public LazyListFactoryTests()
        {
            _lazyLoadResolverMock = new Mock<ILazyLoadResolver<IEnumerable<Stub>>>();
            _disposableScopeMock = new Mock<IDisposable>();
            _serviceProviderMock = new Mock<IServiceProvider>();
            _lazyListFactory = new LazyListFactory(_disposableScopeMock.Object, _serviceProviderMock.Object);
        }
        
        [Fact]
        public void GivenLazyListFactoryWhenCreateAndResolverRegisteredShouldReturnInstance()
        {
            _serviceProviderMock.Setup(x => x.GetService(typeof(ILazyLoadResolver<IEnumerable<Stub>>)))
                .Returns(_lazyLoadResolverMock.Object)
                .Verifiable();
            var list = _lazyListFactory.Create<Stub>(1);

            list.Should().NotBeNull();
            list.Should().BeOfType<LazyList<Stub>>();
            _serviceProviderMock.VerifyAll();
            _lazyLoadResolverMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListFactoryWhenCreateAndResolverNotRegisteredShouldReturnInstance()
        {
            _serviceProviderMock.Setup(x => x.GetService(typeof(ILazyLoadResolver<IEnumerable<Stub>>)))
                .Returns(_lazyLoadResolverMock.Object);
            var list = _lazyListFactory.Create<Stub>(1);

            list.Should().NotBeNull();
            list.Should().BeOfType<LazyList<Stub>>();
            _serviceProviderMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListFactoryWhenStaticCreateShouldReturnInstance()
        {
            _serviceProviderMock.Setup(x => x.GetService(typeof(ILazyLoadResolver<IEnumerable<Stub>>)))
                .Returns(_lazyLoadResolverMock.Object);
            LazyListFactory.RegisterInstance(_lazyListFactory);
            var list = LazyListFactory.CreateList<Stub>(1);
            
            list.Should().NotBeNull();
            list.Should().BeOfType<LazyList<Stub>>();
            _serviceProviderMock.VerifyAll();
        }
        
        [Fact]
        public void GivenLazyListFactoryWhenStaticCreateAndNotInitShouldThrow()
        {
            Func<IList<Stub>> action = () => LazyListFactory.CreateList<Stub>(1);

            action.Should().Throw<InvalidOperationException>();
        }

        public void Dispose()
        {
            LazyListFactory.RegisterInstance(null);
            _lazyListFactory.Dispose();
            _disposableScopeMock.Verify(x => x.Dispose(), Times.Once());
        }
    }
}
