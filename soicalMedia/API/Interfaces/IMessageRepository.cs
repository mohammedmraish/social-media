using soicalMedia.DTOs;
using soicalMedia.Entity;
using soicalMedia.Helpers;

namespace soicalMedia.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Massage massage);

        void DeleteMeassage(Massage massage);

        Task<Massage> GetMassage(int id);

        Task<PagedList<MassageDto>> GetMassagesForUser(MassageParams massageParams);

        Task<IEnumerable<MassageDto>> GetMassageThread(string currentUsername, string recipientUsername);

        Task<bool> SaveAllAsync();
    }
}
