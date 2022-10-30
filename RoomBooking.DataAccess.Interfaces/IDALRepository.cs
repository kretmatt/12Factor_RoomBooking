using System.Linq.Expressions;

namespace RoomBooking.DataAccess.Interfaces;

public interface IDALRepository<TEntity> where TEntity:class
{
    IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby, string includeProperties);

    TEntity GetById(object id);

    void Insert(TEntity entity);

    void Delete(TEntity entity);

    void Update(TEntity entity);
}