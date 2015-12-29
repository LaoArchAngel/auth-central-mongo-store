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
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Fsw.Enterprise.AuthCentral.MongoStore.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Fsw.Enterprise.AuthCentral.MongoStore
{
    class ClientStore : MongoDbStore, IClientStore
    {
        private readonly ClientSerializer _serializer;
        private static readonly ILog Log = LogProvider.For<ClientStore>();
        public ClientStore(IMongoDatabase db, StoreSettings settings, ClientSerializer serializer) :
            base(db, settings.ClientCollection)
        {
            _serializer = serializer;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            Client result = null;
            BsonDocument loaded = await Collection.FindOneByIdAsync(clientId).ConfigureAwait(false);
            if (loaded != null)
            {
                result = _serializer.Deserialize(loaded);
            }
            else
            {
                Log.Debug("Client not found with id " + clientId);
            }

            return result;
        }

        public async Task<IList<Client>> GetPageAsync(int pageNumber, int rowsPerPage)
        {
            IList<Client> result = new List<Client>();

            var rowsToSkip = (pageNumber - 1) * rowsPerPage;
            var rowsToReturn = rowsPerPage + 1;
            List<BsonDocument> loaded = await Collection.Find(doc => true).Skip(rowsToSkip).Limit(rowsToReturn).ToListAsync();

            if (loaded != null)
            {
                foreach(BsonDocument doc in loaded)
                {
                    result.Add(_serializer.Deserialize(doc));
                }
            }

            return result;
        }

        public async Task<IList<Client>> GetRangeAsync(int offset, int limit)
        {
            IList<Client> result = new List<Client>();
            List<BsonDocument> loaded = await Collection.Find(doc => true).Skip(offset).Limit(limit+1).ToListAsync();

            if (loaded != null)
            {
                foreach(BsonDocument doc in loaded)
                {
                    result.Add(_serializer.Deserialize(doc));
                }
            }

            return result;
        }

    }
}