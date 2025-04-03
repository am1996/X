using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X.Models;
public class Post{
    [Key]
    public int Id { get; set; }
    required public string Title { get; set; }
    required public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public int? Likes { get; set; } = 0;
    public int? Dislikes { get; set; } = 0;
    [ForeignKey("User")]
    public string? UserId { get; set; }
}