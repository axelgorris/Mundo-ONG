using Microsoft.WindowsAzure.MobileServices;
using NGODirectory.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Linq.Expressions;

namespace NGODirectory.Services
{
    public class AzureCloudTable<T> : ICloudTable<T> where T : TableData
    {
        IMobileServiceSyncTable<T> table;

        public AzureCloudTable(MobileServiceClient client)
        {
            table = client.GetSyncTable<T>();
        }

        #region ICloudTable implementation
        public async Task<T> CreateItemAsync(T item)
        {
            await table.InsertAsync(item);
            return item;
        }

        public async Task DeleteItemAsync(T item)
        {
            await table.DeleteAsync(item);
        }

        public async Task<ICollection<T>> ReadAllItemsAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<ICollection<T>> ReadItemsAsync(int start, int count)
        {
            return await table.Skip(start).Take(count).ToListAsync();
        }

        public async Task<ICollection<T>> ReadAllItemsOrderedAsync<TKey>(Expression<Func<T, TKey>> orderbyExpression = null, bool descending = false)
        {
            if (descending)
                return await table.OrderByDescending(orderbyExpression).ToListAsync();
            else
                return await table.OrderBy(orderbyExpression).ToListAsync();
        }

        public async Task<ICollection<T>> ReadItemsOrderedAsync<TKey>(int start, int count, Expression<Func<T, TKey>> orderby = null, bool descending = false)
        {
            if (descending)
                return await table.OrderByDescending(orderby).Skip(start).Take(count).ToListAsync();
            else
                return await table.OrderBy(orderby).Skip(start).Take(count).ToListAsync();
        }

        public async Task<ICollection<T>> SearchAllItemsOrderedAsync<TKey>(Expression<Func<T, bool>> searchExpression, Expression < Func<T, TKey>> orderbyExpression = null, bool descending = false)
        {
            if (descending)
                return await table  .Where(searchExpression)
                                    .OrderByDescending(orderbyExpression)                                  
                                    .ToListAsync();
            else
                return await table
                                    .Where(searchExpression)
                                    .OrderBy(orderbyExpression)
                                    .ToListAsync();
        }

        public async Task<T> ReadItemAsync(string id)
        {
            return await table.LookupAsync(id);
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            await table.UpdateAsync(item);
            return item;
        }

        public async Task PullAsync()
        {
            string queryName = $"incsync_{typeof(T).Name}";

            await table.PullAsync(queryName, table.CreateQuery());
        }

        #endregion
    }
}
