using DecenaSoluciones.POS.Shared.Extensions;
using System.Text.Json;

namespace DecenaSoluciones.POS.App.Extensions
{
    public class LocalStorageExtension : ILocalStorage
    {
        public async Task SaveStorage<T>(
            string key, T item
            ) where T : class
        {

            var itemJson = JsonSerializer.Serialize(item);
            await SecureStorage.Default.SetAsync(key, itemJson);

        }

        public async Task<T?> GetStorage<T>(
        string key
        ) where T : class
        {
            var itemJson = await SecureStorage.Default.GetAsync(key);

            if (itemJson != null)
            {
                var item = JsonSerializer.Deserialize<T>(itemJson);
                return item;
            }
            else
                return null;
        }

        public void RemoveFromStorage(string key) => SecureStorage.Default.Remove(key);
    }
}
