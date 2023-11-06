using Microsoft.EntityFrameworkCore;
using UserService.Interfaces;
using UserService.Models;

namespace UserService.Controllers;

public class UserController
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByIdAndDomainAsync(Guid userId, string domain) =>
        await _userRepository.GetByIdAndDomainAsync(userId, domain);
    

    public async Task<List<User>> GetByDomainWithPagination(string domain, int pageIndex, int pageSize) =>
        await _userRepository.GetAll()
            .Where(q=>q.Domain == domain)
            .Skip((pageIndex-1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<List<User>> GetByTagValueAndDomain(string value, string domain) =>
        await _userRepository.GetAll()
            .Where(q=>q.Tags
                .Any(a=>a.Value == value && a.Domain == domain))
            .ToListAsync();
}