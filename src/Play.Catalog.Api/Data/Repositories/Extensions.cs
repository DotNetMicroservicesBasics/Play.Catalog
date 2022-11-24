using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Data.Entities;
using Play.Catalog.Settings;

namespace Play.Catalog.Data.Repositories
{
    public static class Extenstions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            ///Serializer for proper MongoDB serialization of specified type
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            // Add mongoDb to the ServiceCollection

            services.AddSingleton(serviceProvider =>
             {
                 var configuration = serviceProvider.GetService<IConfiguration>();
                 var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                 var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                 return mongoClient.GetDatabase(mongoDbSettings.DbName);
             });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider =>
            {
                var database = serviceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });

            return services;
        }
    }
}