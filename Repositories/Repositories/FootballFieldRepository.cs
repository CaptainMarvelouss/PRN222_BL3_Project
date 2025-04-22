using BusinessObjects;
using DataAccessObjects;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class FootballFieldRepository : IFootballFieldRepository
    {
        public List<Field> GetFootballFields() => FootballFieldDAO.GetFootballFields();

        public Field GetFootballFieldById(int id) => FootballFieldDAO.GetFootballFieldById(id);

        public void AddFootballField(Field field) => FootballFieldDAO.AddFootballField(field);

        public void UpdateFootballField(Field field) => FootballFieldDAO.UpdateFootballField(field);

        public void DeleteFootballField(Field field) => FootballFieldDAO.DeleteFootballField(field);
    }
}
