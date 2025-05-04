using System.Net;
using PSK2025.Data;

namespace PSK2025.ApiService.Extensions;

public static class ErrorExtensions
{
    public static IResult MapErrorToResponse(this Error error)
    {
        return error.HttpStatusCode switch
        {
            HttpStatusCode.Conflict => Results.Conflict(error),
            HttpStatusCode.NotFound => Results.NotFound(),
            HttpStatusCode.BadRequest => Results.BadRequest(error),
            HttpStatusCode.Unauthorized => Results.Json(error, statusCode: (int)HttpStatusCode.Unauthorized),
            HttpStatusCode.Forbidden => Results.Json(error, statusCode: (int)HttpStatusCode.Forbidden),
            HttpStatusCode.UnprocessableEntity => Results.UnprocessableEntity(error),
            _ => Results.Json(error, statusCode: (int)HttpStatusCode.InternalServerError)
        };
    }
}