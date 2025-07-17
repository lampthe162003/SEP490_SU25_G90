
using System.Collections.Concurrent;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.ResetCodeService
{
    public class ResetCodeStorageService : IResetCodeStorageService
    {
        private readonly ConcurrentDictionary<string, (string code, DateTime expiresAt)> _storage = new();

        public (string Code, DateTime ExpiresAt)? GetCode(string email)
        {
            if (_storage.TryGetValue(email, out var value))
            {
                return value;
            }
            return null;
        }

        public void RemoveCode(string email)
        {
            _storage.TryRemove(email, out _);
        }

        public void SaveCode(string email, string code, TimeSpan expiresIn)
        {
            _storage[email] = (code, DateTime.UtcNow.Add(expiresIn));
        }
    }
}
