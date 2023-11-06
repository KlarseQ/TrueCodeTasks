namespace UserServiceTests;

[TestFixture]
public class UserControllerTests
{
    private AppDbContext _dbContext = null!;
    private UserController _controller = null!;

    private static IEnumerable<Tag> GetListTagsForTest(int count) => Enumerable.Range(0, count)
        .Select(_ => new Tag { Value = Guid.NewGuid().ToString(), Domain = Guid.NewGuid().ToString() });

    private static IEnumerable<User> GetListUsersForTest(int count) => Enumerable.Range(0, count)
        .Select(_ => new User { Name = Guid.NewGuid().ToString(), Domain = Guid.NewGuid().ToString() });

    private void CreateUsersWithTagsForTest(IEnumerable<User> users)
    {
        _dbContext.Users.AddRange(users);
        _dbContext.SaveChanges();
    }
    
    private static Tuple<List<User>, List<Tag>> GetMixedData()
    {
        var users = GetListUsersForTest(5).ToList();
        var tags = GetListTagsForTest(5).ToList();
        
        var random = new Random();

        foreach (var user in users)
        {
            var selectedTags = new List<Tag>();

            while (selectedTags.Count < 2)
            {
                var randomIndex = random.Next(tags.Count);
                var selectedTag = tags[randomIndex];

                if (!selectedTags.Contains(selectedTag))
                {
                    selectedTags.Add(selectedTag);
                }
            }

            user.Tags.AddRange(selectedTags);
        }

        return new Tuple<List<User>, List<Tag>>(users, tags);
    }
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var connectionString = configuration.GetValue<string>("TestAppConnection");
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        _dbContext = new AppDbContext(options);
        _controller = new UserController(new UserRepository(_dbContext));
        
        _dbContext.Database.Migrate();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _dbContext.Database.ExecuteSqlRaw("DELETE FROM TagToUser");
        _dbContext.Database.ExecuteSqlRaw("DELETE FROM Users");
        _dbContext.Database.ExecuteSqlRaw("DELETE FROM Tags");
    }

    [Test]
    public void GetUserByIdAndDomainTest()
    {
        #region Prepare

        var (users, tags) = GetMixedData();

        CreateUsersWithTagsForTest(users);

        var firstUser = users.First();
        
        #endregion

        var user = _controller.GetByIdAndDomainAsync(firstUser.UserId, firstUser.Domain).Result;
        
        Assert.Multiple(() =>
        {
            Assert.That(user, Is.Not.Null);
            Assert.That(user!.UserId == firstUser.UserId && user.Domain == firstUser.Domain &&
                        user.Tags.Count == firstUser.Tags.Count, Is.True);
        });
    }

    [Test]
    public void GetByDomainWithPaginationTest()
    {
        #region Prepare

        var (users, tags) = GetMixedData();

        var firstUser = users.First();
        var lastUser = users.Last();
        lastUser.Domain = firstUser.Domain;
        
        CreateUsersWithTagsForTest(users);
        
        var tagIds = new List<User> { firstUser, lastUser }.SelectMany(user => user.Tags)
            .Select(tag => tag.TagId)
            .ToList();
        
        #endregion

        var usersResult = _controller.GetByDomainWithPagination(firstUser.Domain, 1, 10).Result;
        
        Assert.Multiple(() =>
        {
            Assert.That(usersResult, Is.Not.Null);
            Assert.That(usersResult!, Has.Count.GreaterThan(1));
            Assert.That(usersResult.Any(a => a.Tags.Any(ta => tagIds.Contains(ta.TagId))), Is.True);
        });
    }

    [Test]
    public void GetByTagValueAndDomainTest()
    {
        #region Prepare

        var (users, tags) = GetMixedData();

        CreateUsersWithTagsForTest(users);

        var firstTag = tags.First();
        var userIds = firstTag.Users.Select(s => s.UserId);
        
        #endregion

        var usersResult = _controller.GetByTagValueAndDomain(firstTag.Value, firstTag.Domain).Result;
        
        Assert.Multiple(() =>
        {
            Assert.That(usersResult, Is.Not.Null);
            Assert.That(usersResult.Any(a => userIds.Contains(a.UserId)), Is.True);
        });
    }
}