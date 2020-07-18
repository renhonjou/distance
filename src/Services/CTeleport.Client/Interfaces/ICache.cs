namespace CTeleport.Client.Interfaces
{
    public interface ICache
    {
        string GetCachedData(string key);
        void PutCachedData(string key, string data, int expiredInMinutes);
        bool Exists(string key);
    }
}
