using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Repositories.Interface;

namespace Repositories
{
    /// <summary>
    /// Represents the class for repository base.
    /// </summary>    
    public class RepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        private DbContext Context { get; set; }

        internal DbSet<TEntity> DbSet;

        /// <summary>
        /// Initializes a new instance of the RepositoryBase class.
        /// </summary>
        /// <param name="unitOfWork">Object of IUnitOfWork.</param>
        public RepositoryBase(IUnitOfWork<TContext> unitOfWork)
        {
            Context = unitOfWork.GetContext();
            DbSet = Context.Set<TEntity>();
        }

        #region Select/Get/Query
        /// <summary>
        /// Gets an IQueryable that is used to retrieve entities from table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        public virtual IQueryable<TEntity> All()
        {
            return DbSet.AsQueryable();
        }

        /// <summary>
        /// Gets all entities based on given parameters.
        /// </summary>
        /// <param name="predicate">A condition to filter entities.</param>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable();
        }

        /// <summary>
        /// Gets all entities based on given filters.
        /// </summary>
        /// <param name="filter">A condition to filter entities.</param>
        /// <param name="total">Returns count of entities.</param>
        /// <param name="index">The page index.</param>
        /// <param name="size">The page size.</param>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            resetSet = skipCount == 0 ? resetSet.Take(size) : resetSet.Skip(skipCount).Take(size);
            total = resetSet.Count();
            return resetSet.AsQueryable();
        }

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities.</param>
        /// <returns>Entity or null.</returns>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return All().FirstOrDefault(predicate);
        }

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities.</param>
        /// <returns>Entity or null.</returns>
        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await All().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Entity.</returns>
        public virtual TEntity GetByID(object id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Entity.</returns>
        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }
        #endregion

        #region Insert
        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity.</param>
        /// <returns>Void.</returns>
        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        /// <summary>
        /// Inserts a new entity.
        /// Reference: http://stackoverflow.com/questions/12144077/async-await-when-to-return-a-task-vs-void
        /// </summary>
        /// <param name="entity">Inserted entity.</param>
        /// <returns>Task.</returns>
        public virtual async Task InsertAsync(TEntity entity)
        {
            await Task.FromResult(DbSet.Add(entity));
        }

        /// <summary>
        /// Inserts list of entities.
        /// </summary>
        /// <param name="entities">List of entities to be inserted.</param>
        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        /// <summary>
        /// Inserts list of entities.
        /// </summary>
        /// <param name="entities">List of entities to be inserted.</param>
        /// <returns>Task.</returns>
        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.FromResult(DbSet.AddRange(entities));
        }

        #endregion

        #region Delete
        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Void.</returns>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities.</param>
        /// <returns>Void.</returns>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var deletEntities = DbSet.Where(predicate).ToList();
            foreach (var entity in deletEntities)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entityToDelete">Entity to be deleted.</param>
        /// <returns>Void.</returns>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }

            DbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Task.</returns>
        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await DbSet.FindAsync(id);
            await DeleteAsync(entityToDelete);
        }

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities.</param>
        /// <returns>Task.</returns>
        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var deleteEntities = await DbSet.Where(predicate).ToListAsync();
            foreach (var entity in deleteEntities)
            {
                await DeleteAsync(entity);
            }
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entityToDelete">Entity to be deleted.</param>
        /// <returns>Task.</returns>
        public virtual async Task DeleteAsync(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                await Task.FromResult(DbSet.Attach(entityToDelete));
            }

            await Task.FromResult(DbSet.Remove(entityToDelete));
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="entityToUpdate">Entity.</param>
        /// <param name="performAudit">Flag to indicate whether to audit or not.</param>
        /// <returns>Void.</returns>
        public virtual void Update(TEntity entityToUpdate, bool performAudit = false)
        {
            bool exceptionCaptured = false;

            //// Perform audit for the entity.
            if (performAudit)
            {
                try
                {
                    SetOriginalValue(entityToUpdate);
                }
                catch (Exception)
                {
                    exceptionCaptured = true;
                }
            }

            if (exceptionCaptured || performAudit == false)
            {
                DbSet.Attach(entityToUpdate);
            }

            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Updates an existing entity. 
        /// Reference: http://stackoverflow.com/questions/12144077/async-await-when-to-return-a-task-vs-void
        /// </summary>
        /// <param name="entityToUpdate">Entity.</param>
        /// <param name="performAudit">Flag to indicate whether to audit or not.</param>
        /// <returns>Task.</returns>
        public virtual async Task UpdateAsync(TEntity entityToUpdate, bool performAudit = false)
        {
            bool exceptionCaptured = false;

            if (performAudit)
            {
                try
                {
                    await SetOriginalValueAsync(entityToUpdate);
                }
                catch (Exception)
                {
                    exceptionCaptured = true;
                }
            }

            if (exceptionCaptured || performAudit == false)
            {
                await Task.FromResult(DbSet.Attach(entityToUpdate));
            }

            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }


        /// <summary>
        /// Sets original values to entity.
        /// </summary>
        /// <param name="entityToUpdate">Entity to update.</param>
        public virtual async Task SetOriginalValueAsync(TEntity entityToUpdate)
        {
            var entityType = entityToUpdate.GetType();

            //// Get ID column name from entity type.
            var idColumnName = GetIdColumnName(entityType);
            var entity = await GetByIDAsync(entityType.GetProperty(idColumnName).GetValue(entityToUpdate, null));
            foreach (var property in entityType.GetProperties())
            {
                if (property.GetSetMethod() != null)
                {
                    if (property.Name.Equals("RowVersion"))
                    {
                        Context.Entry(entityToUpdate).OriginalValues[property.Name] = entityType.GetProperty(property.Name).GetValue(entityToUpdate);
                    }
                    else
                    {
                        property.SetValue(entity, entityType.GetProperty(property.Name).GetValue(entityToUpdate, null), null);
                    }
                }
            }
        }

        /// <summary>
        /// Sets original values to entity.
        /// </summary>
        /// <param name="entityToUpdate">Entity to update.</param>
        public virtual void SetOriginalValue(TEntity entityToUpdate)
        {
            var entityType = entityToUpdate.GetType();

            //// Get ID column name from entity type.
            var idColumnName = GetIdColumnName(entityType);
            var entity = GetByID(entityType.GetProperty(idColumnName).GetValue(entityToUpdate, null));
            foreach (var property in entityType.GetProperties())
            {
                if (property.GetSetMethod() != null)
                {
                    if (property.Name.Equals("RowVersion"))
                    {
                        Context.Entry(entityToUpdate).OriginalValues[property.Name] = entityType.GetProperty(property.Name).GetValue(entityToUpdate);
                    }
                    else
                    {
                        property.SetValue(entity, entityType.GetProperty(property.Name).GetValue(entityToUpdate, null), null);
                    }
                }
            }
        }

        /// <summary>
        /// Gets ID column name from type.
        /// </summary>
        /// <param name="type">Type object.</param>
        /// <returns>Returns ID column name.</returns>
        public virtual string GetIdColumnName(Type type)
        {
            string name = string.Empty;
            if (type.BaseType != null)
            {
                string[] splittedBaseName = type.BaseType.Name.Split('_');

                /*As per database table naming convention there will be atleast 3 part separated 
                  by underscore like OL_CM_Campaign will have OL, CM and Campaign. 
                  Whereas the table like OL_CM_Group_CampaignPanelGroup will have OL, CM, Group and CampaignPanelGroup*/
                if (splittedBaseName.Length >= 3)
                {
                    name = string.Format("{0}Id", string.Join("", splittedBaseName.Skip(2)));
                }
            }

            return name;
        }
        #endregion
    }
}
