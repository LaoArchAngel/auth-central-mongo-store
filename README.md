# MongoDb Persistence for IdentityServer3 #
A divergent fork of https://github.com/jageall/IdentityServer.v3.MongoDb.

## Build Status ##

## Usage ##

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Register your IUserService implementation
            var userService = new Registration<IUserService>(/*...*/);

            // Create and modify default settings
            var settings = IdentityServerMongoDb.StoreSettings.DefaultSettings();
            settings.ConnectionString = "mongodb://localhost";

            // Create the MongoDB factory
            var factory = new IdentityServerMongoDb.ServiceFactory(userService, settings);

            // Overwrite services, e.g. with in memory stores
            factory.Register(new Registration<IEnumerable<Client>>(MyClients.Get()));
            factory.ClientStore = new Registration<IClientStore>(typeof(InMemoryClientStore));

            var options = new IdentityServerOptions()
            {
                Factory = factory,
            };

            app.Map("/idsrv", idServer =>
            {
                idServer.UseIdentityServer(options);
            });
        }

## Credits ##
MongoDb Persistence for Thinktecture IdentityServer is built using the following great open source projects:
- [IdentityServer.v3.MongoDb](https://github.com/jageall/IdentityServer.v3.MongoDb)
- [Thinktecture Identity Server v3](https://github.com/identityserver/identityserver3)
- [MongoDb](http://www.mongodb.org/)
- [MongoDb C# Driver](https://github.com/mongodb/mongo-csharp-driver)
- [Katana](https://katanaproject.codeplex.com/)
- [xUnit](https://github.com/xunit)
- [Autofac](http://autofac.org/)
- [LibLog](https://github.com/damianh/liblog)
