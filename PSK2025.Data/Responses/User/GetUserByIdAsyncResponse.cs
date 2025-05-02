using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSK2025.Models.DTOs;

namespace PSK2025.Data.Responses;

public record GetUserByIdAsyncResponse(UserDto User);
