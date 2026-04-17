using MISA_WEB06.Common.Attribute;
using MISA_WEB06.Common.Base;
using MISA_WEB06.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.Common.Model
{
    public class Employee : BaseModel
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        [Key]
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [CheckDuplicate("Mã nhân viên đã bị trùng")] // Custom attribute để kiểm tra trùng lặp mã nhân viên
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// Giới tính nhân viên
        /// </summary>
        /// 
        public int Age { get; set; }
        public EnumGender? Gender { get; set; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        /// 

        public Employee()
        {

        }

        /// <summary>
        /// Constructor có tham số
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="employeeCode"></param>
        /// <param name="employeeName"></param>
        public Employee(Guid employeeId, string employeeCode, string employeeName)
        {
            EmployeeId = employeeId;
            EmployeeCode = employeeCode;
            EmployeeName = employeeName;
        }
    }
}
