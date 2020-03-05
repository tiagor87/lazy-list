using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LazyList.Core
{
    public static class LazyLoadExtensions
    {
        public static void AddLazyList(this IServiceCollection services, params Assembly[] assemblies)
        {
            var resolvers = assemblies.Concat(new [] { typeof(ILazyLoadResolver).Assembly })
                .SelectMany(x => x.DefinedTypes)
                .Where(x => typeof(ILazyLoadResolver).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract && x != typeof(ExpressionLazyLoadResolver<>))
                .ToList();
            foreach (var resolver in resolvers)
            {
                services.AddScoped(typeof(ILazyLoadResolver), resolver);
            }

            services.AddScoped<ILazyListFactory, LazyListFactory>();
            LazyListFactory.Init(services.BuildServiceProvider);
        }
    }
}