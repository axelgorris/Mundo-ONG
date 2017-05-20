﻿using Microsoft.WindowsAzure.MobileServices;
using NGODirectory.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NGODirectory.Services
{
    public class AzureCloudTable<T> : ICloudTable<T> where T : TableData
    {
        IMobileServiceTable<T> table;

        public AzureCloudTable(MobileServiceClient client)
        {
            this.table = client.GetTable<T>();
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

        public async Task<T> ReadItemAsync(string id)
        {
            return await table.LookupAsync(id);
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            await table.UpdateAsync(item);
            return item;
        }
        #endregion
    }
}
