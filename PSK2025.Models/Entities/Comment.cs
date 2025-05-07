namespace PSK2025.Models.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string UserId { get; set; } = null!;

    public string Content { get; set; } = string.Empty;

    public Project Project { get; set; } = null!;
    public User User { get; set; } = null!;
}