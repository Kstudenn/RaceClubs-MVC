using RunGroupWebApp.Models;

namespace RunGroupWebApp.Data.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> getAllUserClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTrack(string id);
        bool Update(AppUser user);
         bool Save();
    }
}
