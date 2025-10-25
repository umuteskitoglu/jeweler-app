using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Users.Commands;

public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName, string Role) : IRequest<Guid>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with the given email already exists.");
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName,
            request.Role,
            new AuditInfo(DateTime.UtcNow, Guid.Empty));

        await _userRepository.AddAsync(user);
        return user.Id;
    }
}

