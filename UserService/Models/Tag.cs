using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class Tag
{
    [Key]
    public Guid TagId { get; set; }

    [Required]
    public string Value { get; set; } = default!;

    [Required]
    public string Domain { get; set; } = default!;

    public List<User> Users { get; set; } = new();

    public List<TagToUser> TagsToUsers { get; set; } = new();
}