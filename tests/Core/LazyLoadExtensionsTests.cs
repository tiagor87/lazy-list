using System;
using System.Collections.Generic;
using FluentAssertions;
using LazyList.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace LazyList.Tests.Core
{
    [CollectionDefinition("LazyListFactory", DisableParallelization = false)]
    public class LazyLoadExtensionsTests : IDisposable
    {
        private readonly Mock<IServiceCollection> _servicesMock;

        public LazyLoadExtensionsTests()
        {
            _servicesMock = new Mock<IServiceCollection>();
        }
        
        [Fact]
        public void GivenServiceCollectionWhenAddLazyListShouldScanAssemblies()
        {
            _servicesMock.Setup(x => x.Add(It.IsAny<ServiceDescriptor>()))
                .Verifiable();
            var services = _servicesMock.Object;
            
            services.AddLazyList(typeof(StubLazyLoadResolver).Assembly);
            
            _servicesMock.Verify(x => x.Add(It.Is<ServiceDescriptor>(descriptor => descriptor.ServiceType == typeof(ILazyLoadResolver))), Times.Once());
            _servicesMock.Verify(x => x.Add(It.Is<ServiceDescriptor>(descriptor => descriptor.ServiceType == typeof(ILazyListFactory))), Times.Once());
        }
        
        [Fact]
        public void GivenServiceCollectionWhenAddLazyListShouldInitializeFactory()
        {
            var services = new ServiceCollection();
            services.AddLazyList(typeof(StubLazyLoadResolver).Assembly);
            var provider = services.BuildServiceProvider();
            
            Func<IList<Stub>> action = () =>
            {
                using var _ = provider.CreateScope();
                return LazyListFactory.CreateList<Stub>(1);
            };

            action.Should().NotThrow();
        }

        public void Dispose()
        {
            LazyListFactory.Init(null);
        }
    }
}