using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.App.Data.Common
{
    public interface IUnitOfWork :IDisposable
    {
        /// <summary>
        /// Function us to Get instance of a Object on Database
        /// </summary>
        /// <typeparam name="TEntity">Object is target</typeparam>
        /// <returns></returns>
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// Function use to Save all Object is changed into Database
        /// </summary>
        void SaveChanges();

        void TransactionSaveChanges();
    }
}
