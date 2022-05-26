using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using social_media.Data;
using social_media.Entity;
using soicalMedia.DTOs;
using soicalMedia.Helpers;
using soicalMedia.Interfaces;

namespace soicalMedia.Data;

    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UserRepository(DataContext context,IMapper mapper)
        {
                this.context = context;
            this.mapper = mapper;
        }

        //--------------------------------------------------------------
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await context.Users
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username); 
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(p=>p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
             context.Entry(user).State = EntityState.Modified;
        }
        public async Task<MemberDtos> GetMemberByUsernameAsync(string username)
        {
            return await context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDtos>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDtos>> GetMembersAsync(UserParams userParams)
        {
            var query = context.Users
                .ProjectTo<MemberDtos>(mapper.ConfigurationProvider)
                .AsNoTracking();

            return await PagedList<MemberDtos>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
      
        }

    }

