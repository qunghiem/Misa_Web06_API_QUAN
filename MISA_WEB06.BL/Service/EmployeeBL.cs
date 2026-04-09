using MISA_WEB06.BL.Base;
using MISA_WEB06.BL.Interface;
using MISA_WEB06.Common.Model;
using MISA_WEB06.DL.Base;
using MISA_WEB06.DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.BL.Service
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        public EmployeeBL(BaseDL<Employee> baseDL) : base(baseDL)
        {
        }
    }
}
