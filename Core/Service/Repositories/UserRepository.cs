using Core.Service.Base.Configs;
using Core.Service.Base.DataSets;
using Core.Service.Models;

namespace Core.Service.Repositories
{

    public class UserRepository : BaseRepository<AppDbContext, User>, IUserRepository {
    }

    public interface IUserRepository : IBaseRepository<User> {
    }
}
