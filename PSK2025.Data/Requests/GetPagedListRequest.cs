using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Requests;

public record GetPagedListRequest(int PageNumber, int PageSize);
