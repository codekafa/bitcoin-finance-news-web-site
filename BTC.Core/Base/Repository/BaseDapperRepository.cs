using BTC.Core.Base.Attributes;
using BTC.Core.Base.Connection;
using BTC.Core.Base.CustomException;
using BTC.Core.Base.Data;
using BTC.Core.Base.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Core.Base.Repository
{
    public class BaseDapperRepository<DBCON, T> : IRepositoryBase<T> where T : class, IEntity where DBCON : BaseDatabaseConnection, new()
    {
        private BaseDatabaseConnection _connectionFactory;

        Type type = typeof(T);

        System.Reflection.PropertyInfo[] propInfo;
        public BaseDapperRepository()
        {
            _connectionFactory = new DBCON();
        }

        public int Insert(T entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_insert))
                    GetInsert();

                using (var db = _connectionFactory.GetConnection)
                {
                    int value = db.ExecuteScalar<int>(_insert, entity);
                    return value;
                }

            }
            catch (Exception ex)
            {
                _connectionFactory.Dispose();
                throw new DatabaseException(ex.Message, ex);
            }

        }

        public bool InsertSqlBulkCopy(List<T> list)
        {
            try
            {
                using (SqlBulkCopy copy = new SqlBulkCopy(_connectionFactory.ConnectionString))
                {
                    copy.BulkCopyTimeout = 180;
                    PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
                    DataTable table = new DataTable();
                    for (int i = 0; i < propertyDescriptorCollection.Count; i++)
                    {
                        PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                        Type propType = propertyDescriptor.PropertyType;

                        copy.ColumnMappings.Add(propertyDescriptor.Name, propertyDescriptor.Name);

                        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            table.Columns.Add(propertyDescriptor.Name, Nullable.GetUnderlyingType(propType));
                        }
                        else
                        {
                            table.Columns.Add(propertyDescriptor.Name, propType);
                        }
                    }
                    object[] values = new object[propertyDescriptorCollection.Count];
                    foreach (T listItem in list)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = propertyDescriptorCollection[i].GetValue(listItem);
                        }
                        table.Rows.Add(values);
                    }
                    copy.DestinationTableName = type.Name;
                    copy.WriteToServer(table);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }

        }

        public bool Delete(T entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_delete))
                    GetDelete();
                using (var db = _connectionFactory.GetConnection)
                {
                    int value = db.ExecuteScalar<int>(_delete, entity);
                    return true;
                }


            }
            catch (Exception ex)
            {
                _connectionFactory.Dispose();
                throw new DatabaseException(ex.Message, ex);
            }

        }

        public bool Update(T entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_update))
                    GetUpdate();
                using (var db = _connectionFactory.GetConnection)
                {
                    int value = db.ExecuteScalar<int>(_update, entity);
                    return true;
                }

            }
            catch (Exception ex)
            {
                _connectionFactory.Dispose();
                throw new DatabaseException(ex.Message, ex);
            }

        }

        public List<T> GetByCustomQuery(string query, object param)
        {
            try
            {
                using (var db = _connectionFactory.GetConnection)
                {
                    var result = db.Query<T>(query, param).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                _connectionFactory.Dispose();
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public T GetByID(int Id)
        {
            type = typeof(T);
            propInfo = type.GetProperties();
            if (string.IsNullOrWhiteSpace(_select))
                GetSelect();
            string query = _select + " where " + propInfo[0].Name + " = @" + propInfo[0].Name;

            using (var db = _connectionFactory.GetConnection)
            {
                var result = db.Query<T>(query, new { ID = Id }).FirstOrDefault();
                return result;
            }


        }

        public bool ExecuteQuery(string query, object param = null)
        {
            try
            {
                using (var db = _connectionFactory.GetConnection)
                {
                    db.Execute(query, param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _connectionFactory.Dispose();
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public List<T> GetByStoredProcedure(string query, object param = null)
        {
            using (var db = _connectionFactory.GetConnection)
            {
                var result = db.Query<T>(query, param, null, true, null, CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<T> GetAll()
        {
            if (string.IsNullOrWhiteSpace(_select))
                GetSelect();
            using (var db = _connectionFactory.GetConnection)
            {
                var result = db.Query<T>(_select, null).ToList();
                return result;
            }


        }

        private string _insert { get; set; }

        private string _update { get; set; }

        private string _delete { get; set; }

        private string _select { get; set; }

        private void GetDelete()
        {
            StringBuilder sbQry = new StringBuilder();
            propInfo = type.GetProperties();

            sbQry.AppendFormat("Delete From {0} Where {1}=@{1}",
              type.Name.Replace("Entity", string.Empty), propInfo[0].Name, "{0}");

            _delete = sbQry.ToString();
        }

        private void GetUpdate()
        {

            StringBuilder sbQry = new StringBuilder();
            propInfo = type.GetProperties();
            foreach (System.Reflection.PropertyInfo pi in propInfo)
            {
                var is_entity_column = ((DapperAttribute[])pi.GetCustomAttributes(typeof(DapperAttribute), false)).Where(x => x.IsEntityColumn == false).FirstOrDefault();
                if (is_entity_column != null && !is_entity_column.IsEntityColumn || pi.Name == "ID")
                    continue;

                if (sbQry.ToString() == string.Empty)
                    sbQry.AppendFormat("Update {0} Set {1}=@{1}",
                             type.Name, pi.Name);
                else
                    sbQry.AppendFormat(", {0}=@{0}", pi.Name);
            }

            if (sbQry.ToString() != string.Empty)
                sbQry.AppendFormat(" Where {0}=@{0} ", propInfo[0].Name);

            sbQry.Replace("[", "{").Replace("]", "}");

            _update = sbQry.ToString();
        }

        private void GetInsert()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbQry = new StringBuilder();
            propInfo = type.GetProperties();

            for (int i = 1; i < propInfo.Count(); i++)
            {

                var is_entity_column = ((DapperAttribute[])propInfo[i].GetCustomAttributes(typeof(DapperAttribute), false)).Where(x => x.IsEntityColumn == false).FirstOrDefault();
                if (is_entity_column != null && !is_entity_column.IsEntityColumn || propInfo[i].Name == "ID")
                    continue;

                if (sbQry.ToString() == string.Empty)
                    sbQry.AppendFormat("INSERT INTO {0} ({1}",
                       type.Name.Replace("Entity", string.Empty), propInfo[i].Name);
                else
                {
                    sbQry.AppendFormat(", {0}", propInfo[i].Name);
                    sb.Append(",");
                }
                sb.Append("@" + propInfo[i].Name);
            }

            if (sbQry.ToString() != string.Empty)
                sbQry.AppendFormat(") VALUES({0}) select  SCOPE_IDENTITY()", sb.ToString());

            _insert = sbQry.ToString();
        }

        private void GetSelect()
        {
            StringBuilder sbQry = new StringBuilder();
            propInfo = type.GetProperties();
            foreach (System.Reflection.PropertyInfo pi in propInfo)
            {
                var is_entity_column = ((DapperAttribute[])pi.GetCustomAttributes(typeof(DapperAttribute), false)).Where(x => x.IsEntityColumn == false).FirstOrDefault();
                if (is_entity_column != null && !is_entity_column.IsEntityColumn)
                    continue;

                if (sbQry.ToString() == string.Empty)
                    sbQry.AppendFormat("Select {0}", pi.Name);
                else
                    sbQry.AppendFormat(", {0}", pi.Name);
            }

            if (sbQry.ToString() != string.Empty)
                sbQry.AppendFormat(" From {0} ", type.Name.Replace("Entity", string.Empty));

            _select = sbQry.ToString();
        }


    }
}
