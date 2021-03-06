﻿/*
 * ---------------------------------------------------------------------------
 *
 * NOTICE: This file was modified from it's original form by Food Service
 *         Warehouse and is hencforce considered a derivitive work. The 
 *         Original Copyright as been included, unmodified, below.
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
using Fsw.Enterprise.AuthCentral.MongoStore;
using MongoDB.Driver;

namespace Fsw.Enterprise.AuthCentral.MongoStore.Tests
{
    public class PersistenceTestFixture
    {
        private readonly StoreSettings _settings;
        private readonly IMongoDatabase _database;
        private readonly Factory _factory;

        public PersistenceTestFixture()
        {
            _settings = StoreSettings.DefaultSettings();
            _settings.Database = "testidentityserver";
            var registrations = new ServiceFactory(null, _settings);
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.Database);
            _factory = new Factory(registrations);
        }


        public IMongoDatabase Database
        {
            get { return _database; }
        }

        public StoreSettings Settings
        {
            get { return _settings; }
        }

        public Factory Factory
        {
            get { return _factory; }
        }
    }
}