using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Models.Entities;

public class RefreshToken
{
    private RefreshToken() { }

    private RefreshToken(Guid id, string token, DateTime expiresAt, string userId)
    {
        Id = id;
        Token = token;
        ExpiresAt = expiresAt;
        UserId = userId;
        Created = DateTime.UtcNow;
        IsRevoked = false;
    }

    public static RefreshToken Create(string token, DateTime expiresAt, string userId)
    {
        return new RefreshToken(Guid.NewGuid(), token, expiresAt, userId);
    }

    public Guid Id { get; private set; }
    public string Token { get; private set; }
    public string UserId { get; private set; }
    public User User { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime Created { get; private set; }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
