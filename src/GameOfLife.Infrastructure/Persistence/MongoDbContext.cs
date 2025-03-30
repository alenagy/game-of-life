using GameOfLife.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GameOfLife.Infrastructure.Persistence
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<Board> Boards => _database.GetCollection<Board>("Boards");
    }
}