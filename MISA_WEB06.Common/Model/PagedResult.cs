using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.Common.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T>
    {
        // danh sách kết quả trả về
        public IEnumerable<T> Data { get; set; }

        // tổng số bản ghi tìm được
        public int TotalCount { get; set; }

        // số trang(index), chỉ mục
        public int PageIndex { get; set; }

        // size của trang
        public int PageSize { get; set; }


        // tổng số trang, làm tròn lên
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    }
}
