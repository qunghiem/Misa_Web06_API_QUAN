using Dapper;
using MISA_WEB06.Common.Base;
using MISA_WEB06.Common.ProcedureName;
using MISA_WEB06.DL.Interface;
using MySqlConnector;
using System;
using System.Collections.Generic;
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
    }
}
