﻿/*
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

using AuthCentral.MongoStore.Admin;
using IdentityServer3.Core.Configuration;

namespace IdentityServer.Admin.MongoDb
{
    public class AdminServiceRegistry
    {
        public AdminServiceRegistry()
        {
            ClientService = new Registration<IClientService>(typeof(AdminService));
            DatabaseService = new Registration<IDatabaseService>(typeof(AdminService));
            ScopeService = new Registration<IScopeService>(typeof(AdminService));
            TokenCleanupService = new Registration<ICleanupExpiredTokens>(typeof(CleanupExpiredTokens));
   
        }

        public Registration<IClientService> ClientService { get; set; }
        public Registration<IDatabaseService> DatabaseService { get; set; }
        public Registration<IScopeService> ScopeService { get; set; }

        public Registration<ICleanupExpiredTokens> TokenCleanupService { get; set; }
    }
}
