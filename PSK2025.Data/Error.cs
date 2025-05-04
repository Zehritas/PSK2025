using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace PSK2025.Data;

public sealed record Error(string Code, string Description, HttpStatusCode? HttpStatusCode = HttpStatusCode.InternalServerError)
{
    public static readonly Error None = new(string.Empty, string.Empty, System.Net.HttpStatusCode.OK);
    public static readonly Error NullValue = new("Error:NullValue", "Null value was provided", System.Net.HttpStatusCode.BadRequest);
    public static Error ValidationError(Error[] errors) => new(
        "ValidationError",
        string.Join(",\n", errors.Select(e => e.Description)),
        System.Net.HttpStatusCode.UnprocessableEntity);
    public static Error ValidationError(IEnumerable<ValidationFailure> validationFailures) => new(
        "ValidationError",
        string.Join(", \n", validationFailures.Select(x => x.ErrorMessage)),
        System.Net.HttpStatusCode.UnprocessableEntity);

    public static implicit operator Result(Error error) => Result.Failure(error);
}