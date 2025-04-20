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
    [ForeignKey("User")]
    public string? UserId { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<Like>? Likes { get; set; }
    public virtual ICollection<Dislike>? Dislikes { get; set; }
}