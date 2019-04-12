using FirstWebApp.Models;
using System.Collections.Generic;

namespace FirstWebApp.Repositories
{
    public interface IUserRepository
    {
        bool Delete(User author);
        List<User> Fetch(int? authorId = null);
        User RetrieveById(int authorId);
        bool Save(User author);
    }
}
