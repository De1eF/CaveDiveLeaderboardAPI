using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestDeleF.Models;

namespace TestDeleF.Services
{
    public class PlayerService
    {
        private readonly IMongoCollection<PlayerDto> _playerCollection;

        public PlayerService(IOptions<PlayerDatabaseSettings> playerDatabaseSettings)
        {
            var mongoClient = new MongoClient(
            playerDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                playerDatabaseSettings.Value.DatabaseName);

            _playerCollection = mongoDatabase.GetCollection<PlayerDto>(
                playerDatabaseSettings.Value.PlayerCollectionName);
        }

        public async Task<List<PlayerDto>> GetAsync() =>
        await _playerCollection.Find(_ => true).ToListAsync();

        public async Task<PlayerDto?> GetAsync(string id) =>
            await _playerCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<PlayerDto>> GetOutdatedAsync() =>
            await _playerCollection.Find(x => x.ExpieryTime > DateTimeOffset.Now.ToUnixTimeMilliseconds())
                .ToListAsync();

        public async Task CreateAsync(PlayerDto newBook) =>
            await _playerCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, PlayerDto updatedBook) =>
            await _playerCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _playerCollection.DeleteOneAsync(x => x.Id == id);

        public async Task RemoveOutdatedAsync() =>
            await _playerCollection.DeleteManyAsync(x => x.ExpieryTime < DateTimeOffset.Now.ToUnixTimeMilliseconds());
    }
}
