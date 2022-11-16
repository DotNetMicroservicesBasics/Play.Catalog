using System.Collections.ObjectModel;
using MongoDB.Driver;
using Play.Catalog.Data.Entities;

namespace Play.Catalog.Data.Repositories
{

    public class ItemsRepository : IItemsRepository
    {
        private const string CollectionName = "Items";
        private readonly IMongoCollection<Item> _dbCollection;

        private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

        public ItemsRepository(IMongoDatabase database)
        {
            // var mongoClient = new MongoClient("mongodb://localhost:27017");
            // var database = mongoClient.GetDatabase("Catalog");
            _dbCollection = database.GetCollection<Item>(CollectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(i => i.Id, id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            var filter = _filterBuilder.Eq(i => i.Id, entity.Id);
            await _dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(i => i.Id, id);
            await _dbCollection.DeleteOneAsync(filter);
        }

    }
}