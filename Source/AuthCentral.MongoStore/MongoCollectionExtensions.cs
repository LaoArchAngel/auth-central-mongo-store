﻿/*
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
using MongoDB.Bson;
using MongoDB.Driver;

namespace Fsw.Enterprise.AuthCentral.MongoStore
{
    static class MongoCollectionExtensions
    {
        public static Task<BsonDocument> FindOneByIdAsync(this IMongoCollection<BsonDocument> collection, object id)
        {
            return collection.Find(Filter.ById(id)).FirstOrDefaultAsync();
        }

        public static Task<DeleteResult> DeleteOneByIdAsync(this IMongoCollection<BsonDocument> collection, object id)
        {
            return collection.DeleteOneAsync(Filter.ById(id));
        }
    }
}
