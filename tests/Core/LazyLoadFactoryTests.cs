using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LazyList.Core;
using Xunit;

namespace LazyList.Tests.Core
{
    public class LazyLoadFactoryTests
    {
        private readonly LazyLoadFactory _lazyLoadFactory;

        public LazyLoadFactoryTests()
        {
            _lazyLoadFactory = new LazyLoadFactory(new[]
            {
                new LazyLoadResolver<Stub>( () => Task.FromResult(new Stub())),
            });
        }
        
        [Fact]
        public void GivenLazyLoadWhenResolveWithSameIdShouldReturnSameInstance()
        {
            var teste = _lazyLoadFactory.Resolve<Stub>(1);
            var teste2 = _lazyLoadFactory.Resolve<Stub>(1);
            teste.Should().Be(teste2);
        }
        
        [Fact]
        public void GivenLazyLoadWhenResolveWithDifferentParametersShouldReturnDifferentInstances()
        {
            var teste = _lazyLoadFactory.Resolve<Stub>(1);
            var teste2 = _lazyLoadFactory.Resolve<Stub>(2);
            teste.Should().NotBe(teste2);
        }

        [Fact]
        public async Task GivenLazyLoadWhenResolveInParallelTasksShouldReturnSameInstance()
        {
            var tasks = new List<Task<Stub>>();
            for (var i = 0; i < 10000; i++)
            {
                tasks.Add(_lazyLoadFactory.ResolveAsync<Stub>(1));
            }

            await Task.WhenAll(tasks);
            tasks.Select(x => x.GetAwaiter().GetResult()).Distinct().Should().HaveCount(1);
        }
    }
}
