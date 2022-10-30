namespace RoomBooking.BusinessLogic.Interfaces;

public interface ISimpleCRUDLogic<TEntity> where TEntity : class
{
    IEnumerable<TEntity> ReadAll();
    TEntity ReadById(int id);
    void Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(int id);
}