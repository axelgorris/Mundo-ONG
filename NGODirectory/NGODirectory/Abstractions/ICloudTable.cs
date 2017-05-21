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
        Task<ICollection<T>> ReadAllItemsAsync();
        Task<ICollection<T>> ReadItemsAsync(int start, int count);
        Task<ICollection<T>> ReadAllItemsOrderedAsync<TKey>(Expression<Func<T, TKey>> orderby, bool descending = false);
        Task<ICollection<T>> ReadItemsOrderedAsync<TKey>(int start, int count, Expression<Func<T, TKey>> orderby = null, bool descending = false);
        Task PullAsync();
    }
}
