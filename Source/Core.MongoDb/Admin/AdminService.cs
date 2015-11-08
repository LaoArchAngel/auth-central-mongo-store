/*
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

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using IdentityServer3.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Builder = MongoDB.Driver.Builders<MongoDB.Bson.BsonDocument>;

using AuthCentral.MongoStore;

namespace AuthCentral.MongoStore.Admin
{
    internal class AdminService : IDatabaseService, IClientService, IScopeService
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly StoreSettings _settings;
        private readonly ClientSerializer _clientSerializer;
        private readonly ClientStore _clientStore;
        private readonly ScopeStore _scopeStore;

        public AdminService(IMongoClient client, IMongoDatabase db, StoreSettings settings)
        {
            _client = client;
            _db = db;
            _settings = settings;
            _clientSerializer = new ClientSerializer();
            _clientStore = new ClientStore(db, settings, _clientSerializer);
            _scopeStore = new ScopeStore(db, settings);
        }

        public async Task CreateDatabase(bool expireUsingIndex = true)
        {
            var cursor = await _db.ListCollectionsAsync();
            var list = await cursor.ToListAsync();

            if (!list.CollectionExists(_settings.ClientCollection))
            {
                await _db.CreateCollectionAsync(_settings.ClientCollection);
            }
            if (!list.CollectionExists(_settings.ScopeCollection))
            {
                await _db.CreateCollectionAsync(_settings.ScopeCollection);
            }
            if (!list.CollectionExists(_settings.ConsentCollection))
            {
                var collection = _db.GetCollection<BsonDocument>(_settings.ConsentCollection);

                await collection.Indexes.CreateOneAsync(Builder.IndexKeys.Ascending("subject"));


                await collection.Indexes.CreateOneAsync(Builder.IndexKeys.Combine(
                    Builder.IndexKeys.Ascending("clientId"), Builder.IndexKeys.Ascending("subject")));

            }

            var tokenCollections = new[]
            {
                _settings.AuthorizationCodeCollection,
                _settings.RefreshTokenCollection,
                _settings.TokenHandleCollection
            };

            foreach (var tokenCollection in tokenCollections)
            {
                var collection = _db.GetCollection<BsonDocument>(tokenCollection);
                var options = new CreateIndexOptions() {ExpireAfter = TimeSpan.FromSeconds(1)};


                await collection.Indexes.CreateOneAsync(Builder.IndexKeys.Combine(
                    Builder.IndexKeys.Ascending("_clientId"), Builder.IndexKeys.Ascending("_subjectId")));
                
                try
                {
                    await collection.Indexes.CreateOneAsync(Builder.IndexKeys
                        .Ascending("_expires"),
                        options);
                }
                catch (MongoWriteConcernException)
                {
                    await collection.Indexes.DropOneAsync("_expires");
                    await collection.Indexes.CreateOneAsync(Builder.IndexKeys
                    .Ascending("_expires"),
                    options);
                }
            }
        }

        public async Task Save(Scope scope)
        {
            var doc = new ScopeSerializer().Serialize(scope);
            var collection = _db.GetCollection<BsonDocument>(_settings.ScopeCollection);
            await collection.ReplaceOneAsync(Filter.ById(scope.Name), doc, new UpdateOptions() {IsUpsert = true} );
        }

        public async Task Save(Client client)
        {
            var doc = _clientSerializer.Serialize(client);
            var collection = _db.GetCollection<BsonDocument>(_settings.ClientCollection);
            await collection.ReplaceOneAsync(
                Filter.ById(client.ClientId), 
                doc,
                new UpdateOptions() { IsUpsert = true}
                );
        }

        public async Task RemoveDatabase()
        {
            await _client.DropDatabaseAsync(_settings.Database);
        }

        async Task IClientService.Delete(string clientId)
        {
            var collection = _db.GetCollection<BsonDocument>(_settings.ClientCollection);
            await collection.DeleteOneByIdAsync(clientId);
        }

        async Task IScopeService.Delete(string scopeName)
        {
            var collection = _db.GetCollection<BsonDocument> (_settings.ScopeCollection);
            await collection.DeleteOneByIdAsync(scopeName );
        }

        async Task<Client> IClientService.Find(string clientId)
        {
            return await _clientStore.FindClientByIdAsync(clientId);
        }

        async Task<Scope> IScopeService.Find(string scopeName)
        {
            IList<string> scopes = new List<string>();
            scopes.Add(scopeName);

            IEnumerable<Scope> result = await _scopeStore.FindScopesAsync(scopes);
            IList<Scope> betterResult = new List<Scope>(result);
            
            if(result == null || betterResult.Count <= 0)
            {
                return null;
            }

            return betterResult[0];
        }

        async Task<IEnumerable<Scope>> IScopeService.Find(IEnumerable<string> scopeNames)
        {
            return await _scopeStore.FindScopesAsync(scopeNames);
        }

   }
}