using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data;

public class Error
{
    public string Code { get; set; }
    public string Message { get; set; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}
