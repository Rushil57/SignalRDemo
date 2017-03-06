using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    /// <summary>
    /// Defines the methods that are used in a repository base.
    /// </summary>
    public interface IRepositoryBase<TEntity, TContext>
    {
        #region Select/Get/Query
        /// <summary>
        /// Gets an IQueryable that is used to retrieve entities from table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        IQueryable<TEntity> All();

        /// <summary>
        /// Gets all entities based on given parameters.
        /// </summary>
        /// <param name="predicate">A condition to filter entities.</param>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets all entities based on given filters.
        /// </summary>
        /// <param name="filter">A condition to filter entities.</param>
        /// <param name="total">Returns count of entities.</param>
        /// <param name="index">The page index.</param>
        /// <param name="size">The page size.</param>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities.</param>
        /// <returns>Entity or null.</returns>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities.</param>
        /// <returns>Entity or null.</returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Entity.</returns>
        TEntity GetByID(object id);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Entity.</returns>
        Task<TEntity> GetByIDAsync(object id);

        #endregion

        #region Insert
        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity.</param>
        /// <returns>Void.</returns>
        void Insert(TEntity entity);

        /// <summary>
        /// Inserts a new entity.
        /// Reference: http://stackoverflow.com/questions/12144077/async-await-when-to-return-a-task-vs-void
        /// </summary>
        /// <param name="entity">Inserted entity.</param>
        /// <returns>Task.</returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts list of entities.
        /// </summary>
        /// <param name="entities">List of entities to be inserted.</param>
        void InsertRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Inserts list of entities.
        /// </summary>
        /// <param name="entities">List of entities to be inserted.</param>
        /// <returns>Task.</returns>
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        #endregion

        #region Delete
        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Void.</returns>
        void Delete(object id);

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities.</param>
        /// <returns>Void.</returns>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entityToDelete">Entity to be deleted.</param>
        /// <returns>Void.</returns>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(object id);

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entityToDelete">Entity to be deleted.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(TEntity entityToDelete);
        #endregion

        #region Update

        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="entityToUpdate">Entity.</param>
        /// <param name="performAudit">Flag to indicate whether to audit or not.</param>
        /// <returns>Void.</returns>
        void Update(TEntity entityToUpdate, bool performAudit = false);

        /// <summary>
        /// Updates an existing entity. 
        /// Reference: http://stackoverflow.com/questions/12144077/async-await-when-to-return-a-task-vs-void
        /// </summary>
        /// <param name="entityToUpdate">Entity.</param>
        /// <param name="performAudit">Flag to indicate whether to audit or not.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(TEntity entityToUpdate, bool performAudit = false);

        /// <summary>
        /// Sets original values to entity.
        /// </summary>
        /// <param name="entityToUpdate">Entity to update.</param>
        Task SetOriginalValueAsync(TEntity entityToUpdate);

        /// <summary>
        /// Sets original values to entity.
        /// </summary>
        /// <param name="entityToUpdate">Entity to update.</param>
        void SetOriginalValue(TEntity entityToUpdate);

        /// <summary>
        /// Gets Id column name from type.
        /// </summary>
        /// <param name="type">Type object.</param>
        /// <returns>Returns Id column name.</returns>
        string GetIdColumnName(Type type);

        #endregion


    }
}
