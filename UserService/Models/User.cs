namespace UserService.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Domain { get; set; } = default!;

    public List<Tag> Tags { get; set; } = new();

    public List<TagToUser> TagsToUsers { get; set; } = new();
}