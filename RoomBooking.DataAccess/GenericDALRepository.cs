using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomBooking.DataAccess.Interfaces;

namespace RoomBooking.DataAccess;

public class GenericDALRepository<TEntity>:IDALRepository<TEntity> where TEntity:class
{
    private RoomBookingContext _context;
    private DbSet<TEntity> _dbSet;
    private ILogger<GenericDALRepository<TEntity>> _logger;

    public GenericDALRepository(RoomBookingContext context, ILogger<GenericDALRepository<TEntity>> logger)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
        _logger = logger;
    }

    public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby, string includeProperties)
    {
        _logger.LogInformation("Beginning building the query for retrieving the data");
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            _logger.LogInformation("Applying the filter for the query");
            query = query.Where(filter);
        }
        
        includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(
                property =>
                {
                    _logger.LogInformation($"Including property {property}");
                    query = query.Include(property);
                }
            );

        _logger.LogInformation("Retrieving data from the database");
        
        return orderby != null ? orderby(query).ToList() : query.ToList();
    }

    public virtual TEntity GetById(object id)
    {
        _logger.LogInformation($"Retrieving entity of type {typeof(TEntity)} with id {id}");
        return _dbSet.Find(id);
    }

    public virtual void Insert(TEntity entity)
    {
        _logger.LogInformation($"Inserting new entity of type {typeof(TEntity)}");
        _dbSet.Add(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        _logger.LogInformation($"Removing entity of type {typeof(TEntity)}");
        _dbSet.Remove(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _logger.LogInformation($"Updating entity of type {typeof(TEntity)}");
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
}