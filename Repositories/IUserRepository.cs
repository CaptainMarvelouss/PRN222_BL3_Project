using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        void UpdateUser(User user);
        void BlockUser(int id);
        void UnblockUser(int id);
        List<User> GetUsers();
        User GetUserById(int id);
    }
}
