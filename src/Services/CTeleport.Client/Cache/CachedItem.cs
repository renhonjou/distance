using System;

namespace CTeleport.Client.Cache
{
    public class CachedItem
    {
        public CachedItem(string value, DateTime expired)
        {
            Value = value;
            Expired = expired;
        }

        public string Value { get; }
        public DateTime Expired { get; }
    }
}
