using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class FootballFieldDAO
    {
        public static List<Field> GetFootballFields()
        {
            var list = new List<Field>();
            try
            {
                using var context = new FootballFieldBookingContext();
                list = context.Fields.ToList();
            }
            catch (Exception ex)
            {
               
            }
            return list;
        }

        public static Field GetFootballFieldById(int id)
        {
            using var context = new FootballFieldBookingContext();
            return context.Fields.FirstOrDefault(f => f.FieldId.Equals(id));
        }

        public static void AddFootballField(Field field)
        {
            try
            {
                using var context = new FootballFieldBookingContext();
                context.Fields.Add(field);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }
        }

        public static void UpdateFootballField(Field field)
        {
            try
            {
                using var context = new FootballFieldBookingContext();

                var existingField = context.Fields.AsNoTracking().FirstOrDefault(f => f.FieldId == field.FieldId);
                if (existingField == null)
                {
                    throw new Exception("Field not found.");
                }
                field.CreatedAt = existingField.CreatedAt;

                context.Entry<Field>(field).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }
        }

        public static void DeleteFootballField(Field field)
        {
            try
            {
                using var context = new FootballFieldBookingContext();
                var check = context.Fields.SingleOrDefault(f => f.FieldId == field.FieldId);
                context.Fields.Remove(check);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
