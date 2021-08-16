using CommandAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandAPI.Data
{
    public class SqlGenericRepo<T> : ICommandAPIRepo<T> where T : class
    {
        private readonly CommandContext _context;

        public SqlGenericRepo(CommandContext injectedContext)
        {
            _context = injectedContext;
        }

        public void CreateCommand(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void DeleteCommand(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAllCommands()
        {
            return _context.Set<T>();
        }

        public T GetCommandById(int Id)
        {
            return _context.Set<T>().Find(Id);
        }

        public bool SaveChanges()
        {
           return _context.SaveChanges() > 0;
        }

        public void UpdateCommand(T entity)
        {

        }
    }
}
