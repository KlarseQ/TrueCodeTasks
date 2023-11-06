namespace UserService.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAndDomainAsync(Guid userId, string domain);

    IQueryable<User> GetAll();
}