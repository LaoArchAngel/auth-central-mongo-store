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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Xunit;

namespace AuthCentral.MongoStore.Tests
{
    public class ConsentSerializerTests : PersistenceTest, IClassFixture<PersistenceTestFixture>
    {
        private readonly Consent _expected;
        private Consent _actual;
        private readonly Task _setup;


        [Fact]
        public async Task CheckAll()
        {
            await _setup;
            var serializer = new JsonSerializer() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var expected = JObject.FromObject(_expected, serializer).ToString();
            var actual = JObject.FromObject(_actual, serializer).ToString();
            Assert.Equal(expected, actual);
        }

        public ConsentSerializerTests(PersistenceTestFixture data)
            : base(data)
        {
            
            _expected = new Consent
            {
                ClientId = "client",
                Subject = "subject",
                Scopes = new[] { "scope1", "scope2" }
            };
            _setup = Setup();
        }

        private async Task Setup()
        {
            var store = Factory.Resolve<IConsentStore>();
            await store.UpdateAsync(_expected);
            _actual = await store.LoadAsync(_expected.Subject, _expected.ClientId);
        }
    }
}