using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekstop.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? MiddleName { get; set; }

        public string? MobilePhone { get; set; }

        public DateOnly? BirthDate { get; set; }

        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public int? PositionId { get; set; }
        public string? PositionName { get; set; }

        public int? SupervisorId { get; set; }

        public int? AssistantId { get; set; }

        public string? WorkPhone { get; set; }

        public string? Email { get; set; }

        public string? Office { get; set; }

        public string? AdditionalInfo { get; set; }
    }
}
