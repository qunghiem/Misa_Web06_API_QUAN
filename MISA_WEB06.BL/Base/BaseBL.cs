using MISA_WEB06.BL.Interface;
using MISA_WEB06.Common.Attribute;
using MISA_WEB06.Common.Model;
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
            _baseDL = baseDL; // Nhận được và cất vào biến dùng chung
        }

        /// <summary>
        /// Hàm lấy tất cả đối tuợng 
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAll()
        {
            var res = _baseDL.GetAll();
            return res;
        }

        /// <summary>
        /// Hàm lấy 1 đối tuợng theo Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<T> GetById(Guid Id)
        {
            var res = await _baseDL.GetById(Id);
            return res;
        }

        /// <summary>
        /// Hàm thêm 1 đối tuợng 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Insert(T entity)
        { 
            var isValid = await ValidateBussiness(entity);
            if(!isValid)
            {
                throw new Exception();
            }
            var res =await _baseDL.Insert(entity);
            return res; 
        }

        /// <summary>
        ///  Validate tổng hợp
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> ValidateBussiness(T entity)
        {
            var result = true;

            // Validate check trùng
            result = await ValidateDuplicate(entity);

            // Validate các nghiệp vụ khác...
            return result;
            
        }

        /// <summary>
        /// Validate trùng lặp
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> ValidateDuplicate(T entity)
        {
            // lấy ra danh sách thuộc tính
            var listProperty = typeof(T).GetProperties();

            foreach (var props in listProperty)
            {
                // kiểm tra thuộc tính props đó có được gán nhãn CheckDuplicateAttribute hay không
                var attr = Attribute.GetCustomAttribute(props, typeof(CheckDuplicateAttribute)) as CheckDuplicateAttribute;
                // Nếu không
                if (attr == null)
                {
                    continue;
                }

                // Nếu có thì sẽ đi check 

                // Lấy ra giá trị của thuộc tính đó
                var value = props.GetValue(entity);

                //Thực hiện check trùng cho thuộc tính đó
                // props.Name: tên thuộc tính đang được check trùng
                // value: giá trị của thuộc tính đang được check trùng
                var res = await _baseDL.CheckDuplicate(props.Name, value, entity);
                if (res == true)
                {
                    throw new Exception(attr.ErrorMessage);
                }
            }
            return true;
        }

        /// <summary>
        /// Hàm cập nhật 1 đối tuợng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Update(T entity)
        {

            var isValid = await ValidateBussiness(entity);
            if(!isValid)
            {
                throw new Exception();
            }
            var res = await _baseDL.Update(entity);
            return res;
        }

        /// <summary>
        /// Hàm xóa 1 đối tuợng theo Id 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteById(Guid Id)
        {
            var res = await _baseDL.DeleteById(Id);
            return res;
        }

        /// <summary>
        /// Hàm xóa nhiều đối tuợng theo Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteMultiple(List<Guid> ids)
        {
            var res = await _baseDL.DeleteMultiple(ids);
            return res;
        }

        public async Task<PagedResult<T>> Search(string? keyword, int pageIndex, int pageSize)
        {
            // bắt đầu từ trang 1
            if (pageIndex < 1) pageIndex = 1;

            // set page size
            if(pageSize < 1) pageSize = 10;
            
            return await _baseDL.Search(keyword, pageIndex, pageSize);
        }
    }
}
