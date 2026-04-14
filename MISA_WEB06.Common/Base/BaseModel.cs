using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.Common.Base
{
    /// <summary>
    /// chứa các thuộc tính chung cho tất cả các model trong hệ thống
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }
        
        /// <summary>
        /// Ngày sửa đổi gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }
       
        /// <summary>
        /// Người sửa đổi gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
