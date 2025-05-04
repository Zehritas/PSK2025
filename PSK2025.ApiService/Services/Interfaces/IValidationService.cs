namespace PSK2025.ApiService.Services.Interfaces;
using FluentValidation.Results;

public interface IValidationService
{
    Task<ValidationResult> ValidateAsync<T>(T request, CancellationToken cancellationToken = default);
}
