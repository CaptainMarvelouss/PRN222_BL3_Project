using BusinessObjects;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public List<User> GetUsers() => UserDAO.GetUsers();

        public User GetUserById(int id) => UserDAO.GetUserById(id);

        public void AddUser(User user) => UserDAO.AddUser(user);

        public void BlockUser(int id) => UserDAO.BlockUser(id);

        public void UnblockUser(int id) => UserDAO.UnblockUser(id);

        public void UpdateUser(User user) => UserDAO.UpdateUser(user);
    }
}
