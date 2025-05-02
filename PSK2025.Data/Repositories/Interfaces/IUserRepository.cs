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
        // Add more methods like AddAsync, UpdateAsync, DeleteAsync, etc. as needed
    }
}