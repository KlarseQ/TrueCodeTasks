namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAndDomainAsync(Guid userId, string domain)
    {
        return await _context.Users.Include(u=>u.Tags).AsNoTracking()
            .FirstOrDefaultAsync(f => f.UserId == userId && f.Domain == domain);
    }

    public IQueryable<User> GetAll() =>
        _context.Users.Include(u => u.Tags).AsNoTracking().AsQueryable();
}