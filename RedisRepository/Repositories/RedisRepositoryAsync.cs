using RedisRepository.Repositories.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisRepository.Repositories
{
    public class RedisRepositoryAsync<T> : IRedisRepositoryAsync<T> where T : class
    {
        private readonly IDatabase _redisCache;

        public RedisRepositoryAsync(IConnectionMultiplexer connection)
        {
            _redisCache = connection.GetDatabase() ?? throw new ArgumentNullException(nameof(connection));
        }

        #region Métodos com hash

        /// <summary>
        /// Adiciona um objeto serializado como JSON
        /// </summary>
        /// <typeparam name="TO">Any</typeparam>
        /// <param name="hash">Dictionary hash</param>
        /// <param name="key">Dictionary key</param>
        /// <param name="obj">Dictionary obj</param>
        public async Task SetWithJsonAsync<TO>(string hash, string key, TO obj)
        {
            HashEntry[] value = { new HashEntry(key, JsonSerializer.Serialize(obj)) };

            await _redisCache.HashSetAsync(hash, value);
        }

        /// <summary>
        /// Adiciona um objeto serializado como Array de bytes
        /// </summary>
        /// <typeparam name="TO">Any</typeparam>
        /// <param name="hash">Dictionary hash</param>
        /// <param name="key">Dictionary key</param>
        /// <param name="obj">Dictionary obj</param>
        public async Task SetWithBytesAsync<TO>(string hash, string key, TO obj)
        {
            HashEntry[] value = { new HashEntry(key, JsonSerializer.SerializeToUtf8Bytes(obj)) };

            await _redisCache.HashSetAsync(hash, value);
        }

        /// <summary>
        /// Obtém uma lista de chaves a partir de um hash
        /// </summary>
        /// <param name="hash">Dictionary hash</param>
        /// <returns>All dictionary keys</returns>
        public async Task<string[]> GetKeysByHashAsync(string hash)
        {
            var result = await _redisCache.HashKeysAsync(hash);
            if (result.Any())
                return Array.ConvertAll(result, x => (string)x);
            else
                return null;
        }

        /// <summary>
        /// Obtém um objeto <T> a partir do hash e da key
        /// </summary>
        /// <param name="hash">Dictionary hash</param>
        /// <param name="key">Dictionary key</param>
        /// <returns>A value field</returns>
        public async Task<T> GetObjectAsync(string hash, string key)
        {
            var result = await _redisCache.HashGetAsync(hash, key);
            if (result.HasValue)
                return JsonSerializer.Deserialize<T>(result);
            else
                return null;
        }

        /// <summary>
        /// Obtém todos os objetos a partir de um hash
        /// </summary>
        /// <param name="hash">Dictionary hash</param>
        /// <returns>All values dictionary</returns>
        public async Task<IEnumerable<T>> GetAllObjectsAsync(string hash)
        {
            return _redisCache.HashGetAllAsync(hash).Result.Select(x => JsonSerializer.Deserialize<T>(x.Value));
        }

        #endregion

        #region Métodos simples

        /// <summary>
        /// Adiciona um chave-valor string-string
        /// </summary>
        /// <param name="key">Field Key</param>
        /// <param name="obj">Field Value</param>
        public async Task SetAsync(string key, string obj)
        {
            await _redisCache.StringSetAsync(key, obj);
        }

        /// <summary>
        /// Adiciona um chave-valor string-int
        /// </summary>
        /// <param name="key">Field Key</param>
        /// <param name="obj">Field Value</param>
        public async Task SetAsync(string key, int obj)
        {
            await _redisCache.StringSetAsync(key, obj);
        }

        /// <summary>
        /// Adiciona um chave-valor string-byte[]
        /// </summary>
        /// <param name="key">Field Key</param>
        /// <param name="obj">Field Value</param>
        public async Task SetAsync(string key, byte[] obj)
        {
            await _redisCache.StringSetAsync(key, obj);
        }

        /// <summary>
        /// Obtém um registro com base em uma chave
        /// </summary>
        /// <param name="key">Field Key</param>
        /// <returns>String of key-value</returns>
        public async Task<string> GetAsync(string key)
        {
            return await _redisCache.StringGetAsync(key);
        }

        #endregion
    }
}
