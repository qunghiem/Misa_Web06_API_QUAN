using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.Common.ProcedureName
{
    public static class ProcedureName
    {
        /// <summary>
        /// Procedure cho Lấy tất cả bản ghi
        /// </summary>
        public static string GetAll = "Proc_{0}_GetAll";

        /// <summary>
        /// Procedure cho Lấy bản ghi theo ID
        /// </summary>
        public static string GetById = "Proc_{0}_GetById";

        /// <summary>
        /// Procedure cho Thêm bản ghi
        /// </summary>
        public static string Insert = "Proc_{0}_Insert";

        /// <summary>
        /// Cập nhật bản ghi
        /// </summary>
        public static string Update = "Proc_{0}_Update";

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        public static string Delete = "Proc_{0}_Delete";
    }
}
