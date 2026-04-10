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
        //public async Task<T> GetById(Guid Id)
        //{
        //    string storedProcedure = string.Format(ProcedureName.GetById, typeof(T).Name);
        //    using (var cnn = new MySqlConnection(connectionString))
        //    {
        //        var res = await cnn.QueryAsync<T>(storedProcedure, commandType: CommandType.StoredProcedure);
        //        return res;
        //    }
        //}
        #endregion

        public async Task<T> GetByID(Guid Id)
        {
            string className = typeof(T).Name;
            string storeProcedure = string.Format(ProcedureName.GetById, typeof(BaseModel).Name);
            var param = new DynamicParameters();
            param.Add($"p_{className}Id", Id);
            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.QueryFirstOrDefaultAsync<T>(storeProcedure, param, commandType: CommandType.StoredProcedure);
                return res;
            }
        }

    }
}
