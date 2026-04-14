using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.Common.Model
{
    public class ErrorResult
    {
        public string? DevMsg { get; set; }
        public string? UserMsg { get; set; }
        public object? MoreInfo { get; set; }
        public Guid? TraceID { get; set; }

        public ErrorResult()
        {
            
        }
    }
}
