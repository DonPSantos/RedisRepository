namespace RedisRepository.Repositories.Interfaces
{
    public interface IRedisRepositoryAsync<T> where T : class
    {
        #region Contratos hash

        public Task SetWithJsonAsync<TO>(string hash, string key, TO obj);

        public Task SetWithBytesAsync<TO>(string hash, string key, TO obj);

        public Task<string[]> GetKeysByHashAsync(string hashKey);

        public Task<T> GetObjectAsync(string hash, string key);

        public Task<IEnumerable<T>> GetAllObjectsAsync(string hash);

        #endregion Contratos hash

        #region Contratos simples

        public Task SetAsync(string key, string obj);

        public Task SetAsync(string key, int obj);

        public Task SetAsync(string key, byte[] obj);

        public Task<string> GetAsync(string key);

        #endregion Contratos simples
    }
}