using System.Collections.Generic;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    /// <summary>
    ///     Interface for a user repository that pulls many / unfiltered users.
    /// </summary>
    /// <typeparam name="TAccount">The type of the account.</typeparam>
    public interface IBulkUserRepository<out TAccount>
    {
        /// <summary>
        ///     Gets a page of users.
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="count">The number of users in the page.  Should never be greater than <paramref name="pageSize" /></param>
        /// <returns></returns>
        IEnumerable<TAccount> GetPagedUsers(int page, int pageSize, out long count);
    }
}