public interface IRepository
{
    Task<string> GetById(int id);
}