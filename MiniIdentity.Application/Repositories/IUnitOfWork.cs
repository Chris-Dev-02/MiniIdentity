namespace MiniIdentity.Application.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
