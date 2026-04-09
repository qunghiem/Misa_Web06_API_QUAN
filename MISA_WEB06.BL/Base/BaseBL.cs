using MISA_WEB06.BL.Interface;
using MISA_WEB06.DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.BL.Base
{
    public class BaseBL<T> : IBaseBL<T>
    {
        private IBaseDL<T> _baseDL;

        public BaseBL(IBaseDL<T> baseDL) 
        {
            _baseDL = baseDL;
        }

        public Task<IEnumerable<T>> GetAll()
        {
            var res = _baseDL.GetAll();
            return res;
        }
    }
}
