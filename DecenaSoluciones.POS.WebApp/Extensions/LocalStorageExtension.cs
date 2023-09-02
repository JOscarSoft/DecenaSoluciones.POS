using Blazored.LocalStorage;
using System.Text.Json;

namespace DecenaSoluciones.POS.WebApp.Extensions
{
    public static class LocalStorageExtension
    {
        public static async Task SaveStorage<T>(
            this ILocalStorageService sessionStorageService,
            string key, T item
            ) where T : class
        {

            var itemJson = JsonSerializer.Serialize(item);
            await sessionStorageService.SetItemAsStringAsync(key, itemJson);

        }

        public static async Task<T?> GetStorage<T>(
        this ILocalStorageService sessionStorageService,
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


    }
}
