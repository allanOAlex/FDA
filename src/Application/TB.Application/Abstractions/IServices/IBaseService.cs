namespace TB.Application.Abstractions.IServices
{
    public interface IBaseService<T> where T : class
    {
        Task<T> Create(T request);
        Task<List<T>> FindAll();
        Task<T> FindById(int Id);
        Task<T> Update(T request);
        Task<T> Delete(T request);
    }
}
