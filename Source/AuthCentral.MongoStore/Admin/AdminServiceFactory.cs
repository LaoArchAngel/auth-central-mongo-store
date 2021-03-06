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
using MongoDB.Driver;

namespace Fsw.Enterprise.AuthCentral.MongoStore.Admin
{
    public static class AdminServiceFactory
    {
        public static IClientService CreateClientService(StoreSettings settings)
        {
            return Create(settings) as IClientService;
        }

        public static IDatabaseService CreateDatabaseService(StoreSettings settings)
        {
            return Create(settings) as IDatabaseService;
        }

        public static IScopeService CreateScopeService(StoreSettings settings)
        {
            return Create(settings) as IScopeService;
        }
   
        private static AdminService Create(StoreSettings settings)
        {
            var mongoClient = new MongoClient(settings.ConnectionString);
            var db = mongoClient.GetDatabase(settings.Database);
            return new AdminService(mongoClient, db, settings);
        }
    }
}
