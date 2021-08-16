using CommandAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandAPI.Data
{
    public interface ICommandAPIRepo <T> where T : class
    {
        bool SaveChanges();
        IEnumerable<T> GetAllCommands();
        T GetCommandById(int Id);
        void CreateCommand(T entity);
        void UpdateCommand(T entity);
        void DeleteCommand(T entity);
    }
}
