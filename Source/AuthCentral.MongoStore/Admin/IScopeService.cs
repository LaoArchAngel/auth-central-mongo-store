/*
 * ---------------------------------------------------------------------------
 *
 * NOTICE: This file was modified from its original form by Food Service
 *         Warehouse and is intended to be derivative work. The 
 *         original copyright has been included, unmodified, below.
 *
 * ---------------------------------------------------------------------------
 *         
 * Copyright 2014, 2015 James Geall
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;

namespace Fsw.Enterprise.AuthCentral.MongoStore.Admin
{
    /// <summary>
    /// Service for interacting with AuthCentral's scopes.
    /// </summary>
    public interface IScopeService
    {
        /// <summary>
        /// Saves the specified scope.
        /// </summary>
        /// <param name="scope">The scope.</param>
        Task Save(Scope scope);

        /// <summary>
        /// Deletes the specified scope name.
        /// </summary>
        /// <param name="scopeName">Name of the scope.</param>
        Task Delete(string scopeName);

        /// <summary>
        /// Finds the specified scope name.
        /// </summary>
        /// <param name="scopeName">Name of the scope.</param>
        Task<Scope> Find(string scopeName);

        /// <summary>
        /// Finds the scopes with the specified names. Searches for exact matches.
        /// </summary>
        /// <param name="scopeNames">The scope names.</param>
        /// <returns>List of scopes with the given names.</returns>
        Task<IEnumerable<Scope>> Find(IEnumerable<string> scopeNames);

        /// <summary>
        /// Gets all of the scopes.
        /// </summary>
        /// <param name="publicOnly">if set to <c>true</c> will only return publicly visibly scopes.</param>
        /// <returns>List of all scopes.</returns>
        Task<IEnumerable<Scope>> Get(bool publicOnly = true);
    }
}