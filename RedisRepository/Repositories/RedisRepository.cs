using RedisRepository.Repositories.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisRepository.Repositories
{
    public class RedisRepository<T> : IRedisRepository<T> where T : class
    {
        private readonly IDatabase _redisCache;

        public RedisRepository(IConnectionMultiplexer connection)
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
        public void SetWithJson<TO>(string hash, string key, TO obj)
        {
            HashEntry[] value = { new HashEntry(key, JsonSerializer.Serialize(obj)) };

            _redisCache.HashSet(hash, value);
        }

        /// <summary>
        /// Adiciona um objeto serializado como Array de bytes
        /// </summary>
        /// <typeparam name="TO">Any</typeparam>
        /// <param name="hash">Dictionary hash</param>
        /// <param name="key">Dictionary key</param>
        /// <param name="obj">Dictionary obj</param>
        public void SetWithBytes<TO>(string hash, string key, TO obj)
        {
            HashEntry[] value = { new HashEntry(key, JsonSerializer.SerializeToUtf8Bytes(obj)) };

            _redisCache.HashSet(hash, value);
        }

        /// <summary>
        /// Obtém uma lista de chaves a partir de um hash
        /// </summary>
        /// <param name="hash">Dictionary hash</param>
        /// <returns>All dictionary keys</returns>
        public string[] GetKeysByHash(string hash)
        {
            if (!string.IsNullOrEmpty(hash))
            {
                var result = _redisCache.HashKeys(hash);

                if (result.Any())
                    return Array.ConvertAll(result, x => (string)x);
                else
                    return Array.Empty<string>();
            }
            else
                return Array.Empty<string>();
        }

        /// <summary>
        /// Obtém um objeto <T> a partir do hash e da key
        /// </summary>
        /// <param name="hash">Dictionary hash</param>
        /// <param name="key">Dictionary key</param>
        /// <returns>A value field</returns>
        public T GetObject(string hash, string key)
        {
            var result = _redisCache.HashGet(hash, key);
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
        public IEnumerable<T> GetAllObjects(string hash)
        {
            if (!string.IsNullOrEmpty(hash))
            {
                var result = _redisCache.HashGetAll(hash);

                return result.Any() ? result.Select(x => JsonSerializer.Deserialize<T>(x.Value)): Enumerable.Empty<T>();
            }else
                return Enumerable.Empty<T>();
        }

        #endregion

        #region Métodos simples

        /// <summary>
        /// Adiciona um chave-valor string-string
        /// </summary>
        /// <param name="key">Field Key</param>
        /// <param name="obj">Field Value</param>
        public void Set(string key, string obj)
        {
            _redisCache.StringSet(key, obj);
        }

        /// <summary>
        /// Adiciona um chave-valor string-int
        /// </summary>
        /// <param name="key">Field Key</param>
        /// <param name="obj">Field Value</param>
        public void Set(string key, int obj)
        {
            _redisCache.StringSet(key, obj);
        }

        /// <summary>
        /// Adiciona um chave-valor string-byte[]
        /// </summary>
        /// <param name="key">Field Key</param>
        /// <param name="obj">Field Value</param>
        public void Set(string key, byte[] obj)
        {
            _redisCache.StringSet(key, obj);
        }

        /// <summary>
        /// Obtém um registro com base em uma chave
        /// </summary>
        /// <param name="key">Field Key</param>
        /// <returns>String of key-value</returns>
        public string Get(string key)
        {
            return _redisCache.StringGet(key);
        }

        #endregion
    }
}
