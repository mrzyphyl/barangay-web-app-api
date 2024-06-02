using Core.Service.Base.Service;
using Core.Service.Models;
using Core.Service.Repositories;

namespace Core.Service.Web.User {

    public partial class UserService : BaseServiceQuery<Models.User, UserRepository, UserState> { 
    }

    public partial class UserService {
        public void Save(Models.User user) {
            if (user == null)
                throw new InvalidOperationException("Can't save empty user");

            base.Save(user);
        }
    }
}
