using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IFootballFieldRepository
    {
        void AddFootballField(Field field);
        void UpdateFootballField(Field field);
        void DeleteFootballField(Field field);
        List<Field> GetFootballFields();
        Field GetFootballFieldById(int id);
    }
}
