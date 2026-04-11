using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.BL.Interface
{
    public interface IBaseBL<T>
    {

        #region Lấy tất cả bản ghi
        public Task<IEnumerable<T>> GetAll();
        #endregion

        #region Lấy bản ghi theo ID
        public Task<T> GetById(Guid Id);
        #endregion

        #region Thêm bản ghi
        public Task<int> Insert(T entity);
        #endregion

        #region Cập nhật bản ghi
        public Task<int> Update(T entity);
        #endregion


        #region Xóa bản ghi theo ID
        public Task<int> DeleteById(Guid id); 
        #endregion
    }
}
