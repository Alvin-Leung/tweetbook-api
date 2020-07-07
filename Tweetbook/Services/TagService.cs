using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class TagService : IDataService<Tag, string>
    {
        private readonly DataContext dataContext;

        public TagService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await this.dataContext.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<bool> DeleteAsync(string tagName)
        {
            var tagToDelete = await this.dataContext.Tags.FirstOrDefaultAsync(tag => tag.Name == tagName);

            if (tagToDelete == null)
            {
                return false;
            }

            this.dataContext.Tags.Remove(tagToDelete);
            var numDeleted = await this.dataContext.SaveChangesAsync();

            return numDeleted > 0;
        }

        public Task<Tag> GetAsync(string tagName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAsync(Tag newTag)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Tag item)
        {
            throw new NotImplementedException();
        }
    }
}
