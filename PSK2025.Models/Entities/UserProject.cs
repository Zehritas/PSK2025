namespace PSK2025.Models.Entities;

public class UserProject
{
    public string UserId { get; set; } = null!;
    public Guid ProjectId { get; set; }

    public User User { get; set; } = null!;
    public Project Project { get; set; } = null!;
}