using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Requests.Auth;

public record RegisterNewUserRequest(string FirstName, string LastName, string Email, string Password);