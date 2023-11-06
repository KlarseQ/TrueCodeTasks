using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class TagToUser
{
    [Key]
    public Guid EntityId { get; set;}
    
    public Guid? UserId { get; set; }
    
    public Guid? TagId { get; set; }
    
    public User? User { get; set; }
    
    public Tag? Tag { get; set; }
}