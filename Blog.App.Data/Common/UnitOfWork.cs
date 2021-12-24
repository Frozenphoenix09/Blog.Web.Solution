using Blog.App.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Transactions;

namespace Blog.App.Data.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogDataBaseContext _context;
        private readonly Dictionary<Type, object> _repository;
        private bool _disposed;

        public UnitOfWork(BlogDataBaseContext context)
        {
            _context = context;
            _repository = new Dictionary<Type, object>();
            _disposed = false;
        }

        /// <summary>
        /// Function us to Get instance of a Object on Database
        /// </summary>
        /// <typeparam name="TEntity">Object is target</typeparam>
        /// <returns></returns>
        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            //Check if the Dictionary key contains the Model class
            if (_repository.Keys.Contains(typeof(TEntity)))
            {
                return _repository[typeof(TEntity)] as IBaseRepository<TEntity>;
            }
            // If the repository for that Model class doesn't exist, create it
            var repository = new BaseRepository<TEntity>(_context);

            _repository.Add(typeof(TEntity), repository);
            return repository;
        }

        public void TransactionSaveChanges()
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                try
                {
                    _context.SaveChanges();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (ValidationException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}