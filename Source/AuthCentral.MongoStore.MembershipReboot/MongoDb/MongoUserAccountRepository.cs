﻿using System.Collections.Generic;
using System.Linq;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Fsw.Enterprise.AuthCentral.IdMgr;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Fsw.Enterprise.AuthCentral.MongoDb
{
    /// <summary>
    /// Repository for MembershipReboot UserAccount objects from a Mongo data source.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoUserAccountRepository<T> : QueryableUserAccountRepository<T>, IBulkUserRepository<T>
        where T : HierarchicalUserAccount, new()
    {
        private readonly MongoDatabase _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoUserAccountRepository{T}"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public MongoUserAccountRepository(MongoDatabase db)
        {
            UseEqualsOrdinalIgnoreCaseForQueries = true;
            _db = db;
        }

        /// <summary>
        /// Gets a direct queryable of the Users table.
        /// </summary>
        /// <value>
        /// The queryable.
        /// </value>
        protected override IQueryable<T> Queryable
        {
            get { return (IQueryable<T>) _db.Users().FindAll().AsQueryable(); }
        }

        /// <summary>
        /// Gets a paged list of users
        /// </summary>
        /// <param name="page">The page of users to get.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="count">The number of users in the page.</param>
        /// <returns></returns>
        public IEnumerable<T> GetPagedUsers(int page, int pageSize, out long count)
        {
            // Optional parameters must appear after all other parameters
            if (page < 1)
            {
                page = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 25;
            }

            MongoCursor<T> accountsQuery = _db.Users().FindAllAs<T>();

            // MongoCursor.Limit(x) restricts how many records can be retrieved
            // But MongoCursor.Skip enumerates the list, removes the first N and returns the rest as an IEnumerable<T>
            // This enumeration counts towards Limit. Once Skip is called, Limit is no longer useful.
            // Therefore to our Limit we have to add the number of records that will be skipped.
            accountsQuery.Limit = page*pageSize;
            // Count() locks the cursor, so Limit can't be called anymore
            count = accountsQuery.Count();
            IEnumerable<T> accounts = accountsQuery.Skip((page - 1)*pageSize);

            return accounts;
        }

        /// <summary>
        /// Creates a blank <see cref="UserAccount"/>
        /// </summary>
        /// <returns>New <see cref="UserAccount"/></returns>
        public override T Create()
        {
            return new T();
        }

        /// <summary>
        /// Adds the specified <see cref="UserAccount"/> to the repository.
        /// </summary>
        /// <param name="item">The <see cref="UserAccount"/> to add to the repository.</param>
        public override void Add(T item)
        {
            _db.Users().Insert(item);
        }

        /// <summary>
        /// Updates the specified <see cref="UserAccount"/> that already exists in the repository.  Matches by the unique ID. 
        /// </summary>
        /// <param name="item">The <see cref="UserAccount"/> with changes.</param>
        public override void Update(T item)
        {
            _db.Users().Save(item);
        }

        /// <summary>
        /// Removes the specified <see cref="UserAccount"/>.  Only the identitfier need be filled.
        /// </summary>
        /// <param name="item">The <see cref="UserAccount"/> to remove.</param>
        public override void Remove(T item)
        {
            _db.Users().Remove(Query<T>.EQ(e => e.ID, item.ID));
        }

        /// <summary>
        /// Finds an account by a linked account.
        /// </summary>
        /// <param name="tenant">The tenant the account we're searching for is in.</param>
        /// <param name="provider">The name of the provider of the linked account.</param>
        /// <param name="id">The provider's unique identifier for the linked account.</param>
        /// <returns>A <see cref="UserAccount"/></returns>
        public override T GetByLinkedAccount(string tenant, string provider, string id)
        {
            IQueryable<T> query =
                from a in Queryable
                where a.Tenant == tenant
                from la in a.LinkedAccountCollection
                where la.ProviderName == provider && la.ProviderAccountID == id
                select a;
            return query.SingleOrDefault();
        }

        /// <summary>
        /// Finds an account by their SSL certificate thumbprint.
        /// </summary>
        /// <param name="tenant">The tenant.</param>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns></returns>
        public override T GetByCertificate(string tenant, string thumbprint)
        {
            IQueryable<T> query =
                from a in Queryable
                where a.Tenant == tenant
                from c in a.UserCertificateCollection
                where c.Thumbprint == thumbprint
                select a;
            return query.SingleOrDefault();
        }

        //void IUserAccountRepository<UserAccount>.Remove(UserAccount item)
        //}
        //    This.Add((T)item);
        //{

        //void IUserAccountRepository<UserAccount>.Add(UserAccount item)
        //}
        //    return This.Create();
        //{

        //UserAccount IUserAccountRepository<UserAccount>.Create()

        //IUserAccountRepository<T> This { get { return (IUserAccountRepository<T>)this; } }
        //{
        //    This.Remove((T)item);
        //}

        //void IUserAccountRepository<UserAccount>.Update(UserAccount item)
        //{
        //    This.Update((T)item);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByID(Guid id)
        //{
        //    return This.GetByID(id);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByUsername(string username)
        //{
        //    return This.GetByUsername(username);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByUsername(string tenant, string username)
        //{
        //    return This.GetByUsername(tenant, username);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByEmail(string tenant, string email)
        //{
        //    return This.GetByEmail(tenant, email);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByMobilePhone(string tenant, string phone)
        //{
        //    return This.GetByMobilePhone(tenant, phone);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByVerificationKey(string key)
        //{
        //    return This.GetByVerificationKey(key);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByLinkedAccount(string tenant, string provider, string id)
        //{
        //    return This.GetByLinkedAccount(tenant, provider, id);
        //}

        //UserAccount IUserAccountRepository<UserAccount>.GetByCertificate(string tenant, string thumbprint)
        //{
        //    return This.GetByCertificate(tenant, thumbprint);
        //}
    }
}