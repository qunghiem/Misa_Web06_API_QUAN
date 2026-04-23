using Dapper;
using MISA_WEB06.Common;
using MISA_WEB06.Common.Attribute;
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
using System.Text.Json;
using System.Threading.Tasks;

namespace MISA_WEB06.DL.Base
{
    public class BaseDL<T> : IBaseDL<T>
    {
        private string connectionString = "server=localhost;port=3306;database=misa_web06;user=root;password=root;";

        #region Lấy tất cả bản ghi
        public async Task<IEnumerable<T>> GetAll()
        {
            //string storedProcedure = string.Format(ProcedureName.GetAll, typeof(T).Name);

            //using (var cnn = new MySqlConnection(connectionString))
            //{
            //    var res = await cnn.QueryAsync<T>(storedProcedure, commandType: CommandType.StoredProcedure);
            //    return res;
            //}

            var tableName = typeof(T).Name;
            string sql = $"SELECT * FROM {tableName}";

            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.QueryAsync<T>(sql, commandType: CommandType.Text);
                return res;
            }
        }
        #endregion


        #region Lấy bản ghi theo ID
        public async Task<T> GetById(Guid Id)
        {
            //string className = typeof(T).Name;
            //// chuẩn bị tên stored procedure
            //string storedProcedure = string.Format(ProcedureName.GetById, className);

            //// chuẩn bị tham số param
            //var param = new DynamicParameters();

            //param.Add($"p_{className}Id", Id);

            //using (var cnn = new MySqlConnection(connectionString))
            //{
            //    var res = await cnn.QueryFirstOrDefaultAsync<T>(
            //        storedProcedure,
            //        param,
            //        commandType: CommandType.StoredProcedure
            //    );
            //    return res;
            //}

            var tableName = typeof(T).Name;
            string sql = $"SELECT * FROM {tableName} WHERE {tableName}Id = @Id";
            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.QueryFirstOrDefaultAsync<T>(
                    sql,
                    new { Id },
                    commandType: CommandType.Text
                );
                return res;
            }
        }
        #endregion

        #region Thêm bản ghi
        public async Task<int> Insert(T entity)
        {
            //string className = typeof(T).Name;
            //string storedProcedure = string.Format(ProcedureName.Insert, className);

            //var param = new DynamicParameters();

            //var listProps = typeof(T).GetProperties();
            //foreach (var prop in listProps)
            //{
            //    param.Add($"p_{prop.Name}", prop.GetValue(entity));
            //}

            //using (var cnn = new MySqlConnection(connectionString))
            //{
            //    var res = await cnn.ExecuteAsync(
            //        storedProcedure,
            //        param,
            //        commandType: CommandType.StoredProcedure
            //    );
            //    return res;
            //}
            string tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties();

            var primaryKeyProp = properties.FirstOrDefault(p => p.Name == $"{tableName}Id");
            if (primaryKeyProp != null)
            {
                var currentValue = primaryKeyProp.GetValue(entity);
                // Nếu ID đang là Guid trống (toàn số 0), hãy cấp cho nó một Guid mới
                if (currentValue is Guid guidValue && guidValue == Guid.Empty)
                {
                    primaryKeyProp.SetValue(entity, Guid.NewGuid());
                }
            }

            // 1. Lấy danh sách tên cột (ví dụ: Name, Age, Email)
            var columnNames = properties.Select(p => p.Name);

            // 2. Tạo danh sách tham số tương ứng (ví dụ: @Name, @Age, @Email)
            var paramNames = properties.Select(p => $"@{p.Name}");

            // 3. Xây dựng câu lệnh SQL Insert động
            string sql = $@"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) 
                    VALUES ({string.Join(", ", paramNames)})";

            using (var cnn = new MySqlConnection(connectionString))
            {
                // Dapper sẽ tự động map các thuộc tính của 'entity' vào các tham số @PropertyName
                var res = await cnn.ExecuteAsync(sql, entity, commandType: CommandType.Text);
                return res;
            }

        }
        #endregion

        #region Sửa bản ghi
        public async Task<int> Update(T entity)
        {
            //Cách 1: dùng Procedure 
            //string className = typeof(T).Name;
            //string storedProcedure = string.Format(ProcedureName.Update, className);

            //var param = new DynamicParameters();
            //var listProps = typeof(T).GetProperties();
            //foreach (var prop in listProps)
            //{
            //    param.Add($"p_{prop.Name}", prop.GetValue(entity));
            //}
            //using (var cnn = new MySqlConnection(connectionString))
            //{
            //    var res = await cnn.ExecuteAsync(
            //        storedProcedure,
            //        param,
            //        commandType: CommandType.StoredProcedure
            //    );
            //    return res;
            //}

            var tableName = typeof(T).Name;

            var properties = typeof(T).GetProperties();

            var columnNames = properties.Select(p => p.Name);

            var paramNames = properties.Select(p => $"@{p.Name}");

            var sql = $@"UPDATE {tableName} 
                    SET {String.Join(", ", columnNames.Zip(paramNames, (col, param) => $"{col} = {param}"))}
                    WHERE {tableName}Id = @{tableName}Id";

            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.ExecuteAsync(
                        sql,
                        entity,
                        commandType: CommandType.Text
                );
                return res;
            }
        }
        #endregion

        #region Xỏa bản ghi theo ID
        public async Task<int> DeleteById(Guid Id)
        {
            //string className = typeof(T).Name;
            //string storedProcedure = string.Format(ProcedureName.Delete, className);

            //var param = new DynamicParameters();
            //param.Add($"p_{className}Id", Id);

            //using (var cnn = new MySqlConnection(connectionString))
            //{
            //    var res = await cnn.ExecuteAsync(
            //        storedProcedure,
            //        param,
            //        commandType: CommandType.StoredProcedure
            //    );
            //    return res;
            //}
            var tableName = typeof(T).Name;

            var properties = typeof(T).GetProperties();

            var columnNames = properties.Select(p => p.Name);

            var sql = @$"DELETE from {tableName} where {tableName}Id = @Id";

            using(var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.ExecuteAsync(sql, new { Id = Id }, commandType: CommandType.Text);
                return res;
            }
        }
        #endregion

        #region Xóa nhiều bản ghi
        public async Task<int> DeleteMultiple(List<Guid> ids)
        {
            //string className = typeof(T).Name;
            //string storedProcedure = string.Format(ProcedureName.DeleteMultiple, className);

            //var param = new DynamicParameters();
            //param.Add($"p_{className}Ids", string.Join(",", ids));
            //using (var cnn = new MySqlConnection())
            //{
            //    var res = await cnn.ExecuteAsync(
            //        storedProcedure,
            //        param,
            //        commandType: CommandType.StoredProcedure
            //    );
            //    return res;
            //}

            var tableName = typeof(T).Name;

            var sql = @$"DELETE from {tableName} WHERE {tableName}Id IN @Ids";

            using (var cnn = new MySqlConnection(connectionString))
            {
                var res = await cnn.ExecuteAsync(sql, new { Ids = ids }, commandType: CommandType.Text);
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
            //string className = typeof(T).Name;
            //string storedProcedure = string.Format(ProcedureName.CheckDuplicate, className);

            //var param = new DynamicParameters();
            //// Lấy ra thuộc tính là khóa chính của T
            //var primaryKeyName = "";
            //object primaryKeyValue = null;
            //// danh sach thuoc tinh cua T
            //var listProps = typeof(T).GetProperties();
            //foreach (var prop in listProps)
            //{
            //    // kiểm tra xem prop có phải là khóa chính hay không
            //    var isPrimaryKey = Attribute.IsDefined(prop, typeof(KeyAttribute));
            //    if (isPrimaryKey)
            //    {
            //        primaryKeyName = prop.Name;
            //        primaryKeyValue = prop.GetValue(entity);
            //    }
            //}
            //param.Add($"p_{propertyName}", value);

            //// truyền thêm khóa chính vào param nếu có
            //if(string.IsNullOrEmpty(primaryKeyName))
            //{
            //    throw new Exception($"Không tìm thấy khóa chính");
            //} else
            //{
            //    param.Add($"p_{primaryKeyName}", primaryKeyValue);
            //}

            //using (var cnn = new MySqlConnection(connectionString))
            //{
            //    var res = await cnn.QueryFirstOrDefaultAsync<int>(
            //        storedProcedure,
            //        param,
            //        commandType: CommandType.StoredProcedure
            //    );
            //    return res > 0;
            //}

            // Cách 2: dùng Dynamic SQL
            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties();
            var columnNames = properties.Select(p => p.Name);
            var paramNames = properties.Select(p => $"@{p.Name}");

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
            var sql = $@"SELECT COUNT(*) 
                      FROM {tableName} 
                      WHERE {propertyName} = @Value
                      AND {primaryKeyName} <> @Id;";

            using (var cnn = new MySqlConnection(connectionString)) 
            {
                var res = await cnn.ExecuteScalarAsync<int>(
                    sql, 
                    new { Value = value, Id = primaryKeyValue });

                return res > 0;
            }
        }



        //Cách 1: dùng Procedure để tìm kiếm phân trang
        //public async Task<PagedResult<T>> Search(string? keyword, int pageIndex, int pageSize)
        //{
        //    string className = typeof(T).Name;
        //    string storedProcedure = string.Format(ProcedureName.Search, className);
        //    var param = new DynamicParameters();

        //    param.Add("p_Keyword", keyword ?? "");
        //    param.Add("p_PageNumber", pageIndex);
        //    param.Add("p_PageSize", pageSize);

        //    using var cnn = new MySqlConnection(connectionString);
        //    using var multi = await cnn.QueryMultipleAsync(
        //        storedProcedure,
        //        param,
        //        commandType: CommandType.StoredProcedure
        //    );
        //    var totalCount = await multi.ReadFirstOrDefaultAsync<int>();

        //    var data = await multi.ReadAsync<T>();

        //    return new PagedResult<T>
        //    {
        //        Data = data,
        //        TotalCount = totalCount,
        //        PageIndex = pageIndex,
        //        PageSize = pageSize
        //    };
        //}

        //Cách 2: sử dụng Query động để tìm kiếm, phân trang kết hợp lọc nhiều điều kiện cùng 1 lúc
        public async Task<PagedResult<T>> Search(string? keyword, int pageIndex, int pageSize, List<FilterItem>? filters = null)
        {
            var tableName = typeof(T).Name;
            // Lấy ra danh sách cột có type là string để search
            var searchableColumns = typeof(T).GetProperties().Where(p => p.PropertyType == typeof(string)).Select(p => p.Name).ToList();
            var conditions = new List<string> { "1=1" };
            var param = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(keyword) && searchableColumns.Any())
            {
                var likes = searchableColumns.Select(col => $"{col} LIKE @keyword");
                conditions.Add($"({string.Join(" OR ", likes)})");
                param.Add("keyword", $"%{keyword}%");
            }

            // Xử lý filter động
            if (filters?.Count > 0)
            {
                for (int i = 0; i < filters.Count; i++)
                {
                    string op = filters[i].Operator == "==" ? "=" : filters[i].Operator;
                    string pName = $"f_param_{i}";
                    conditions.Add($"{filters[i].Field} {op} @{pName}");
                    param.Add(pName, GetValueFromJsonElement(filters[i].Value));
                }
            }
            var whereClause = string.Join(" AND ", conditions);

            var offSet = (pageIndex - 1) * pageSize;
            param.Add("offSet", offSet);
            param.Add("pageSize", pageSize);

            var sql = @$"SELECT COUNT(*) FROM {tableName} WHERE {whereClause};
                        SELECT * FROM {tableName} WHERE {whereClause} ORDER BY CreatedDate DESC LIMIT @pageSize OFFSET @offSet;";

            using var  cnn = new MySqlConnection(connectionString);
            using var multi = await cnn.QueryMultipleAsync(
                sql, param
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

        // Hàm bổ trợ để xử lý JsonElement
        private object GetValueFromJsonElement(object value)
        {
            if (value is JsonElement element)
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.String:
                        return element.GetString();
                    case JsonValueKind.Number:
                        // Tự động nhận diện số nguyên hoặc số thực
                        if (element.TryGetInt32(out int intVal)) return intVal;
                        if (element.TryGetInt64(out long longVal)) return longVal;
                        return element.GetDouble();
                    case JsonValueKind.True:
                        return true;
                    case JsonValueKind.False:
                        return false;
                    case JsonValueKind.Null:
                        return null;
                    default:
                        return element.GetRawText();
                }
            }
            return value;
        }
    }
}
