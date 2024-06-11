using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data.Repository.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Data.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContext)
        {

            _context = context;
            _httpContext = httpContext;
        }

        public async Task<List<Club>> getAllUserClubs()
        {
            var currentUser = _httpContext.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(r => r.AppUser.Id == currentUser);
            return await userClubs.ToListAsync();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var currentUser = _httpContext.HttpContext?.User.GetUserId();
            var userRaces = _context.Races.Where(r => r.AppUser.Id == currentUser);
            return await userRaces.ToListAsync();
        }
        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);

        }

        public async Task<AppUser> GetByIdNoTrack(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
