namespace RedisRepository.Repositories.Interfaces
{
    public interface IRedisRepository<T> where T : class
    {
        #region Contratos hash

        public void SetWithJson<TO>(string hash, string key, TO obj);
        public void SetWithBytes<TO>(string hash, string key, TO obj);
        string[] GetKeysByHash(string hashKey);
        public T GetObject(string hash, string key);
        public IEnumerable<T> GetAllObjects(string hash);

        #endregion

        #region Contratos simples

        public void Set(string key, string obj);
        public void Set(string key, int obj);
        public void Set(string key, byte[] obj);
        public string Get(string key);

        #endregion
    }
}
