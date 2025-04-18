using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class UserDAO
    {
        public static List<User> GetUsers()
        {
            var list = new List<User>();
            try
            {
                using var context = new FootballFieldBookingContext();
                list = context.Users.Include(u => u.Role).ToList();
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public static User GetUserById(int id)
        {
            using var context = new FootballFieldBookingContext();
            return context.Users.Include(u => u.Role).FirstOrDefault(f => f.UserId.Equals(id));
        }

        public static void AddUser(User user)
        {
            try
            {
                using var context = new FootballFieldBookingContext();
                context.Users.Add(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public static void UpdateUser(User user)
        {
            try
            {
                using var context = new FootballFieldBookingContext();
                context.Entry<User>(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public static void BlockUser(int id)
        {
            try
            {
                using var context = new FootballFieldBookingContext();
                var existingUser = context.Users.FirstOrDefault(u => u.UserId == id);
                if (existingUser == null)
                {
                    throw new Exception("User not found.");
                }
                existingUser.IsBlocked = true;
                context.Entry<User>(existingUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public static void UnblockUser(int id)
        {
            try
            {
                using var context = new FootballFieldBookingContext();
                var existingUser = context.Users.FirstOrDefault(u => u.UserId == id);
                if (existingUser == null)
                {
                    throw new Exception("User not found.");
                }
                existingUser.IsBlocked = false;
                context.Entry<User>(existingUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
