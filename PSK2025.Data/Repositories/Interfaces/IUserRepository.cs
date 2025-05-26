// File: Interfaces/IUserRepository.cs
using PSK2025.Models.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PSK2025.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}