using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using social_media.Data;
using soicalMedia.DTOs;
using soicalMedia.Entity;
using soicalMedia.Helpers;
using soicalMedia.Interfaces;

namespace soicalMedia.Data
{
    public class MassageRepository : IMessageRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public MassageRepository(DataContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //---------------------------------------------------
        public void AddMessage(Massage massage)
        {
            context.Messages.Add(massage);  
        }

        public void DeleteMeassage(Massage massage)
        {
            context.Messages.Remove(massage);
        }

        public async Task<Massage> GetMassage(int id)
        {
            return await context.Messages
                .Include(u => u.Sender)
                .Include(u => u.Recipient)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<MassageDto>> GetMassagesForUser(MassageParams massageParams)
        {
            var query = context.Messages
                 .OrderByDescending(m => m.MessageSent)
                 .AsQueryable();

            query = massageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == massageParams.Username && u.RecipientDeleted==false),
                "Outbox" => query.Where(u => u.Sender.UserName == massageParams.Username && u.SenderDeleted==false),
                _ => query.Where(u => u.Recipient.UserName == massageParams.Username && u.DateRead == null),
            };

            var massages = query.ProjectTo<MassageDto>(mapper.ConfigurationProvider);

            return await PagedList<MassageDto>.CreateAsync(massages,  massageParams.PageNumber, massageParams.PageSize);

        }

        public async Task<IEnumerable<MassageDto>> GetMassageThread(string currentUsername, string recipientUsername)
        {
            var massages = await context.Messages
                .Include(u=>u.Sender).ThenInclude(p=>p.Photos)
                .Include(u=>u.Recipient).ThenInclude(p=>p.Photos)
                .Where(m => m.Recipient.UserName == currentUsername
                        && m.Sender.UserName == recipientUsername
                        || m.Recipient.UserName == recipientUsername
                        && m.Sender.UserName == currentUsername
                 )
                .OrderBy(m=>m.MessageSent)
                .ToListAsync();

            var unreadMassages = massages.Where(m => m.DateRead == null
                && m.Recipient.UserName == currentUsername).ToList();

            if (unreadMassages.Any()) 
            {
                foreach (var massage in unreadMassages)
                { 
                  massage.DateRead = DateTime.Now;
                }

                await context.SaveChangesAsync();
            }

            return mapper.Map<IEnumerable<MassageDto>>(massages);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
