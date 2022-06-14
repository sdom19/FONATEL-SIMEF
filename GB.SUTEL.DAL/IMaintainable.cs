using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GB.SUTEL.DAL
{
    /// <summary>
    /// Base for any regular maintainable
    /// </summary>
    /// <createdby>
    /// <name>Walter Montes, Marvin Zumbado</name>
    /// <contact>walter.montes@grupobabel.com; walmon93@hotmail.com, marvinzzz@gmail.com, marvin.zumbado@grupobabel.com</contact>
    /// </createdby>
    /// <created>January 13, 2014</created>
    public interface IMaintainable<T> : IDisposable
    {
        /// <summary>
        /// Get a single item by Id
        /// </summary>
        /// <param name="expression">query expression</param>
        /// <returns>Entity</returns>
        T Single(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Queryable list of all the entities
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Queryable list of all entities</returns>
        System.Linq.IQueryable All();
        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="item">Entity to save</param>
        void Add(T item);
        /// <summary>
        /// Update a entity
        /// </summary>
        /// <param name="item">Entity to update</param>
        void Update(T item);
        /// <summary>
        /// Delete a item, pass the entity with at least the id
        /// </summary>
        /// <param name="item">Entity to delete</param>
        void Delete(T item);
        /// <summary>
        /// Delete a bunch of items by a expression
        /// </summary>
        /// <param name="expression">query expression</param>
        void Delete(Expression<Func<T, bool>> expression);
        /// <summary>
        /// Delete all items
        /// </summary>
        void DeleteAll();
        /// <summary>
        /// Execute the modifications to the data source
        /// </summary>
        void CommitChanges();
    }
}
