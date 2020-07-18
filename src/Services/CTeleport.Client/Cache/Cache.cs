using System;
using System.Collections.Generic;
using CTeleport.Client.Interfaces;

namespace CTeleport.Client.Cache
{
    public class Cache : ICache
    {
        private Dictionary<string, CachedItem> _repository;

        private IDictionary<string, CachedItem> Repository => _repository ??= new Dictionary<string, CachedItem>();

        public bool Exists(string key)
        {
            return Repository.ContainsKey(key);
        }

        public void PutCachedData(string key, string data, int expiredInMinutes)
        {
            var item = new CachedItem(data, DateTime.UtcNow.AddMinutes(expiredInMinutes));
            if (Exists(key))
            {
                Repository[key] = item;
            }
            else
            {
                Repository.Add(key, item);
            }
        }

        public string GetCachedData(string key)
        {
            if (!Exists(key)) return null;

            var item = Repository[key];
            return item.Expired > DateTime.UtcNow ? item.Value : null;
        }
    }
}
