using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Models.DTOs;

public record TaskDto(
    Guid Id,
    Guid? UserID,
    DateTime CreatedAt,
    DateTime? ClosedAt,
    string TaskStatus);
