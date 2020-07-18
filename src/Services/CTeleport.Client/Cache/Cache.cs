using System.Collections.Generic;
using CTeleport.Client.Interfaces;

namespace CTeleport.Client.Cache
{
    public class Cache : ICache
    {
        private Dictionary<string, string> _repository;

        private IDictionary<string, string> Repository => _repository ??= new Dictionary<string, string>();

        public bool Exists(string key)
        {
            return Repository.ContainsKey(key);
        }

        public void PutCachedData(string key, string data)
        {
            if (Exists(key))
            {
                Repository[key] = data;
            }
            else
            {
                Repository.Add(key, data);
            }
        }

        public string GetCachedData(string key)
        {
            return Exists(key) ? Repository[key] : null;
        }
    }
}
