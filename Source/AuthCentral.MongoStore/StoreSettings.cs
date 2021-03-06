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
namespace Fsw.Enterprise.AuthCentral.MongoStore
{
    public class StoreSettings
    {
        public string Database { get; set; }
        public string ClientCollection { get; set; }
        public string ScopeCollection { get; set; }
        public string ConsentCollection { get; set; }
        public string AuthorizationCodeCollection { get; set; }
        public string ConnectionString { get; set; }
        public string RefreshTokenCollection { get; set; }
        public string TokenHandleCollection { get; set; }

        public static StoreSettings DefaultSettings()
        {
            return new StoreSettings
            {
                //ConnectionString = "mongodb://localhost",
                ConnectionString = "mongodb://FSWDEVMongo:30000,FSWDEVMongo:40000,FSWDEVMongo:50000/authcentral-ids?connectTimeoutMS=20000",
                Database = "identityserver_testonly",
                ClientCollection = "clients",
                ScopeCollection = "scopes",
                ConsentCollection = "consents",
                AuthorizationCodeCollection = "authorizationCodes",
                RefreshTokenCollection = "refreshtokens",
                TokenHandleCollection = "tokenhandles"
            };
        }
    }
}