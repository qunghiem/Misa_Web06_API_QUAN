using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.Common.Attribute
{
    /// <summary>
    /// Tạo attribute để đánh dấu các property cần kiểm tra trùng lặp khi thêm mới hoặc cập nhật
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckDuplicateAttribute : System.Attribute
    {
        public string ErrorMessage { get; set; }

        public CheckDuplicateAttribute(string errorMessage = "")
        {
            ErrorMessage = errorMessage;
        }
    }
}
