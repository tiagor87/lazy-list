using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LazyList.Core;
using Xunit;

namespace LazyList.Tests.Core
{
    public class ExpressionLazyLoadResolverTests
    {
        [Fact]
        public void GivenLazyResolverWhenResolveAndParameterNullShouldThrow()
        {
            var resolver = new ExpressionLazyLoadResolver<string>((_) => Task.FromResult(string.Empty));
            Func<object> action = () => resolver.Resolve(null);
            action.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public async Task GivenLazyLoadWhenResolveInParallelTasksShouldReturnSameInstance()
        {
            var resolver = new ExpressionLazyLoadResolver<Stub>((_) => Task.FromResult(new Stub()));
            var tasks = new List<Task<Stub>>();
            for (var i = 0; i < 10000; i++)
            {
                tasks.Add(resolver.ResolveAsync(1));
            }

            await Task.WhenAll(tasks);
            tasks.Select(x => x.GetAwaiter().GetResult()).Distinct().Should().HaveCount(1);
        }
    }
}