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
