
using System.Collections.Generic;

namespace Biblio.Data
{
    public interface IRepository<T> where T : class
    {
        bool Create(T entity);
        T Read(int id);
        bool Update(T entity);
        bool Delete(int id);
        List<T> GetAll();
    }
}

