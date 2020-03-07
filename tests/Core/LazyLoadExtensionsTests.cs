using System;
using System.Collections.Generic;
using FluentAssertions;
using LazyList.Core;
using LazyList.Extensions;
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
            
            services.AddLazyList(typeof(StubLazyLoadResolver<>).Assembly);
            
            _servicesMock.Verify(x => x.Add(It.Is<ServiceDescriptor>(descriptor => descriptor.ServiceType == typeof(ILazyLoadResolver<>))), Times.Once());
            _servicesMock.Verify(x => x.Add(It.Is<ServiceDescriptor>(descriptor => descriptor.ServiceType == typeof(ILazyListFactory))), Times.Once());
        }
        
        [Fact]
        public void GivenServiceCollectionWhenAddLazyListShouldInitializeFactory()
        {
            var services = new ServiceCollection();
            services.AddLazyList(typeof(StubLazyLoadResolver<>).Assembly);
            
            Func<IList<Stub>> action = () => LazyListFactory.CreateList<Stub>(1);

            action.Should().NotThrow();
        }

        [Fact]
        public void GivenServiceCollectionWhenAddLazyListShouldRegisterOpenTypeGenericsAndSpecificTypes()
        {
            var services = new ServiceCollection();
            services.AddLazyList(typeof(StubLazyLoadResolver<>).Assembly);

            var stubs = LazyListFactory.CreateList<Stub>(1);

            stubs.Should().NotBeNull();
            stubs.Should().BeOfType<LazyList<Stub>>();
        }

        public void Dispose()
        {
            LazyListFactory.RegisterInstance(null);
        }
    }
}