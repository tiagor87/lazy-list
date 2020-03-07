using System.Linq;
using System.Reflection;
using LazyList.Core;
using Microsoft.Extensions.DependencyInjection;

namespace LazyList.Extensions
{
    public static class LazyLoadExtensions
    {
        public static void AddLazyList(this IServiceCollection services, params Assembly[] assemblies)
        {
            var resolvers = assemblies
                .SelectMany(x => x.DefinedTypes)
                .Where(x =>
                    x.IsClass &&
                    !x.IsAbstract &&
                    x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(ILazyLoadResolver<>)))
                .ToList();
            foreach (var resolver in resolvers)
            {
                if (resolver.GenericTypeParameters.Any())
                {
                    services.AddScoped(typeof(ILazyLoadResolver<>), resolver);
                }
                else
                {
                    var interfaces = resolver.GetInterfaces().Where(x => x.GetGenericTypeDefinition() == typeof(ILazyLoadResolver<>))
                        .ToList();
                    foreach (var interfaceType in interfaces)
                    {
                        services.AddScoped(interfaceType, resolver);
                    }
                }
            }

            services.AddSingleton<ILazyListFactory>(provider =>
            {
                var scope = provider.CreateScope();
                return new LazyListFactory(scope, scope.ServiceProvider);
            });
            
            var factory = services.BuildServiceProvider().GetService<ILazyListFactory>();
            LazyListFactory.RegisterInstance(factory);
        }
    }
}