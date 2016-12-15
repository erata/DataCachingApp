using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCachingApp.Repository.Interfaces
{
    public interface ICacheRepository<T> where T : class
    {
        T Get(int id);

        List<T> GetList();

        T GetOrSet(int id, Func<T> getItemCallback);

        void Add(T data);

        void AddList(List<T> list);

        void Update(T item);

        void Delete(int id);

        int Count() ;

    }
}
