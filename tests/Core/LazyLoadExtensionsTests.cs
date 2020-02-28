﻿using System.Reflection;
using LazyList.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace LazyList.Tests.Core
{
    public class LazyLoadExtensionsTests
    {
        private readonly Mock<IServiceCollection> _servicesMock;

        public LazyLoadExtensionsTests()
        {
            _servicesMock = new Mock<IServiceCollection>();
        }
        
        [Fact]
        public void GivenServiceCollectionWhenAddResolversShouldScanAssemblies()
        {
            _servicesMock.Setup(x => x.Add(It.Is<ServiceDescriptor>(descriptor => descriptor.ServiceType == typeof(ILazyLoadResolver) && descriptor.ImplementationType == typeof(ExpressionLazyLoadResolver<>))))
                .Verifiable();
            var services = _servicesMock.Object;
            
            services.AddResolvers(typeof(StubLazyLoadResolver).Assembly);
            
            _servicesMock.Verify(x => x.Add(It.IsAny<ServiceDescriptor>()), Times.Exactly(2));
        }
    }
}