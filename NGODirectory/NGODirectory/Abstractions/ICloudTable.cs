using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NGODirectory.Abstractions
{
    public interface ICloudTable<T> where T : TableData
    {
        Task<T> CreateItemAsync(T item);
        Task<T> ReadItemAsync(string id);
        Task<T> UpdateItemAsync(T item);
        Task DeleteItemAsync(T item);
        Task<ICollection<T>> ReadAllItemsAsync<TKey>(Expression<Func<T, TKey>> orderby);
        Task<ICollection<T>> ReadItemsAsync<TKey>(int start, int count, Expression<Func<T, TKey>> orderby = null);
        Task PullAsync();
    }
}
