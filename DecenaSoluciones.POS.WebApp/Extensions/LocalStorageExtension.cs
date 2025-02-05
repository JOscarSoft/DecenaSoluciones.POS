using Blazored.LocalStorage;
using DecenaSoluciones.POS.Shared.Extensions;
using System.Text.Json;

namespace DecenaSoluciones.POS.WebApp.Extensions
{
    public class LocalStorageExtension : ILocalStorage
    {
        private readonly ILocalStorageService sessionStorageService;
        public LocalStorageExtension(ILocalStorageService sessionStorageService)
        {
            this.sessionStorageService = sessionStorageService;
        }

        public async Task SaveStorage<T>(
            string key, T item
            ) where T : class
        {
            var itemJson = JsonSerializer.Serialize(item);
            await sessionStorageService.SetItemAsStringAsync(key, itemJson);
        }

        public async Task<T?> GetStorage<T>(
        string key
        ) where T : class
        {
            var itemJson = await sessionStorageService.GetItemAsStringAsync(key);

            if (itemJson != null)
            {
                var item = JsonSerializer.Deserialize<T>(itemJson);
                return item;
            }
            else
                return null;
        }

        public void RemoveFromStorage(string key)
        {
            sessionStorageService.RemoveItemAsync(key);
        }
    }
}
