using FluentValidation;
using PSK2025.ApiService.Services.Interfaces;
using FluentValidation.Results;

namespace PSK2025.ApiService.Services;

public class ValidationService(IServiceProvider serviceProvider) : IValidationService
{
    public async Task<ValidationResult> ValidateAsync<T>(T request, CancellationToken cancellationToken = default)
    {
        var validator = serviceProvider.GetService(typeof(IValidator<T>)) as IValidator<T>;
        if (validator == null)
        {
            throw new InvalidOperationException($"No validator found for {typeof(T).Name}");
        }

        return await validator.ValidateAsync(request, cancellationToken);
    }
}