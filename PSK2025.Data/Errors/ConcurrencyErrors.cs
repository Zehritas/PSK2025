using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Errors;

public static class ConcurrencyErrors
{
    public static readonly Error OptimisticLockingError = new(
        "Concurrency.OptimisticLocking",
        "The record was modified by another user.",
        HttpStatusCode.Conflict);
}
