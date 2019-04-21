// <copyright file="RepositoryBase.cs" company="Szt2Company">
// Copyright (c) Szt2Company. All rights reserved.
// </copyright>

namespace DatabaseOperations
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using DatabaseOperations.Interfaces;
    using static DatabaseOperations.Enums;

    /// <summary>
    /// Serving as a base class for repositories
    /// </summary>
    /// <typeparam name="TR">The type of DBContext descendant to use</typeparam>
    public abstract class RepositoryBase<TR> : IRepository, IDisposable
        where TR : DbContext, new()
    {
        private TR db;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TR}"/> class.
        /// </summary>
        protected RepositoryBase()
        {
            this.db = new TR();
        }

        /// <summary>
        /// Saves an element to the database
        /// </summary>
        public virtual void SaveToDB()
        {
            this.db.SaveChanges();
        }

        /// <summary>
        /// Adds an element to the set identified by T, THROWS: an AppError if an entry with the same key already exists,
        /// </summary>
        /// <typeparam name="T">The type of the DB model</typeparam>
        /// <param name="element">The element to be credated</param>
        public virtual void Create<T>(T element)
            where T : class, IDBEntry, IKeyProvider
        {
            if (this.EntryFinder<T>(element.Id) != null)
            {
                throw new ApplicationException($"The {typeof(T).Name} element with the same ID is already in the database");
            }

            try
            {
                // uniqueness is implemented on a db lvl, so it will throw an exception if a non-uniqu element would be inserted
                element.Dbstate = (int)DBState.Active;
                this.db.Set<T>().Add(element);
                this.db.Entry(element).State = EntityState.Added;
                this.db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Tries to delete the entry with the specified type T, Exceptions: AppException if the specified entry does not exist, InvalidOp
        /// </summary>
        /// <typeparam name="T">The type fo the etry to be deleted</typeparam>
        /// <param name="element">The element to be deleted</param>
        public virtual void Delete<T>(T element)
             where T : class, IDBEntry, IKeyProvider
        {
            // T deletee = db.Set<T>().SingleOrDefault(e => e.ID == element.ID);
            T deletee = this.EntryFinder<T>(element.Id);
            if (deletee != null)
            {
                // deletion is setting the the DBSTATE of the deletee to deleted
                // db.Set<T>().Remove(deletee);
                // db.SaveChanges();
                // deletee.DBSTATE = (int)DBState.Deleted;
                // db.Set<T>().Add(deletee);
                // db.SaveChanges();
                deletee.Dbstate = (int)DBState.Deleted;
                this.Update(deletee);
            }
            else
            {
                throw new ApplicationException($"There was an error during deleting {typeof(T).Name} instance");
            }
        }

        /// <inheritdoc/>
        public virtual IQueryable<T> GetElements<T>()
            where T : class, IDBEntry
        {
            return this.db.Set<T>().Where(e => e.Dbstate == (int)DBState.Active);
        }

        /// <inheritdoc/>
        public virtual void Update<T>(T element)
            where T : class, IKeyProvider
        {
            // T updatee = db.Set<T>().SingleOrDefault(e => e.ID == element.ID);
            T updatee = this.EntryFinder<T>(element.Id);
            if (updatee != null)
            {
                // we replace the existing element properties with the one we were given
                // var props = updatee.GetType().GetProperties();
                // foreach (var p in props)
                // {
                //    //we separate the collection type, bc whatever comes with that has to be ADDED to the existing, not replacing it
                //    if (!(p.PropertyType.Name == "ICollection`1"))
                //    {
                //        updatee.GetType().GetProperty(p.Name).SetValue(updatee, element.GetType().GetProperty(p.Name).GetValue(element));
                //    }
                //
                // }
                this.db.Entry(updatee).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            else
            {
                throw new ApplicationException($"An error occured during updating the {typeof(T).Name} instance");
            }
        }

        /// <summary>
        /// Finds a T entry in the database, returns null if no entry was found
        /// </summary>
        /// <typeparam name="T">The modelclass entry type</typeparam>
        /// <param name="id">The id of the entry</param>
        /// <returns>T entry if it exists in the database, null otherwise</returns>
        public virtual T EntryFinder<T>(int id)
        {
            return (T)this.db.Set(typeof(T)).Find(id);
        }

        /// <summary>
        /// Disposes of meneged resources
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the managed resouces of the database
        /// </summary>
        /// <param name="disposing">Flag to check if the method is called manually (True) or from a finalier (False)</param>
        protected virtual void Dispose(bool disposing)
        {
            // see the dispose pattern: https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
            // if resouces have been disposed, return
            if (this.disposed)
            {
                return;
            }

            // if resouces are currently under disposing, do the dispose
            if (disposing)
            {
                this.db.Dispose();
            }

            // set the flag to true
            this.disposed = true;
        }
    }
}
