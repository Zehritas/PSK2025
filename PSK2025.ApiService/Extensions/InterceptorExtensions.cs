using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PSK2025.ApiService.Interceptor;
namespace PSK2025.ApiService.Extensions;

public static class InterceptorExtensions
{
    public static IServiceCollection AddBusinessOperationLogging(
        this IServiceCollection services,
        Func<Type, bool>? interfaceFilter = null)
    {
        services.AddSingleton<IProxyGenerator, ProxyGenerator>();

        services.AddTransient<LoggingInterceptor>();

        var serviceDescriptors = services
        .Where(descriptor =>
            descriptor.ServiceType.IsInterface &&
            !descriptor.ServiceType.IsGenericTypeDefinition &&
            descriptor.ImplementationType != null &&
            (interfaceFilter == null || interfaceFilter(descriptor.ServiceType)) &&
            descriptor.Lifetime != ServiceLifetime.Singleton) 
        .ToList();

        foreach (var descriptor in serviceDescriptors)
        {
            services.Remove(descriptor);

            services.Add(new ServiceDescriptor(
                descriptor.ServiceType,
                serviceProvider =>
                {
                    var proxyGenerator = serviceProvider.GetRequiredService<IProxyGenerator>();
                    var actual = serviceProvider.GetRequiredService(descriptor.ImplementationType!);
                    var interceptor = serviceProvider.GetRequiredService<LoggingInterceptor>();

                    return proxyGenerator.CreateInterfaceProxyWithTarget(
                        descriptor.ServiceType,
                        actual,
                        interceptor);
                },
                descriptor.Lifetime));

            services.Add(new ServiceDescriptor(
                descriptor.ImplementationType!,
                descriptor.ImplementationType!,
                descriptor.Lifetime));
        }

        return services;
    }
}