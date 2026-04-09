using Dapper;
using MISA_WEB06.Common.Base;
using MISA_WEB06.Common.ProcedureName;
using MISA_WEB06.DL.Interface;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA_WEB06.DL.Base
{
    public class BaseDL<BaseModel> : IBaseDL<BaseModel>
    {
        private string connectionString = "server=localhost;port=3306;database=misa_web06.api;user=root;password=root;";
        
        public async Task<IEnumerable<BaseModel>> GetAll()
        {
            string storeProcureName = string.Format(ProcedureName.GetAll, typeof(BaseModel).Name);

            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.QueryAsync<BaseModel>(storeProcureName, commandType: System.Data.CommandType.StoredProcedure);
                return res;
            }
        }
    }
}
