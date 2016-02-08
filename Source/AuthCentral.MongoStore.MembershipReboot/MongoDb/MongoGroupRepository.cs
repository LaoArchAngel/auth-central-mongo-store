using System;
using System.Collections.Generic;
using System.Linq;
using BrockAllen.MembershipReboot;
using MongoDB.Driver.Builders;

namespace Fsw.Enterprise.AuthCentral.MongoDb
{
    /// <summary>
    ///     Repository of <see cref="HierarchicalGroup" /> objects from the Mongo data store.
    /// </summary>
    /// <typeparam name="T">A class that is of type <see cref="HierarchicalGroup" /></typeparam>
    public class MongoGroupRepository<T> :
        QueryableGroupRepository<T>
        where T : HierarchicalGroup, new()
    {
        private readonly MongoDatabase _db;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MongoGroupRepository{T}" /> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public MongoGroupRepository(MongoDatabase db)
        {
            _db = db;
        }

        /// <summary>
        ///     Gets the direct queryable of all groups.
        /// </summary>
        /// <value>
        ///     The queryable.
        /// </value>
        protected override IQueryable<T> Queryable
        {
            get { return (IQueryable<T>) _db.Groups().FindAll().AsQueryable(); }
        }

        /// <summary>
        ///     Creates a new instance of a <see cref="HierarchicalGroup" />
        /// </summary>
        /// <returns>
        ///     <see cref="HierarchicalGroup" />
        /// </returns>
        public override T Create()
        {
            return new T();
        }

        /// <summary>
        ///     Adds the <see cref="HierarchicalGroup" /> to the repository.  Must have a unique or empty identifier.
        /// </summary>
        /// <param name="item">The new <see cref="HierarchicalGroup" />.</param>
        public override void Add(T item)
        {
            _db.Groups().Insert(item);
        }

        /// <summary>
        ///     Removes the given <see cref="HierarchicalGroup" /> from the repository.  Only the id is necessary.
        /// </summary>
        /// <param name="item">The <see cref="HierarchicalGroup" /> to remove.</param>
        public override void Remove(T item)
        {
            _db.Groups().Remove(Query<T>.EQ(e => e.ID, item.ID));
        }

        /// <summary>
        ///     Updates the specified <see cref="HierarchicalGroup" />.
        /// </summary>
        /// <param name="item">The <see cref="HierarchicalGroup" /> with changes.  ID cannot be changed.</param>
        public override void Update(T item)
        {
            _db.Groups().Save(item);
        }

        /// <summary>
        ///     Finds the parent <see cref="HierarchicalGroup" /> of the group with the given <paramref name="childGroupId" />
        /// </summary>
        /// <param name="childGroupId">The child group identifier.</param>
        /// <returns><see cref="HierarchicalGroup" /> that contains the child group.</returns>
        public override IEnumerable<T> GetByChildID(Guid childGroupId)
        {
            IQueryable<T> q =
                from g in Queryable
                from c in g.Children
                where c.ChildGroupID == childGroupId
                select g;
            return q;
        }
    }
}