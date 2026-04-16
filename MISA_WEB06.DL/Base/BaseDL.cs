using Dapper;
using MISA_WEB06.Common.Base;
using MISA_WEB06.Common.Model;
using MISA_WEB06.Common.ProcedureName;
using MISA_WEB06.DL.Interface;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.DL.Base
{
    public class BaseDL<T> : IBaseDL<T>
    {
        private string connectionString = "server=localhost;port=3306;database=misa_web06;user=root;password=root;";

        #region Lấy tất cả bản ghi
        public async Task<IEnumerable<T>> GetAll()
        {
            string storedProcedure = string.Format(ProcedureName.GetAll, typeof(T).Name);
            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.QueryAsync<T>(storedProcedure, commandType: CommandType.StoredProcedure);
                return res;
            }
        }
        #endregion


        #region Lấy bản ghi theo ID
        public async Task<T> GetById(Guid Id)
        {
            string className = typeof(T).Name;
            // chuẩn bị tên stored procedure
            string storedProcedure = string.Format(ProcedureName.GetById, className);

            // chuẩn bị tham số param
            var param = new DynamicParameters();

            param.Add($"p_{className}Id", Id);

            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.QueryFirstOrDefaultAsync<T>(
                    storedProcedure,
                    param,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
        }
        #endregion

        #region Thêm bản ghi
        public async Task<int> Insert(T entity)
        {
            string className = typeof(T).Name;
            string storedProcedure = string.Format(ProcedureName.Insert, className);

            var param = new DynamicParameters();

            var listProps = typeof(T).GetProperties();
            foreach (var prop in listProps)
            {
                param.Add($"p_{prop.Name}", prop.GetValue(entity));
            }

            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.ExecuteAsync(
                    storedProcedure,
                    param,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
        }
        #endregion

        #region Sửa bản ghi
        public async Task<int> Update(T entity)
        {
            string className = typeof(T).Name;
            string storedProcedure = string.Format(ProcedureName.Update, className);

            var param = new DynamicParameters();
            var listProps = typeof(T).GetProperties();
            foreach (var prop in listProps)
            {
                param.Add($"p_{prop.Name}", prop.GetValue(entity));
            }
            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.ExecuteAsync(
                    storedProcedure,
                    param,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
        }
        #endregion

        #region Xỏa bản ghi theo ID
        public async Task<int> DeleteById(Guid Id)
        {
            string className = typeof(T).Name;
            string storedProcedure = string.Format(ProcedureName.Delete, className);

            var param = new DynamicParameters();
            param.Add($"p_{className}Id", Id);

            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.ExecuteAsync(
                    storedProcedure,
                    param,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
        }
        #endregion

        #region Xóa nhiều bản ghi
        public async Task<int> DeleteMultiple(List<Guid> ids)
        {
            string className = typeof(T).Name;
            string storedProcedure = string.Format(ProcedureName.DeleteMultiple, className);

            var param = new DynamicParameters();
            param.Add($"p_{className}Ids", string.Join(",", ids));
            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.ExecuteAsync(
                    storedProcedure,
                    param,
                    commandType: CommandType.StoredProcedure
                );
                return res;
            }
        }
        #endregion


        /// <summary>
        /// Check xem có bị trùng hay không
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> CheckDuplicate(string propertyName, object value, T entity)
        {
            string className = typeof(T).Name;
            string storedProcedure = string.Format(ProcedureName.CheckDuplicate, className);

            var param = new DynamicParameters();
            // Lấy ra thuộc tính là khóa chính của T
            var primaryKeyName = "";
            object primaryKeyValue = null;
            // danh sach thuoc tinh cua T
            var listProps = typeof(T).GetProperties();
            foreach (var prop in listProps)
            {
                // kiểm tra xem prop có phải là khóa chính hay không
                var isPrimaryKey = Attribute.IsDefined(prop, typeof(KeyAttribute));
                if (isPrimaryKey)
                {
                    primaryKeyName = prop.Name;
                    primaryKeyValue = prop.GetValue(entity);
                }
            }
            param.Add($"p_{propertyName}", value);

            // truyền thêm khóa chính vào param nếu có
            if(string.IsNullOrEmpty(primaryKeyName))
            {
                throw new Exception($"Không tìm thấy khóa chính");
            } else
            {
                param.Add($"p_{primaryKeyName}", primaryKeyValue);
            }

            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.QueryFirstOrDefaultAsync<int>(
                    storedProcedure,
                    param,
                    commandType: CommandType.StoredProcedure
                );
                return res > 0;
            }
        }


        public async Task<PagedResult<T>> Search(string? keyword, int pageIndex, int pageSize)
        {
            string className = typeof(T).Name;
            string storedProcedure = string.Format(ProcedureName.Search, className);
            var param = new DynamicParameters();

            param.Add("p_Keyword", keyword ?? "");
            param.Add("p_PageNumber", pageIndex);
            param.Add("p_PageSize", pageSize);

            using var cnn = new MySqlConnection(connectionString);
            using var multi = await cnn.QueryMultipleAsync(
                storedProcedure,
                param,
                commandType: CommandType.StoredProcedure
            );
            var totalCount = await multi.ReadFirstOrDefaultAsync<int>();

            var data = await multi.ReadAsync<T>();

            return new PagedResult<T>
            {
                Data = data,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
    }
}
