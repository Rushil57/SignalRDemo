using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Interface;

namespace Repositories
{
    /// <summary>
    /// Represents the class for unit of work.
    /// </summary>   
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        private TContext Context { get; set; }
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        public UnitOfWork()
        {
            Context = new TContext();
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        public TContext GetContext()
        {
            return Context;
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                throw;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        public async Task SaveAsync()
        {
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                throw;
            }
        }

        /// <summary>
        /// Disposes the context.
        /// </summary>
        /// <param name="disposing">Flag to identify disposing.</param>
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Disposes the context.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
