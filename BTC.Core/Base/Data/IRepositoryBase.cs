using BTC.Core.Base.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTC.Core.Base.Data
{
        public interface IRepositoryBase<T> where T : class, IEntity
        {
            T GetByID(int Id);
            List<T> GetAll();
            int Insert(T entity);
            bool Delete(T entity);
            bool Update(T entity);
            bool ExecuteQuery(string query, object param = null);
            List<T> GetByCustomQuery(string query, object param);

            List<T> GetByStoredProcedure(string query, object param);

            bool InsertSqlBulkCopy(List<T> list);
        }
}
