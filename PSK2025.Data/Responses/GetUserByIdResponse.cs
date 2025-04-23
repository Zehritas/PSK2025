using PSK2025.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Responses;

public record GetUserByIdResponse(UserDto User);
