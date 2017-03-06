using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    /// <summary>
    /// Defines the methods that are used in an unit of work.
    /// </summary>
    public interface IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        TContext GetContext();

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        void Save();

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// Disposes the context.
        /// </summary>
        /// <param name="disposing">Flag to identify disposing.</param>
        void Dispose(bool disposing);
    }
}
