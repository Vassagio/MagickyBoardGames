using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagickyBoardGames.Models;

namespace MagickyBoardGames.Repositories
{
    public interface IRepository<T> where T : IEntity {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetBy(int id);
        Task<int> Add(T entity);
        Task Delete(int id);
        Task Update(T entity);
    }
}
 