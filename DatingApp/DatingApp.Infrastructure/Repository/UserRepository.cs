using DatingApp.Domain.Models;
using DatingApp.Infrastructure.DbContexts;
using DatingApp.Infrastructure.Helpers;
using DatingApp.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == username.ToLower());
        }

        public async Task<PagedList<AppUser>> GetUsersAsync(int pageNumber, int pageSize,string currentUsername,string gender, int minAge, int maxAge)
        {
            var query = _dbContext.Users.Include(x=>x.Photos).AsQueryable();

            query = query.Where(x => x.UserName != currentUsername);

            query = query.Where(x => x.Gender == gender);

            var minDob = DateTime.Today.AddYears(-maxAge - 1);

            var maxDob = DateTime.Today.AddYears(-minAge);

            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            return await PagedList<AppUser>.CreateAsync(query.AsNoTracking(), pageNumber, pageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
        }
    }
}
