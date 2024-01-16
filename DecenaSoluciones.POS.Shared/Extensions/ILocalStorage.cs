using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Extensions
{
    public interface ILocalStorage
    {
        public Task SaveStorage<T>(string key, T item) where T : class;
        public Task<T?> GetStorage<T>(string key) where T : class;
        public void RemoveFromStorage(string key);
    }
}
