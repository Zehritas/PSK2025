using PSK2025.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Requests.Task;

public record GetTasksRequest(Guid? ProjectId, String? UserId, PriorityStatus? Priority, TaskEntityStatus? Status, GetPagedListRequest Pagination);