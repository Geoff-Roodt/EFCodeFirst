using System.Threading.Tasks;
using System.Collections.Generic;

namespace Todo.Web.Service.Interfaces
{
    public interface IService<TBase> where TBase : class
    {
        IList<TBase> GetAll();
        TBase GetSingle(int id);

        Task<TBase> Add(TBase item);
        Task<int> Update(TBase item);
        Task<bool> Remove(int id);
    }
}