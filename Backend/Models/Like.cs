using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace X.Models;

public class Like
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [ForeignKey("Post")]
    public int PostId { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; } = string.Empty;
    public virtual Post? Post { get; set; } // Navigation to the Post
    public virtual User? User { get; set; } // Navigation to the Post

}