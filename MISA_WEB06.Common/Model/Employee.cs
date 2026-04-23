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

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Country { get; set; }
        public string? Province { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public string? CandidateSource { get; set; }
        public string? RecentWorkplace { get; set; }
        public string? Recruiter { get; set; }
        public string? EducationLevel { get; set; }
        public string? EducationPlace { get; set; }
        public string? Major { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string? Avatar { get; set; }
        public string? Cv { get; set; }


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
