using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekstop.Models
{
    public class EventEmployeeRequest
    {
        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public DateOnly? DateStart { get; set; }

        public DateOnly? DateEnd { get; set; }

        public int? EventEmployeeTypeId { get; set; }
    }
}
