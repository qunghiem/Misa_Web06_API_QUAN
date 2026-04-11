using MISA_WEB06.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.DL.Interface
{
    public interface IBaseDL<T>
    {
        public Task<IEnumerable<T>> GetAll();

        public Task<T> GetById(Guid Id);

        public Task<int> Insert(T entity);

        public Task<int> Update(T entity);

        public Task<int> DeleteById(Guid Id);
    }
}
