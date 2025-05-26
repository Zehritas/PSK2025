namespace PSK2025.Models.Entities;

public class UserProject
{
    public string UserId { get; set; } = null!;
    public Guid ProjectId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Project Project { get; set; } = null!;
}